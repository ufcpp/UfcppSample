using TaskLibrary.Channels;
using CardBattle.Models.GameRules;

namespace CardBattle.Models.Messages
{
    /// <summary>
    /// 死亡時に、コンティニューするかどうかを促すメッセージ。
    /// </summary>
    public class ContinuePrompt : GameProgress, IResponsiveMessage<Confirmation>
    {
        internal Player Player { get; }

        internal ContinuePrompt(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// 誰が死んだか。
        /// </summary>
        public byte PlayerId => Player.Id;

        int IResponsiveMessage.Address => PlayerId;

        public Confirmation Response { get { return _response; } set { _response = value; } }
        object IResponsiveMessage.Response { get { return _response; } set { _response = (Confirmation)value; } }
        private Confirmation _response;

        /// <summary>
        /// コンティニューする/しないを決定。
        /// </summary>
        /// <param name="value">コンティニューするときには true を渡す。</param>
        /// <remarks>
        /// 特に何もしなかったら Response が null のまま、かつ、null だったら false 扱いなので、
        /// コンティニューするときだけこいつを呼べばいいんだけど。
        /// 「すると思ったけどやっぱりやめた」があり得るかもしれないんで、取りやめのために <paramref name="value"/> を用意。
        /// </remarks>
        public void Confirm(bool value = true)
        {
            _response = (Confirmation)value;
        }
    }
}
