using System;
using System.Collections;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusInfliction;
using DChild.Gameplay.Systems.WorldComponents;
using DChild.Serialization;
using Holysoft;
using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public interface IPlayer : IHealable
    {
        ICappedStat health { get; }
        ICappedStat magic { get; }
        IPlayerStats stats { get; }
        IEquipment equipment { get; }
        IAttributes attributes { get; }
        PlayerModifiers modifiers { get; }
        SoulSkillManager soulSkillManager { get; }
        PlayerCharacterState characterState { get; }
        ExtendedAttackResistance attackResistance { get; }
        IStatusResistance statusResistance { get; }
        event EventAction<EventActionArgs> OnDeath;
        event EventAction<CombatConclusionEventArgs> TargetDamaged;
        event EventAction<EventActionArgs> Attacks;
        event EventAction<EventActionArgs> Damaged;
        event EventAction<CombatStateEventArgs> CombatModeChanged;
        void StopCoroutine(Coroutine coroutine);
        Coroutine StartCoroutine(IEnumerator coroutine);
    }

    public interface IPlayerModules : IFacingConfigurator, IFlinchEvents
    {
        ProjectileThrowHandler projectileThrowHandler { get; }
        LootPicker lootPicker { get; }
        PlayerSensors sensors { get; }
        CharacterPhysics2D physics { get; }
        PlayerAnimation animation { get; }
        PlayerCombatAnimation combatAnimation { get; }
        IsolatedObject isolatedObject { get; }
        CharacterColliders colliders { get; }
        IStatusEffectState statusEffectState { get; }
        IAttackResistance attackResistance { get; }
        ICappedStat magic { get; }
        PlayerModifiers modifiers { get; }
        PlayerCharacterState characterState { get; }
        PlayerAnimationState animationState { get; }
        Skills skills { get; }
    }

    public class Player : CombatCharacter, IPlayer, IPlayerModules, IEnemyTarget, IKnockable, IPlayerCombat,
                                           IFlinch, IDamageDealer, IDynamicMagicReference
    {
        #region Stat
        [SerializeField, TabGroup("Combat"), InlineEditor(InlineEditorModes.GUIOnly, Expanded = true), Indent]
        private BasicHealth m_health;
        [SerializeField, TabGroup("Combat"), InlineEditor(InlineEditorModes.GUIOnly, Expanded = true), Indent]
        private Magic m_magic;
        [SerializeField, TabGroup("Combat"), InlineEditor(InlineEditorModes.GUIOnly, Expanded = true), Indent]
        private ExtendedAttackResistance m_extendedAttackResistance;
        [SerializeField, TabGroup("Combat"), InlineEditor(InlineEditorModes.GUIOnly, Expanded = true), Indent]
        private StatusResistance m_statusResistance;
        #endregion

        [SerializeField, TabGroup("Stat")]
        private PlayerAttributes m_attributes;
        [SerializeField, TabGroup("Stat")]
        private PlayerStatsHandle m_statsHandle;

        [SerializeField, TabGroup("Combat")]
        private Equipment m_equipment;
        [SerializeField, TabGroup("Skill")]
        private Skills m_skills;
        [SerializeField, TabGroup("Runtime Data")]
        private PlayerModifiers m_modifiers;
        private StatusEffectState m_statusEffectState;
        [ShowInInspector, TabGroup("Skill"), HideInEditorMode]
        private SoulSkillManager m_soulSkillManager;
        private IController m_controller;

        public override float critDamageModifier => m_modifiers.critDamageModifier;
        public override IStatusEffectState statusEffectState => m_statusEffectState;
        public override IAttackResistance attackResistance => m_extendedAttackResistance;
        public override IStatusResistance statusResistance => m_statusResistance;
        public override bool isAlive => (m_health?.currentValue ?? 1) > 0;
        private Func<ICappedStat> GetMagic;

        public Skills skills => m_skills;
        public int defense => m_statsHandle.GetStat(PlayerStat.Defense);
        public int magicDefense => m_statsHandle.GetStat(PlayerStat.MagicDefense);
        public ICappedStat health => m_health;
        public ICappedStat magic => GetMagic();
        public IPlayerStats stats => m_statsHandle;
        public SoulSkillManager soulSkillManager => m_soulSkillManager;
        ExtendedAttackResistance IPlayer.attackResistance => m_extendedAttackResistance;

        #region Player Module Implementation
        public PlayerModifiers modifiers => m_modifiers;
        public IAttributes attributes => m_attributes;
        public IEquipment equipment => m_equipment;
        public ProjectileThrowHandler projectileThrowHandler { get; private set; }
        public LootPicker lootPicker { get; private set; }
        [ShowInInspector]
        public PlayerCharacterState characterState { get; private set; }
        public PlayerAnimationState animationState { get; private set; }
        public PlayerSensors sensors { get; private set; }
        public CharacterPhysics2D physics { get; private set; }
        public new PlayerAnimation animation { get; private set; }
        public PlayerCombatAnimation combatAnimation { get; private set; }
        public IsolatedObject isolatedObject { get; private set; }
        public CharacterColliders colliders { get; private set; }
        #endregion

        public event EventAction<FlinchEventArgs> OnFlinch;
        public event EventAction<EventActionArgs> OnDeath;
        
        public event EventAction<CombatStateEventArgs> CombatModeChanged
        {
            add
            {
                characterState.CombatModeChanged += value;
            }
            remove
            {
                characterState.CombatModeChanged -= value;
            }
        }

        public void LoadData(PlayerCharacterData data)
        {
            m_statsHandle.UnsubscribeListeners();
            //m_attributes.LoadData(data.attributeData);
            m_statsHandle.ApplyAttributes();
            m_statsHandle.SubscribeListeners();
            m_soulSkillManager.ClearAllSlots();
            //m_skills.LoadData(data.skills);
        }




        public override void EnableController() => m_controller?.Enable();
        public override void DisableController() => m_controller?.Disable();

        public override void Heal(int health) => m_health.AddCurrentValue(health);

        public override void SetFacing(HorizontalDirection facing)
        {
            m_facing = facing;
            sensors.SetDirection(facing);
        }

        public override void TakeDamage(int totalDamage, AttackType type)
        {
            m_health.ReduceCurrentValue(totalDamage);
            if (isAlive == false)
            {
                DisableController();
                OnDeath?.Invoke(this, EventActionArgs.Empty);
                
            }
        }

        public void Damage(TargetInfo targetInfo, BodyDefense targetDefense)
        {
            var target = targetInfo.target;

            //Reconsider if we need to use interactable or not
            //if (target.CompareTag("Interactable"))
            //{
            //    AttackDamage[] damages = AttackDamage.Add(m_statsHandle.damages, m_modifiers.damageModifier);
            //    for (int i = 0; i < damages.Length; i++)
            //    {
            //        target.TakeDamage(damages[i].damage, damages[i].type);
            //    }
            //}
            //else
            //{
            AttackDamage[] damages = AttackDamage.Add(m_statsHandle.damages, m_modifiers.damageModifier);
            for (int i = 0; i < damages.Length; i++)
            {
                AttackInfo info = new AttackInfo(position, m_statsHandle.GetStat(PlayerStat.CritChance), m_modifiers.critDamageModifier, damages[i]);
                var result = GameplaySystem.combatManager.ResolveConflict(info, targetInfo);
                if (m_equipment.weapon.canInflictStatusEffects && DChildUtility.HasInterface<IStatusReciever>(targetInfo))
                {
                    GameplaySystem.combatManager.InflictStatusTo((IStatusReciever)target, m_equipment.weapon.statusToInflict);
                }
                CallAttackerAttacked(new CombatConclusionEventArgs(info, target, result));
            }
            //}
        }

        public void Flinch(RelativeDirection direction, AttackType damageTypeRecieved)
        {
            if (characterState.canFlinch)
            {
                var eventArgs = new FlinchEventArgs(DamageSourceFacing(direction));
                OnFlinch?.Invoke(this, eventArgs);

            }
        }

        public void Displace(Vector2 force) => physics?.SetVelocity(force);

        public void BecomeInvulnerable(bool value)
        {
            for (int i = 0; i < m_hitboxes.Length; i++)
            {
                m_hitboxes[i].SetInvulnerability(value);
            }
        }

        void IDynamicMagicReference.SetMagicReference(Func<ICappedStat> handle) => GetMagic = handle == null ? GetMagicComponent : handle;

        private ICappedStat GetMagicComponent() => m_magic;

        private void InitializeBehaviours()
        {
            var behaviours = GetComponentsInChildren<IPlayerExternalModule>(true);
            for (int i = 0; i < (behaviours?.Length ?? 0); i++)
            {
                behaviours[i].Initialize(this);
            }
        }

        private RelativeDirection DamageSourceFacing(RelativeDirection m_direction)
        {
            if (m_direction == RelativeDirection.Back)
            {
                return m_direction;
            }
            else
            {
                return m_direction;
            }
        }

        private void Awake()
        {
            m_hitboxes = GetComponentsInChildren<Hitbox>();
            m_controller = GetComponentInChildren<IController>();
            projectileThrowHandler = GetComponentInChildren<ProjectileThrowHandler>();
            sensors = GetComponentInChildren<PlayerSensors>();
            physics = GetComponent<CharacterPhysics2D>();
            animation = GetComponent<PlayerAnimation>();
            combatAnimation = GetComponent<PlayerCombatAnimation>();
            isolatedObject = GetComponent<IsolatedObject>();
            lootPicker = GetComponentInChildren<LootPicker>();
            colliders = GetComponentInChildren<CharacterColliders>();
            m_soulSkillManager = new SoulSkillManager(this);
            characterState = new PlayerCharacterState();
            animationState = new PlayerAnimationState();
            m_statsHandle.Initialize(m_attributes, m_equipment, m_health, m_magic);
            m_statusEffectState = new StatusEffectState();
            InitializeBehaviours();
            GetMagic = GetMagicComponent;
        }

        private void Start()
        {

            m_health?.ResetValueToMax();
            m_magic?.ResetValueToMax();

        }

        private void OnValidate()
        {
            ComponentUtility.AssignNullComponent(this, ref m_health, ComponentUtility.ComponentSearchMethod.Child);
            ComponentUtility.AssignNullComponent(this, ref m_magic, ComponentUtility.ComponentSearchMethod.Child);
            ComponentUtility.AssignNullComponent(this, ref m_extendedAttackResistance, ComponentUtility.ComponentSearchMethod.Child);
            ComponentUtility.AssignNullComponent(this, ref m_statusResistance, ComponentUtility.ComponentSearchMethod.Child);
        }

#if UNITY_EDITOR
        public void Initialize(Transform model)
        {
            m_model = model;
            m_skills = new Skills();
        }

#endif
    }
}