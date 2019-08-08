#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public interface Challenge
    {
        string message { get; }
        bool IsComplete();
    }
}