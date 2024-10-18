namespace Holysoft.Collections
{
    public interface ISerializable<T>
    {
        T SaveData();
        void LoadData(T data);
    }
}
