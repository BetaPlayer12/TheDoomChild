using System;
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
using DChild.Gameplay.SoulSkills;

namespace DChild.Gameplay.Characters.Players
{
    public interface IPlayer
    {
        event EventAction<EventActionArgs> OnDeath;
        Modules.CharacterState state { get; }
        IPlayerStats stats { get; }
        Health health { get; }
        Magic magic { get; }
        Health armor { get; }
        IHealable healableModule { get; }
        IDamageable damageableModule { get; }
        IAttacker attackModule { get; }
        PlayerModuleActivator behaviourModule { get; }
        PlayerSkills skills { get; }
        PlayerSoulSkillHandle soulSkills { get; }
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
        private PlayerModuleActivator m_behaviourModule;
        [SerializeField]
        private PlayerSkills m_skills;
        [SerializeField]
        private PlayerSoulSkillHandle m_soulSkills;
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
        private Modules.CharacterState m_state;
        [SerializeField]
        private Damageable m_damageable;
        [SerializeField]
        private Attacker m_attacker;
        [SerializeField]
        private Magic m_magic;
        [SerializeField]
        private Health m_armor;
        [SerializeField]
        private StatusEffectReciever m_statusEffectReciever;
        [SerializeField]
        private LootPicker m_lootPicker;

        public event EventAction<EventActionArgs> OnDeath;

        public IPlayerStats stats => m_stats;

        public Modules.CharacterState state => m_state;
        public Health health => m_damageable.health;
        public Magic magic => m_magic;
        public Health armor => m_armor;
        public IHealable healableModule => m_damageable;
        public IDamageable damageableModule => m_damageable;
        public IAttacker attackModule => m_attacker;
        public PlayerModifierHandle modifiers => m_modifiers;
        public PlayerModuleActivator behaviourModule => m_behaviourModule;
        public PlayerSkills skills => m_skills;
        public PlayerSoulSkillHandle soulSkills => m_soulSkills;
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
        }

        public void SetPosition(Vector2 position)
        {
            m_controlledCharacter.transform.position = position;
        }

        public void Initialize()
        {
            m_weapon.Initialize();
            m_attackResistance.Initialize();
            m_statusResistance.Initialize();
            m_modifiers.Initialize();
            m_soulSkills.Initialize();
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
            m_state = character.GetComponentInChildren<Modules.CharacterState>();
            m_damageable = character.GetComponentInChildren<Damageable>();
            m_attacker = character.GetComponentInChildren<Attacker>();
            m_magic = character.GetComponentInChildren<Magic>();
            m_lootPicker = character.GetComponentInChildren<LootPicker>();
        }
#endif
    }
}