namespace TaskLibrary.Channels
{
    /// <summary>
    /// 再現実行の際に、再現ずれがないかわかるように、応答のあるメッセージにはIDを振る。
    /// メッセージが何番目で、誰宛てかで一意に弁別できるはず。
    /// </summary>
    public struct ResponsiveMessageId
    {
        private long _value;

        /// <summary>
        /// 何番目のメッセージか。
        /// </summary>
        public int SequenceNumber => unchecked((int)(_value >> 32));

        /// <summary>
        /// 誰宛てか。
        /// </summary>
        public int Address => unchecked((int)_value);

        /// <summary>
        /// </summary>
        /// <param name="sequenceNumber"><see cref="SequenceNumber"/></param>
        /// <param name="address"><see cref="Address"/></param>
        public ResponsiveMessageId(int sequenceNumber, int address)
        {
            _value = ((long)sequenceNumber << 32) | (long)address;
        }
    }

    /// <summary>
    /// 応答を記録しておくための型。
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public struct RecordedResponse<TResponse>
    {
        /// <summary>
        /// メッセージのID。
        /// </summary>
        public ResponsiveMessageId Id { get; }

        /// <summary>
        /// 応答内容。
        /// </summary>
        public TResponse Response { get; }

        /// <summary></summary>
        /// <param name="id"><see cref="Id"/></param>
        /// <param name="response"><see cref="Response"/></param>
        public RecordedResponse(ResponsiveMessageId id, TResponse response)
        {
            Id = id;
            Response = response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequenceNumber"><see cref="ResponsiveMessageId.SequenceNumber"/></param>
        /// <param name="address"><see cref="ResponsiveMessageId.Address"/></param>
        /// <param name="response"><see cref="Response"/></param>
        public RecordedResponse(int sequenceNumber, int address, TResponse response) : this(new ResponsiveMessageId(sequenceNumber, address), response) { }
    }

}
