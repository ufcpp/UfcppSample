using CardBattle.Models;
using CardBattle.Models.GameRules;
using CardBattle.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskLibrary.Channels;
using static TaskLibrary.Channels.TypedAsyncAction<CardBattle.Models.Messages.GameProgress>;

namespace CardBattle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            System.Diagnostics.Debug.WriteLine("ctor " + Thread.CurrentThread.ManagedThreadId);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //TestPlayArea();
            TestEngine();
            //TestApi();
        }

        #region Chanel の listen。いずれ別クラスに移す。

        /// <summary>
        /// 自分のプレイヤーID。
        /// このIDのやつだけ手作業コマンド選択。
        /// 残りはAIプレイのつもり。
        /// </summary>
        const int MyPlayerId = 0;

        private async void TestEngine()
        {
            System.Diagnostics.Debug.WriteLine("begin " + Thread.CurrentThread.ManagedThreadId);
            const int Players = 5;

            var r = new Random();
            var abilities = Enumerable.Range(0, Players).Select(i => GetRandomAbility(r,
#if false
                i // プレイヤーごとに強さを変えたいとき用
#else
                // プレイヤーの強さをそろえたいとき用
                //4 // 勝つ
                //3 // ぎりぎり勝てるくらい
                2 // 負ける
                //1 // 一撃死
#endif
                )).ToArray();

            var c = new Channel<GameProgress>();
            var d = c.ObserveOn().Distribute();

            // MyPlayerId な IResponsiveMessage だけ受け取る
            var playerChannel = d.GetChannel(0);
            playerChannel.Subscribe(
                Create<CommandPrompt>((x, ct) => OnCommandPrompt(x, r)),
                Create<ContinuePrompt>((x, ct) => OnContinuePrompt(x, r))
                );

            // MyPlayerId でない IResponsiveMessage だけ受け取る
            var aiChannel = d.Filter(x => x.Address != null && x.Address != 0, x => x.Message);
            aiChannel.Subscribe(
                Create<CommandPrompt>((x, ct) => OnAiCommandPrompt(x, r)),
                Create<ContinuePrompt>((x, ct) => OnAiContinuePrompt(x, r))
                );

            // その他のメッセージ用
            var generalChannel = d.GetChannel(null);
            generalChannel.Subscribe(
                Create<FinishGame>(OnFinishGame),
                Create<TurnStarted>(OnTurnStarted),
                Create<PlayerAttack>(OnPlayerAttack),
                Create<BossAttack>(OnBossAttack),
                Create<Continue>(OnContinue)
                );

            var engine = new GameEngine(c.WithCancel(CancellationToken.None), r.Next(), abilities);

            //IsEnabled = false;

            try
            {
                playArea.DataContext = engine;
                await c.Completed;
            }
            finally
            {
                //IsEnabled = true;
            }
        }

        private async Task OnContinue(Continue x, CancellationToken ct)
        {
            System.Diagnostics.Debug.WriteLine("OnContinue " + Thread.CurrentThread.ManagedThreadId);
            System.Diagnostics.Debug.WriteLine($"continued : {string.Join(", ", x.Players.Select(p => p.Id))}");

            generalMessage.Visibility = Visibility.Visible;
            generalMessage.Text = $"プレイヤー{string.Join(", ", x.Players.Select(p => p.Id))}がコンティニューしました";
            await Task.Delay(1000);
            generalMessage.Text = "";
            generalMessage.Visibility = Visibility.Collapsed;
        }

        private async Task OnAiContinuePrompt(ContinuePrompt x, Random r)
        {
            System.Diagnostics.Debug.WriteLine("OnAiContinuePrompt " + Thread.CurrentThread.ManagedThreadId);

            var continued = false;
            // 20% の確率でコンティニューさせとく
            if(r.NextDouble() < 0.2)
            {
                x.Confirm();
                continued = true;
            }

            System.Diagnostics.Debug.WriteLine($"continue : ({x.PlayerId}) {continued}");
        }

        private async Task OnContinuePrompt(ContinuePrompt x, Random r)
        {
            System.Diagnostics.Debug.WriteLine("OnContinuePrompt " + Thread.CurrentThread.ManagedThreadId);
            System.Diagnostics.Debug.WriteLine($"continue : ({x.PlayerId})");

            @continue.Visibility = Visibility.Visible;

            var res = await @continue.SelectAsync(CancellationToken.None);

            if (res) x.Confirm();

            @continue.Visibility = Visibility.Collapsed;
        }

        private async Task OnFinishGame(FinishGame x, CancellationToken ct)
        {
            System.Diagnostics.Debug.WriteLine("OnFinishGame " + Thread.CurrentThread.ManagedThreadId);
            System.Diagnostics.Debug.WriteLine($"finished: {x.Status} in turn {x.Turn}, boss: {x.BossHp}, players: {string.Join(", ", x.Players.Select(p => p.Hp))}");
        }

        private async Task OnAiCommandPrompt(CommandPrompt x, Random r)
        {
            System.Diagnostics.Debug.WriteLine("OnAiCommandPrompt " + Thread.CurrentThread.ManagedThreadId);
            System.Diagnostics.Debug.WriteLine($"command : ({x.PlayerId}) {string.Join(", ", x.Hand.Cards)}");

            command.Visibility = Visibility.Visible;
            command.DataContext = x;

            // ランダムに0～4枚 redraw
            var indexes = Enumerable.Range(0, r.Next(0, 5)).Select(_ => r.Next(0, 5)).Distinct().ToArray();

            await command.ShowAsync(indexes, CancellationToken.None);

            if (indexes.Any())
                x.Redraw(indexes);

            command.Visibility = Visibility.Collapsed;
        }

        private async Task OnCommandPrompt(CommandPrompt x, Random r)
        {
            System.Diagnostics.Debug.WriteLine("OnCommandPrompt " + Thread.CurrentThread.ManagedThreadId);
            System.Diagnostics.Debug.WriteLine($"command : ({x.PlayerId}) {string.Join(", ", x.Hand.Cards)}");

            command.Visibility = Visibility.Visible;
            command.DataContext = x;

            var indexes = await command.SelectAsync(CancellationToken.None);

            System.Diagnostics.Debug.WriteLine($"selection: ({x.PlayerId}) {string.Join(", ", indexes)}");

            if (indexes.Any())
                x.Redraw(indexes);

            command.Visibility = Visibility.Collapsed;
        }

        private async Task OnTurnStarted(TurnStarted x, CancellationToken ct)
        {
            System.Diagnostics.Debug.WriteLine("OnTurnStarted " + Thread.CurrentThread.ManagedThreadId);
            System.Diagnostics.Debug.WriteLine($"---- ターン{x.Turn} ----");
        }

        private async Task OnPlayerAttack(PlayerAttack x, CancellationToken ct)
        {
            System.Diagnostics.Debug.WriteLine("OnPlayerAttack " + Thread.CurrentThread.ManagedThreadId);
            System.Diagnostics.Debug.WriteLine($"attacked: ({x.Player.Id}) {string.Join(", ", x.Hand.Cards)} / {x.Damage} => {x.BossHp}");

            await playArea.ShowAttack(x);
        }

        private async Task OnBossAttack(BossAttack x, CancellationToken ct)
        {
            System.Diagnostics.Debug.WriteLine("OnBossAttack " + Thread.CurrentThread.ManagedThreadId);
            System.Diagnostics.Debug.WriteLine($"Boss attacks");

            await playArea.ShowAttack(x);
        }

        #endregion
        #region PlayArea直接テスト

        private static void TestPlayArea()
        {
            const int Players = 5;
            const int Turns = 500;

            var x = new PlayArea(0, Players);
            var r = new Random();

            var abilities = Enumerable.Range(0, Players).Select(i => GetRandomAbility(r, i)).ToArray();

            var sum = new int[Players];

            for (int turn = 0; turn < Turns; turn++)
            {
                System.Diagnostics.Debug.WriteLine($"----- ターン {turn} -----");
                for (int player = 0; player < Players; player++)
                {
                    var p = x.Players[player];
                    var a = abilities[player];
                    var score = Judge.Score(p, a);
                    sum[player] += score;
                    System.Diagnostics.Debug.WriteLine($"{player}: {string.Join(", ", p.Cards)} → {score}");

                    var redrawIndexes = Enumerable.Range(0, r.Next(5)).Select(_ => r.Next(5)).Distinct().ToArray();
                    x.Redraw(player, redrawIndexes);
                }
            }

            System.Diagnostics.Debug.WriteLine("----- 平均 -----");
            for (int player = 0; player < Players; player++)
            {
                var s = sum[player];
                var average = s / (double)Turns;
                System.Diagnostics.Debug.WriteLine($"{player}: {average}");
            }
        }

        #endregion
        #region テスト用のランダムデータ作り

        /// <summary>
        /// テスト用の乱数生成能力値。
        /// レベル帯 0～4で、20以下、40以下、60以下、80以下、100以下、くらいの能力値を想定。
        ///
        /// 試しにやってみたら、
        /// L1帯でスコア5000～1万
        /// L4帯でスコア5万～10万
        /// くらいだったので、HPは同レベル対戦で10ターンくらい続く想定で5万～100万くらいで作ってみる。
        /// </summary>
        /// <param name="random"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private static Ability GetRandomAbility(Random random, int level)
        {
            var i = level;
            var j = level + 1;
            return new Ability(
                (int)(random.Next(2000, 3000) * Math.Pow(2.5, i) * 10), // L1帯で6万、L4帯で97万くらいの平均値になるはず
                random.Next(20 * i, 20 * j),
                random.Next(20 * i, 20 * j),
                random.Next(20 * i, 20 * j),
                random.Next(20 * i, 20 * j),
                random.Next(20 * i, 20 * j),
                random.Next(20 * i, 20 * j),
                random.Next(20 * i, 20 * j),
                random.Next(20 * i, 20 * j)
                );
        }

        #endregion
    }
}
