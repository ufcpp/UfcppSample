namespace ValueTuples.Serialization
{
    public interface IDeserializer
    {
        void Deserialize(IRecord record);
    }
}
