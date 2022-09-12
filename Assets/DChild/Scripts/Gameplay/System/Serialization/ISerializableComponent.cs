namespace DChild.Serialization
{
    public interface ISerializableComponent
    {
        ISaveData Save();
        void Load(ISaveData data);

        void Initialize();
    }
}