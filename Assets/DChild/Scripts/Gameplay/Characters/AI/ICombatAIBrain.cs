using DChild.Gameplay;
using DChild.Gameplay.Combat;

namespace DChild.Gameplay.Characters.AI
{
    public interface ICombatAIBrain
    {
        void SetTarget(IDamageable damageable, Character m_target = null);
    }
}