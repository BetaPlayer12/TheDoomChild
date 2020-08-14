﻿using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using DChild.Gameplay.Inventories;
using DChild.Serialization;
//using Doozy.Engine;
using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;
using DChild.Gameplay.Characters.Players.Behaviour;
using PlayerNew;

namespace DChild.Gameplay.Characters.Players
{
    public interface IPlayer
    {
        event EventAction<EventActionArgs> OnDeath;
        State.CharacterState state { get; }
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
        StatusEffectReciever statusEffectReciever { get; }
        Character character { get; }

        int GetInstanceID();
    }

    [AddComponentMenu("DChild/Gameplay/Player/Player")]
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField]
        private PlayerStats m_stats;
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
        [SerializeField]
        private SoulCrystalHandle m_soulCrystalHandle;

        [Title("Serialzables")]
        [SerializeField]
        private PlayerCharacterSerializer m_serializer;

        [Title("Model")]
        [SerializeField]
        private Character m_controlledCharacter;
        [SerializeField]
        private State.CharacterState m_state;
        [SerializeField]
        private Damageable m_damageable;
        [SerializeField]
        private Attacker m_attacker;
        [SerializeField]
        private Magic m_magic;
        [SerializeField]
        private StatusEffectReciever m_statusEffectReciever;
        [SerializeField]
        private LootPicker m_lootPicker;

        public event EventAction<EventActionArgs> OnDeath;

        public IPlayerStats stats => m_stats;

        public State.CharacterState state => m_state;
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

        public StatusEffectReciever statusEffectReciever => m_statusEffectReciever;

        public StatusEffectResistance statusResistance => m_statusResistance;

        public Character character => m_controlledCharacter;

        public PlayerCharacterData SaveData()
        {
            return m_serializer.SaveData();
        }

        public void LoadData(PlayerCharacterData data)
        {
            m_serializer.LoadData(data);
            m_soulCrystalHandle?.InitializeHandles();
        }

        public void SetPosition(Vector2 position)
        {
            m_controlledCharacter.transform.position = position;
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
            //  m_controlledCharacter.physics.SetVelocity(Vector2.zero);

            m_controller.Disable();
            m_damageable.SetHitboxActive(false);
        }

#if UNITY_EDITOR
        public void Initialize(GameObject character)
        {
            m_controlledCharacter = character.GetComponentInChildren<Character>();
            m_state = character.GetComponentInChildren<State.CharacterState>();
            m_damageable = character.GetComponentInChildren<Damageable>();
            m_attacker = character.GetComponentInChildren<Attacker>();
            m_magic = character.GetComponentInChildren<Magic>();
            m_lootPicker = character.GetComponentInChildren<LootPicker>();
        }
#endif
    }
}