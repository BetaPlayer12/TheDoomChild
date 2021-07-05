using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft;
using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public struct EnemyInfoEventArgs : IEventActionArgs
    {
        public EnemyInfoEventArgs(int instanceID, Vector3 position, bool isBoss) : this()
        {
            this.instanceID = instanceID;
            this.position = position;
            this.isBoss = isBoss;
        }

        public int instanceID { get; }
        public Vector3 position { get; }
        public bool isBoss { get; }
    }

    public abstract class Enemy : CombatCharacter, IDamageDealer
    {
        [SerializeField, TitleGroup("Stat"), InlineEditor(InlineEditorModes.GUIOnly, Expanded = true)]
        protected BasicHealth m_health;

        protected IAIBrain m_brain;
        protected BehaviourHandler m_behaviour;
        protected CharacterColliders m_colliders;
        protected bool m_waitForBehaviourEnd;
        protected AttackDamage m_currentDamage;

        public event EventAction<EnemyInfoEventArgs> Death;

        public IAIBrain brain => m_brain;
        public ICappedStat health => m_health;
        public bool waitForBehaviourEnd => m_waitForBehaviourEnd;
        public override bool isAlive => (m_health?.currentValue ?? 1) > 0;
        public abstract EnemyType enemyType { get; }
        public abstract void InitializeAs(bool isAlive);
        protected abstract new CombatCharacterAnimation animation { get; }

        public Invulnerability ignoreInvulnerability => throw new System.NotImplementedException();

        public bool ignoresBlock => throw new System.NotImplementedException();

        public override void DisableController() => m_brain.Enable(false);
        public override void EnableController() => m_brain.Enable(true);

        public override void Heal(int health)
        {
            m_health.AddCurrentValue(health);
        }

        public override void TakeDamage(int totalDamage, AttackType type)
        {
            m_health.ReduceCurrentValue(totalDamage);
            if (isAlive == false)
            {
                DisableHitboxes();
                m_behaviour.SetActiveBehaviour(null);
                OnDeath();
            }
            else
            {
                OnTakeDamage(type);
            }
        }

        public override void SetFacing(HorizontalDirection facing)
        {
            if (facing == HorizontalDirection.Left)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = Vector3.one;
            }
            m_facing = facing;
        }

        public virtual void Damage(TargetInfo targetInfo, BodyDefense targetDefense)
        {
            if (targetDefense.invulnerabilityLevel == Invulnerability.None)
            {
                //using (Cache<AttackerCombatInfo> info = Cache<AttackerCombatInfo>.Claim())
                //{
                //    info.Value.Initialize(position, 0, 1, m_currentDamage);
                //    var result = GameplaySystem.combatManager.ResolveConflict(info, targetInfo);
                //    CallAttackerAttacked(new CombatConclusionEventArgs(info, targetInfo, result));
                //    info.Release();
                //}
            }
        }

        protected void OnTakeDamage(AttackType type)
        {
            m_behaviour.SetActiveBehaviour(null);
            animation?.DoDamage();
        }

        protected void StopActiveBehaviour() => m_behaviour.StopActiveBehaviour(ref m_waitForBehaviourEnd);

        protected void CallDeathEvent()
        {
            Death?.Invoke(this, new EnemyInfoEventArgs(GetInstanceID(), position, enemyType == EnemyType.Boss));
        }

        protected virtual void OnDeath()
        {
            CallDeathEvent();
        }

        protected virtual void Awake()
        {
            m_objectTime = GetComponent<IsolatedObject>();
            m_brain = GetComponent<IAIBrain>();
            m_behaviour = new BehaviourHandler(this);
            m_health.ResetValueToMax();
            m_hitboxes = GetComponentsInChildren<Hitbox>();
            m_colliders = GetComponentInChildren<CharacterColliders>();
        }

        private void OnValidate()
        {
            ComponentUtility.AssignNullComponent(this, ref m_health, ComponentUtility.ComponentSearchMethod.Child);
        }
    }
}
