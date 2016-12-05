namespace CardBattle.Models.Messages
{
    /// <summary>
    /// はい か いいえ で応えれる応答。
    /// 実質、bool のラッパー クラス。
    /// </summary>
    public class Confirmation : GameResponse
    {
        /// <summary>
        /// 値。
        /// </summary>
        public bool Value { get; }

        private Confirmation(bool value) { Value = value; }

        /// <summary>
        /// 「はい」。true ラッパー。
        /// </summary>
        public static Confirmation Yes { get; } = new Confirmation(true);

        /// <summary>
        /// 「いいえ」。false ラッパー。
        /// </summary>
        public static Confirmation No { get; } = new Confirmation(false);

        public static explicit operator Confirmation(bool value) => value ? Yes : No;
        public static explicit operator Confirmation(bool? value)
            => value == null ? null :
            value.Value ? Yes :
            No;
        public static bool operator true(Confirmation c) => c != null && c.Value;
        public static bool operator false(Confirmation c) => c == null || !c.Value;
    }
}
