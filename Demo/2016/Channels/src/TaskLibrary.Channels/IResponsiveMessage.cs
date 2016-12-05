using System.Collections.Generic;
using System.Linq;

namespace TaskLibrary.Channels
{
    /// <summary>
    /// View 側から応答を返してもらう必要のあるメッセージにつけるインターフェイス。
    /// この <see cref="Response"/> に値を入れておくと、再現データとして記録される。
    /// </summary>
    public interface IResponsiveMessage
    {
        /// <summary>
        /// 誰宛てか、誰が応答を返さないといけないかを判別するためのID。
        /// </summary>
        int Address { get; }

        /// <summary>
        /// View 側からの応答。
        /// </summary>
        object Response { get; set; }
    }

    /// <summary>
    /// 応答の型指定版、View 側からの応答があるメッセージのインターフェイス。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResponsiveMessage<T> : IResponsiveMessage
    {
        /// <summary>
        /// View 側からの応答。
        /// </summary>
        new T Response { get; set; }
    }

    /// <summary>
    /// <see cref="IResponsiveMessage{T}"/>向け拡張。
    /// </summary>
    public static class ResponsiveMessageExtensions
    {
        /// <summary>
        /// <see cref="IResponsiveMessage{T}"/>は明示的実装を推奨してるので、キャストをさぼるための拡張メソッドを1個用意。
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public static TResponse GetResponse<TResponse>(this IResponsiveMessage<TResponse> message) => message.Response;

        /// <summary>
        /// <see cref="GetResponse{TResponse}(IResponsiveMessage{TResponse})"/>を、並列メッセージに対してやるための拡張メソッド。
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static IEnumerable<TResponse> GetResponse<TResponse>(this IEnumerable<IResponsiveMessage<TResponse>> messages) => messages.Select(m => m.Response);
    }
}
