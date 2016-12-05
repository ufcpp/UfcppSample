using TaskLibrary.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardBattle.Models.GameRules;
using CardBattle.Lib;
using CardBattle.Models.Messages;
using System.Threading;

namespace CardBattle.Models
{
    /// <summary>
    /// 1ゲーム分の実行エンジン。
    /// <see cref="Channel{TMessage}"/>使ってViewとやり取りしつつ、ゲームを進行する。
    /// </summary>
    public class GameEngine
    {
        private readonly PlayArea _area;
        private readonly Player[] _players;
        private readonly Random _random;

        public GameEngine(CancellableReceiver<GameProgress> channel, int randomSeed, Ability[] playerAbilities)
        {
            _random = new Random(randomSeed);

            var num = playerAbilities.Length;
            _area = new PlayArea(randomSeed, num);
            _players = new Player[num];

            for (int i = 0; i < _players.Length; i++)
            {
                // とりあえずサンプルだし、ID = インデックスにしてしまう。
                _players[i] = new Player(i, (byte)i, playerAbilities[i], _area.Players[i]);
            }

            Boss = new Boss();

            channel.Execute(Execute);
        }

        /// <summary>
        /// 参加プレイヤー。
        /// </summary>
        public IReadOnlyList<Player> Players => _players;

        /// <summary>
        /// みんなで倒すべき敵ボス。
        /// </summary>
        public Boss Boss { get; }

        public async Task Execute(CancellableReceiver<GameProgress> channel)
        {
            var result = await ExecuteBody(channel);

            await channel.SendAsync(new FinishGame(result.status, result.turn, Boss.Hp, Players.Select(x => x.Snapshot()).ToArray()));
        }

        public async Task<(GameStatus status, int turn)> ExecuteBody(CancellableReceiver<GameProgress> channel)
        {
            for (int i = 1; i <= 10; i++)
            {
                System.Diagnostics.Debug.WriteLine($"Excute Turn {i} on thread {Thread.CurrentThread.ManagedThreadId}");
                var stat = await ExecuteTurn(i, channel);
                if (stat != GameStatus.Playing) return (stat, i);
            }
            return (GameStatus.TurnOver, 10);
        }

        private async Task<GameStatus> ExecuteTurn(int turn, CancellableReceiver<GameProgress> channel)
        {
            await channel.SendAsync(new TurnStarted(turn));

            foreach (var actor in GetActors())
            {
                var stat = await (actor != null ? ExecutePlayer(actor, channel): ExecuteBoss(channel));
                if (stat != GameStatus.Playing) return stat;
            }

            return GameStatus.Playing;
        }

        private async Task<GameStatus> ExecuteBoss(CancellableReceiver<GameProgress> channel)
        {
            var v = _random.NextDouble();

            if (v < 0.5)
            {
                // 50% で平均10万ダメージの全体攻撃
                var attacks = _players.Where(p => !p.IsDead).Select(p => BossAttack(p, 90000, 110000)).ToArray();

                await channel.SendAsync(new BossAttack(attacks));
            }
            else
            {
                // 50% で平均20万ダメージの単体攻撃
                var alive = _players.Where(p => !p.IsDead).ToArray();
                var i = _random.Next(alive.Length);
                var target = alive[i];
                var attack = BossAttack(target, 190000, 210000);

                await channel.SendAsync(new BossAttack(new[] { attack }));
            }

            // コンティニュー処理
            if (_players.Any(p => p.IsDead && !p.IsRetired))
            {
                var messages = _players.Where(p => p.IsDead && !p.IsRetired).Select(p => new ContinuePrompt(p)).ToArray();
                await channel.SendAsync(messages);

                var continuedPlayers = new List<PlayerSnapshop>();

                foreach (var m in messages)
                {
                    // とりあえず、コンティニューしなかった人はリタイア扱いにしとく。
                    if (m.Response)
                    {
                        m.Player.Resurrect();
                        continuedPlayers.Add(m.Player.Snapshot());
                    }
                    else m.Player.Retire();
                }

                if(continuedPlayers.Any())
                {
                    await channel.SendAsync(new Continue(continuedPlayers));
                }
            }

            // 全滅判定
            if (_players.All(p => p.IsDead))
                return GameStatus.PlayerDestroyed;

            return GameStatus.Playing;
        }

        private BossAttackItem BossAttack(Player p, int damageMin, int damageMax)
        {
            var damage = (int)_random.NextDouble(damageMin, damageMax);
            p.Hp = Math.Max(p.Hp - damage, 0);
            var attack = new BossAttackItem(p.Snapshot(), damage);
            return attack;
        }

        private async Task<GameStatus> ExecutePlayer(Player player, CancellableReceiver<GameProgress> channel)
        {
            if (player.IsDead) return GameStatus.Playing;

            _area.RedrawAll(player.Index);
            player.InvalidateHand();

            var res = (await channel.SendAsync(new CommandPrompt(player))).GetResponse();

            if (res?.RedrawCardIndexes is int[] indexes)
                _area.Redraw(player.Index, indexes);

            player.InvalidateHand();

            var damage = Judge.Score(player);
            Boss.Hp -= damage;
            if (Boss.Hp < 0) Boss.Hp = 0;

            await channel.SendAsync(new PlayerAttack(player.Snapshot(), player.Hand, damage, Boss.Hp));

            if (Boss.Hp == 0) return GameStatus.BossDefeated;
            else return GameStatus.Playing;
        }

        /// <summary>
        /// ±20程度の乱数を足したうえで、スピード順に並べたアクター (= プレイヤー or ボス)を返す。
        /// </summary>
        /// <returns>スピード順のアクター。null の時がボスという扱い。</returns>
        /// <remarks>
        /// 死んでるやつも返すので、それをはじくのは使う側で。
        /// (今の作りだと、最初に全部作っちゃうのでどうせターンの途中で死ぬ可能性がある。)
        ///
        /// null の時がボスってのも微妙だけど、まあ、手抜き。
        /// ほんとはたぶん、Player | Boss な union クラス作った方がいいと思う。
        ///
        /// 実際のゲームだと、ターンの最中に補助スキル発動で速さ変わったりするんで、
        /// 乱数だけ先に計算して、都度、現在位置版早いやつをyield returnみたいな実装の方がたぶんいい。
        /// 今回は補助スキルないし、そこまでやらない。
        /// </remarks>
        private IEnumerable<Player> GetActors()
            => _players
                .Select(p => (speed: p.Ability.Speed + _random.NextDouble(-20, 20), player: p))
                .Append((speed: 50 + _random.NextDouble(-20, 20), player: default(Player))) // ボスは速さ50固定
                .OrderByDescending(p => p.speed)
                .Select(p => p.player)
                .ToArray();
    }
}
