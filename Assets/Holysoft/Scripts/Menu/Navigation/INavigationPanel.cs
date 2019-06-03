namespace Holysoft.Menu
{
    public interface INavigationPanel
    {
        void Open();
        void Close();
        void ForceClose();
        void ForceOpen();

#if UNITY_EDITOR
        string name { get; }
#endif
    }
}
