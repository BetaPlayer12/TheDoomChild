namespace DChild.UI
{
    public interface IReferenceUI<T> where T: class
    {
        void SetReference(T reference);
    }
}