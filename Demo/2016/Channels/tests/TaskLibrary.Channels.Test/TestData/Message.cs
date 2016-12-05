namespace TaskLibrary.Channels.Test.TestData
{
    class Message
    {
    }

    class MessageA : Message
    {
        public string Message { get; }

        public MessageA(string message)
        {
            Message = message;
        }
    }

    class MessageB : Message, IResponsiveMessage<ResponseB>
    {
        public int Address { get; }
        public int Value { get; }

        public ResponseB Response { get; set; }
        object IResponsiveMessage.Response { get { return Response; } set { Response = (ResponseB)value; } }

        public MessageB(byte address, int value)
        {
            Address = address;
            Value = value;
        }

        public void SetResult(int value)
        {
            Response = new ResponseB(value);
        }
    }

    class Response
    {
    }

    class ResponseB : Response
    {
        public int Value { get; }

        public ResponseB(int value)
        {
            Value = value;
        }
    }
}
