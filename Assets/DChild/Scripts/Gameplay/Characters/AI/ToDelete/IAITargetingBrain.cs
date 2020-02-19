using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;

namespace DChild.Gameplay.Characters.AI
{
    public interface IAITargetingBrain
    {
        void SetTarget(IEnemyTarget target);
    }
}