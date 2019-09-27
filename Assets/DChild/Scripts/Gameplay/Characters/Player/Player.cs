using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using DChild.Gameplay.Inventories;
using DChild.Serialization;
using Doozy.Engine;
using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;
using DChild.Gameplay.Characters.Players.Behaviour;

namespace DChild.Gameplay.Characters.Players
{
    public interface IPlayer
    {
        event EventAction<EventActionArgs> OnDeath;
        CharacterState state { get; }
        IPlayerStats stats { get; }
        Health health { get; }
        Magic magic { get; }
        IHealable healableModule { get; }
        IDamageable damageableModule { get; }
        IAttacker attackModule { get; }
        PlayerModifierHandle modifiers { get; }
        PlayerWeapon weapon { get; }
        ExtendedAttackResistance attackResistance { get; }
        StatusEffectResistance statusResistance { get; }
        IMainController controller { get; }
        PlayerInventory inventory { get; }
        LootPicker lootPicker { get; }
    }

    public class Player : SerializedMonoBehaviour, IPlayer
    {
        [SerializeField]
        private IPlayerStats m_stats;
        [SerializeField]
        private PlayerWeapon m_weapon;
        [SerializeField]
        private ExtendedAttackResistance m_attackResistance;
        [SerializeField]
        private StatusEffectResistance m_statusResistance;
        [SerializeField]
        private PlayerModifierHandle m_modifiers;
        [SerializeField]
        private PlayerCharacterController m_controller;
        [SerializeField]
        private PlayerInventory m_inventory;

        [Title("Serialzables")]
        [SerializeField]
        private PlayerCharacterSerializer m_serializer;

        [Title("Model")]
        [SerializeField]
        private Character m_controlledCharacter;
        [SerializeField]
        private CharacterState m_state;
        [SerializeField]
        private Damageable m_damageable;
        [SerializeField]
        private Attacker m_attacker;
        [SerializeField]
        private Magic m_magic;
        [SerializeField]
        private LootPicker m_lootPicker;
        [SerializeField]
        private GroundednessHandle m_groundednessHandle;

        public event EventAction<EventActionArgs> OnDeath;

        public IPlayerStats stats => m_stats;

        public CharacterState state => m_state;
        public Health health => m_damageable.health;
        public Magic magic => m_magic;
        public IHealable healableModule => m_damageable;
        public IDamageable damageableModule => m_damageable;
        public IAttacker attackModule => m_attacker;
        public PlayerModifierHandle modifiers => m_modifiers;
        public PlayerWeapon weapon => m_weapon;
        public ExtendedAttackResistance attackResistance => m_attackResistance;
        public PlayerInventory inventory => m_inventory;
        public IMainController controller => m_controller;
        public LootPicker lootPicker => m_lootPicker;

        public StatusEffectResistance statusResistance => m_statusResistance;

        public PlayerCharacterData SaveData()
        {
            return m_serializer.SaveData();
        }

        public void LoadData(PlayerCharacterData data)
        {
            m_serializer.LoadData(data);
        }

        private void Awake()
        {
            var controlledObject = m_controlledCharacter.gameObject.AddComponent<PlayerControlledObject>();
            controlledObject.SetOwner(this);
            m_damageable.Destroyed += OnDestroyed;
        }

        private void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            OnDeath?.Invoke(this, eventArgs);
            GameEventMessage.SendEvent("Game Over");
            m_controlledCharacter.physics.SetVelocity(Vector2.zero);
            m_groundednessHandle.enabled = false;
            m_groundednessHandle.ResetAnimationParameters();
            m_controller.Disable();
            m_damageable.SetHitboxActive(false);
        }
    }
}