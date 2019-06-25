using DChild.Gameplay.Characters;
using Refactor.DChild.Gameplay;
using Refactor.DChild.Gameplay.Characters;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public struct TargetInfo
    {
        public ITarget target { get; }
        public bool isCharacter { get; }
        public HorizontalDirection facing { get; }
        public IFlinch flinchHandler { get; }
        public float damageReduction { get; }

        public TargetInfo(ITarget target, Character character = null, IFlinch flinchHandler = null) : this()
        {
            this.target = target;
            isCharacter = character;
            if (isCharacter)
            {
                facing = character.facing;
            }
            this.flinchHandler = flinchHandler;
        }

        public TargetInfo(ITarget target, float damageReduction) : this()
        {
            this.target = target;
            this.damageReduction = damageReduction;
        }
    }

    public interface ITarget
    {
        bool isAlive { get; }
        Vector2 position { get; }
        IAttackResistance attackResistance { get; }
        void TakeDamage(int totalDamage, AttackType type);
        void Heal(int health);
        int GetInstanceID();
        bool CompareTag(string tag);
    }
}