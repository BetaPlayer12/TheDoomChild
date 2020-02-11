namespace DChild.Gameplay.Trohpies
{
    public interface ISerializableTrophyModule
    {
        TrophyModuleID trophyModuleID { get; }
        ITrophyProgressData SaveData();
        void LoadData(ITrophyProgressData data);
    }
}