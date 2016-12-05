using CardBattle.Lib.ComponentModel;
using System.ComponentModel;
using System.Linq;

namespace CardBattle.Models.GameRules
{
    /// <summary>
    /// 各プレイヤーの状態。
    /// </summary>
    public class Player : BindableBase
    {
        /// <summary>
        /// <see cref="PlayArea"/>での管理用のインデックス。
        /// </summary>
        internal int Index { get; }

        /// <summary>
        /// プレイヤーのID。
        /// </summary>
        public byte Id { get; }

        /// <summary>
        /// 現在の体力。
        /// コンストラクターで<see cref="Ability.MaxHp"/>に初期化。
        /// </summary>
        public int Hp { get { return _hp; } internal set { SetProperty(ref _hp, value, HpProperty); } }
        public static readonly PropertyChangedEventArgs HpProperty = new PropertyChangedEventArgs(nameof(Hp));
        private int _hp = 1000000;

        /// <summary>
        /// 能力値。
        /// </summary>
        public Ability Ability { get; }

        /// <summary>
        /// 手札。
        /// </summary>
        /// <remarks>
        /// <see cref="PlayArea"/>から、<see cref="PlayArea"/>内で管理してるインスタンスを渡してもらう。
        /// </remarks>
        public IHand Hand { get; }

        /// <summary>
        /// View へのバインド用。
        /// <see cref="Cards"/>のクローン。
        /// </summary>
        /// <remarks>
        /// GC 除けしてたらインスタンスが切り替わらず、
        /// インスタンス切り替わらないと View の更新がうまくいかなかったり。
        /// </remarks>
        public Card[] Cards => Hand.Cards.ToArray();
        public static readonly PropertyChangedEventArgs CloneCardsProperty = new PropertyChangedEventArgs(nameof(Cards));

        internal void InvalidateHand() => OnPropertyChanged(CloneCardsProperty);

        internal Player(int index, byte id, Ability ability, IHand hand)
        {
            Index = index;
            Id = id;
            Ability = ability;
            Hand = hand;
            Hp = ability.MaxHp;
        }

        /// <summary>
        /// 死亡状態か判定。
        /// </summary>
        public bool IsDead => Hp <= 0;

        /// <summary>
        /// リタイア状況。
        /// true ならもうコンティニューもしない状態。
        /// </summary>
        public bool IsRetired { get; private set; }

        /// <summary>
        /// 復活。
        /// とりあえずHP全快で復活させる。
        /// </summary>
        internal void Resurrect()
        {
            Hp = Ability.MaxHp;
        }

        /// <summary>
        /// リタイア。
        /// </summary>
        internal void Retire()
        {
            Hp = 0;
            IsRetired = true;
        }
    }
}
