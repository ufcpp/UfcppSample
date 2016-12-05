namespace CardBattle.Models
{
    public enum GameStatus
    {
        /// <summary>
        /// ゲーム進行中。
        /// </summary>
        Playing,

        /// <summary>
        /// プレイヤー側全滅。
        /// </summary>
        PlayerDestroyed,

        /// <summary>
        /// ボス撃破。
        /// </summary>
        BossDefeated,

        /// <summary>
        /// 規定ターンを超えたので終了。
        /// </summary>
        TurnOver,
    }
}
