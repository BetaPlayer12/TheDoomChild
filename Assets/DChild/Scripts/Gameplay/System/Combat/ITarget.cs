using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public struct TargetInfo
    {
        public ITarget target { get; }
        public float damageReduction { get; }

        public TargetInfo(ITarget target, float damageReduction) : this()
        {
            this.target = target;
            this.damageReduction = damageReduction;
        }
    }

    public interface ITarget
    {
        Vector2 position { get; }
        IAttackResistance attackResistance { get; }
        void TakeDamage(int totalDamage, AttackType type);
        void Heal(int health);
        int GetInstanceID();
        bool CompareTag(string tag);
    }
}