namespace CardBattle.Models.Messages
{
    /// <summary>
    /// ターン開始の通知。
    /// </summary>
    public class TurnStarted : GameProgress
    {
        /// <summary>
        /// ターン数。
        /// 1スタート。
        /// </summary>
        public int Turn { get; }

        internal TurnStarted(int turn)
        {
            Turn = turn;
        }
    }
}
