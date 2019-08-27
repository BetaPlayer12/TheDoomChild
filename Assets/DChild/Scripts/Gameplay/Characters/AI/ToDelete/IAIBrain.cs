namespace DChild.Gameplay.Characters.AI
{
    public interface IAIBrain
    {
        bool enabled {set;}
        void Enable(bool value);
        void ResetBrain();
    }
}