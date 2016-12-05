using CardBattle.Lib.ComponentModel;
using System.ComponentModel;

namespace CardBattle.Models.GameRules
{
    /// <summary>
    /// ボス(敵)。
    /// </summary>
    /// <remarks>
    /// ゲーム的に、全プレイヤーで協力して1対のボスを殴るシステムにしてみる。
    ///
    /// 現実にはボス側もプレイヤー側と同じ仕組みで動かすとか、ボス用の能力値クラス作るとかが必要だけど、
    /// めんどくさいんで全部固定。
    ///
    /// しかも、ボスは1体限りという手抜きルール。
    /// </remarks>
    public class Boss : BindableBase
    {
        /// <summary>
        /// 現在の体力。
        /// </summary>
        /// <remarks>
        /// これも固定で100万スタート。
        /// </remarks>
        public int Hp { get { return _hp; } internal set { SetProperty(ref _hp, value, HpProperty); } }
        public static readonly PropertyChangedEventArgs HpProperty = new PropertyChangedEventArgs(nameof(Hp));
        private int _hp = 1000000;
    }
}
