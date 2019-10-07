using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat.StatusAilment;
using DChild.Gameplay.Environment;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public struct TargetInfo
    {
        public IDamageable instance { get; }
        public bool isCharacter { get; }
        public bool hasID { get; }
        public int characterID { get; }
        public HorizontalDirection facing { get; }
        public IFlinch flinchHandler { get; }
        public float damageReduction { get; }
        public bool isPlayer { get; }
        public IPlayer owner { get; }
        public StatusEffectReciever statusEffectReciever { get; }
        public bool isBreakableObject { get; }
        public BreakableObject breakableObject;


        public TargetInfo(IDamageable target, Character character = null, IFlinch flinchHandler = null) : this()
        {
            this.instance = target;
            isCharacter = character;
            if (isCharacter)
            {
                facing = character.facing;
                statusEffectReciever = character.GetComponent<StatusEffectReciever>();
                isPlayer = character.gameObject.layer == LayerMask.NameToLayer("Player");
                if (isPlayer)
                {
                    owner = character.GetComponent<PlayerControlledObject>().owner;
                }
                hasID = character.hasID;
                characterID = character.ID;
            }
            this.flinchHandler = flinchHandler;
        }

        public TargetInfo(IDamageable target, BreakableObject breakableObject = null) : this()
        {
            this.instance = target;
            isBreakableObject = breakableObject;
            if (isBreakableObject)
            {
                this.breakableObject = breakableObject;
            }
        }

        public TargetInfo(IDamageable target, float damageReduction) : this()
        {
            this.instance = target;
            this.damageReduction = damageReduction;
        }
    }
}