using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using Doozy.Engine;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using Refactor.DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

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
        IMainController controller { get; }
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
        private PlayerModifierHandle m_modifiers;
        [SerializeField]
        private PlayerCharacterController m_controller;

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

        public IMainController controller => m_controller;

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
            m_controller.Disable();
            m_damageable.SetHitboxActive(false);
        }
    }
}