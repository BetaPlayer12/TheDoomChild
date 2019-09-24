#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    public interface IPhaseConditionHandle<T> where T : System.Enum
    {
        T GetProposedPhase();
    }
}