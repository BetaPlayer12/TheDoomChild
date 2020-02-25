#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    public interface IBossPhaseInfo
    {
        int[] GetHealthPrecentagePhaseInfo();
    }
}