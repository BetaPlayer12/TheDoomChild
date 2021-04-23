#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    public interface IBattleZoneAIBrain
    {
        void SwitchToBattleZoneAI();

        void SwitchToBaseAI();
    }
}