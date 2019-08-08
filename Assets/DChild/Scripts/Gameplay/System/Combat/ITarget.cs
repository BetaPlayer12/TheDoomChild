using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using Refactor.DChild.Gameplay;
using Refactor.DChild.Gameplay.Characters;
using Refactor.DChild.Gameplay.Characters.Players;
using Refactor.DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public struct TargetInfo
    {
        public ITarget instance { get; }
        public bool isCharacter { get; }
        public HorizontalDirection facing { get; }
        public IFlinch flinchHandler { get; }
        public float damageReduction { get; }
        public bool isPlayer { get; }
        public IPlayer owner { get; }

        public TargetInfo(ITarget target, Character character = null, IFlinch flinchHandler = null) : this()
        {
            this.instance = target;
            isCharacter = character;
            if (isCharacter)
            {
                facing = character.facing;
                isPlayer = character.gameObject.layer == LayerMask.NameToLayer("Player");
                if (isPlayer)
                {
                    owner = character.GetComponent<PlayerControlledObject>().owner;
                }
            }

            this.flinchHandler = flinchHandler;
        }

        public TargetInfo(ITarget target, float damageReduction) : this()
        {
            this.instance = target;
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