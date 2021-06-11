using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat.StatusAilment;
using DChild.Gameplay.Environment;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class TargetInfo
    {
        public IDamageable instance { get; private set; }
        public bool isCharacter { get; private set; }
        public bool hasID { get; private set; }
        public int characterID { get; private set; }
        public HorizontalDirection facing { get; private set; }
        public IFlinch flinchHandler { get; private set; }
        public float damageReduction { get; private set; }
        public bool isPlayer { get; private set; }
        public IPlayer owner { get; private set; }
        public StatusEffectReciever statusEffectReciever { get; private set; }
        public bool isBreakableObject { get; private set; }
        public bool canBlockDamage { get; private set; }
        public BreakableObject breakableObject { get; private set; }


        public void Initialize(IDamageable target, bool canBlockDamage,Character character = null, IFlinch flinchHandler = null)
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
            this.canBlockDamage = canBlockDamage;
            isBreakableObject = false;
            breakableObject = null;
        }

        public void Initialize(IDamageable target, bool canBlockDamage, BreakableObject breakableObject = null)
        {
            this.instance = target;
            isBreakableObject = breakableObject;
            if (isBreakableObject)
            {
                this.breakableObject = breakableObject;
            }

            isCharacter = false;
            isPlayer = false;
            statusEffectReciever = null;
            owner = null;
            hasID = false;
            flinchHandler = null;
            this.canBlockDamage = canBlockDamage;
        }

        public void Initialize(IDamageable target, bool canBlockDamage, float damageReduction)
        {
            this.instance = target;
            this.damageReduction = damageReduction;

            isCharacter = false;
            isPlayer = false;
            statusEffectReciever = null;
            owner = null;
            hasID = false;
            flinchHandler = null;
            this.canBlockDamage = canBlockDamage;
            isBreakableObject = false;
            breakableObject = null;
        }
    }
}