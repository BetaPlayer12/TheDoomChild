#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    public interface IAIAnimationInfo
    {
        string animation { get; }
       float animationTimeScale { get; }
    }
}