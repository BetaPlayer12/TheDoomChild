using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat.StatusAilment;
using Refactor.DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public struct TargetInfo
    {
        public ITarget instance { get; }
        public bool isCharacter { get; }
        public bool hasID { get; }
        public int characterID { get; }
        public HorizontalDirection facing { get; }
        public IFlinch flinchHandler { get; }
        public float damageReduction { get; }
        public bool isPlayer { get; }
        public IPlayer owner { get; }
        public StatusEffectReciever statusEffectReciever { get; }

        public TargetInfo(ITarget target, Character character = null, IFlinch flinchHandler = null) : this()
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

        public TargetInfo(ITarget target, float damageReduction) : this()
        {
            this.instance = target;
            this.damageReduction = damageReduction;
        }
    }
}