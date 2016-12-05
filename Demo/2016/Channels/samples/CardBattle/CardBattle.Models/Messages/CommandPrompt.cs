using TaskLibrary.Channels;
using CardBattle.Models.GameRules;
using System;
using System.Linq;

namespace CardBattle.Models.Messages
{
    /// <summary>
    /// コマンド入力を促すメッセージ。
    /// </summary>
    /// <remarks>
    /// 今回の想定では、コマンド入力は逐次手番。
    /// 1人ずつ手番が来て1人ずつ選択コマンド選択。
    ///
    /// コマンドというか、ポーカーのカード交換みたいなシステムの予定。
    /// </remarks>
    public class CommandPrompt : GameProgress, IResponsiveMessage<CommandResonse>
    {
        /// <summary>
        /// 誰の手番か。
        /// </summary>
        public byte PlayerId { get; }

        /// <summary>
        /// 現在の手札。
        /// </summary>
        public IHand Hand { get; }

        /// <summary>
        /// View へのバインド用。
        /// <see cref="Hand"/>のクローン。
        /// </summary>
        public Card[] Cards => Hand.Cards.ToArray();

        int IResponsiveMessage.Address => PlayerId;

        CommandResonse IResponsiveMessage<CommandResonse>.Response { get { return _response; } set { _response = value; } }
        object IResponsiveMessage.Response { get { return _response; } set { _response = (CommandResonse)value; } }
        private CommandResonse _response;

        internal CommandPrompt(byte playerId, IHand hand)
        {
            PlayerId = playerId;
            Hand = hand;
        }

        public CommandPrompt(Player player) : this(player.Id, player.Hand) { }

        /// <summary>
        /// コマンド入力。
        /// 交換したいカードのインデックスを指定する。
        /// </summary>
        public void Redraw(params int[] indexes)
        {
            if (indexes.Any(i => i < 0 || i >= 5)) throw new IndexOutOfRangeException(nameof(indexes));

            _response = new CommandResonse(indexes);
        }
    }
}
