using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat.StatusAilment;
using DChild.Gameplay.Environment;
using DChild.Gameplay.VFX;
using DChild.Menu.Bestiary;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class TargetInfo
    {
        public IDamageable instance { get; private set; }
        public BodyDefense bodyDefense { get; private set; }
        public FXSpawnConfigurationInfo damageFXInfo { get; private set; }
        public bool canBlockDamage { get; private set; }
        public Collider2D hitCollider { get; private set; }


        public bool isCharacter { get; private set; }
        public bool hasBestiaryData { get; private set; }
        public int bestiaryID { get; private set; }
        public HorizontalDirection facing { get; private set; }
        public IFlinch flinchHandler { get; private set; }
        public bool isPlayer { get; private set; }

        private IPlayer m_owner;
        public IPlayer owner => m_owner;
        public StatusEffectReciever statusEffectReciever { get; private set; }
        public bool isBreakableObject { get; private set; }
        public BreakableObject breakableObject { get; private set; }

        public void Initialize(IDamageable target, bool canBlockDamage, BodyDefense bodyDefense, Collider2D hitCollider, Character character = null, IFlinch flinchHandler = null)
        {
            InitializeEssentials(target, canBlockDamage, bodyDefense, hitCollider);
            isCharacter = character;
            if (isCharacter)
            {
                facing = character.facing;
                statusEffectReciever = character.GetComponent<StatusEffectReciever>();
                isPlayer = GameplaySystem.playerManager.IsPartOfPlayer(character.gameObject, out m_owner);
            }

            var bestiaryEntity = character.GetComponent<BestiaryEntity>();
            hasBestiaryData = bestiaryEntity;
            if (hasBestiaryData)
            {
                bestiaryID = bestiaryEntity.bestiaryID;
            }


            this.flinchHandler = flinchHandler;
            isBreakableObject = false;
            breakableObject = null;
        }

        public void Initialize(IDamageable target, bool canBlockDamage, BodyDefense bodyDefense, Collider2D hitCollider, BreakableObject breakableObject = null)
        {
            InitializeEssentials(target, canBlockDamage, bodyDefense, hitCollider);
            isBreakableObject = breakableObject;
            if (isBreakableObject)
            {
                this.breakableObject = breakableObject;
            }

            var bestiaryEntity = breakableObject.GetComponent<BestiaryEntity>();
            hasBestiaryData = bestiaryEntity;
            if (hasBestiaryData)
            {
                bestiaryID = bestiaryEntity.bestiaryID;
            }

            isCharacter = false;
            isPlayer = false;
            statusEffectReciever = null;
            m_owner = null;
            hasBestiaryData = false;
            flinchHandler = null;
        }

        public void Initialize(Hitbox hitbox, Collider2D hitCollider, Character character = null, IFlinch flinchHandler = null)
        {
            InitializeEssentials(hitbox, hitCollider);
            isCharacter = character;
            if (isCharacter)
            {
                facing = character.facing;
                statusEffectReciever = character.GetComponent<StatusEffectReciever>();
                isPlayer = GameplaySystem.playerManager.IsPartOfPlayer(character.gameObject, out m_owner);
                hasBestiaryData = character.hasID;
            }

            var bestiaryEntity = character.GetComponent<BestiaryEntity>();
            hasBestiaryData = bestiaryEntity;
            if (hasBestiaryData)
            {
                bestiaryID = bestiaryEntity.bestiaryID;
            }

            this.flinchHandler = flinchHandler;
            isBreakableObject = false;
            breakableObject = null;
        }

        public void Initialize(Hitbox hitbox, Collider2D hitCollider, BreakableObject breakableObject = null)
        {
            InitializeEssentials(hitbox, hitCollider);
            isBreakableObject = breakableObject;
            if (isBreakableObject)
            {
                this.breakableObject = breakableObject;
            }

            var bestiaryEntity = breakableObject?.GetComponent<BestiaryEntity>() ?? null;
            hasBestiaryData = bestiaryEntity;
            if (hasBestiaryData)
            {
                bestiaryID = bestiaryEntity.bestiaryID;
            }

            isCharacter = false;
            isPlayer = false;
            statusEffectReciever = null;
            m_owner = null;
            hasBestiaryData = false;
            flinchHandler = null;

        }

        private void InitializeEssentials(Hitbox hitbox, Collider2D hitCollider)
        {
            InitializeEssentials(hitbox.damageable, hitbox.canBlockDamage, hitbox.defense, hitCollider);
            damageFXInfo = hitbox.damageFXInfo;
        }

        private void InitializeEssentials(IDamageable target, bool canBlockDamage, BodyDefense bodyDefense, Collider2D hitCollider)
        {
            this.instance = target;
            this.canBlockDamage = canBlockDamage;
            this.bodyDefense = bodyDefense;
            this.hitCollider = hitCollider;
        }
    }
}