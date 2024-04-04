using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using Holysoft.Event;
using Sirenix.OdinInspector;
using SUtil = Sirenix.Serialization.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    public abstract class CombatAIBrain<T> : AIBrain<T>, ICombatAIBrain, IController where T : IAIInfo
    {
        [Flags]
        protected enum Restriction
        {
            ForbiddenFromAttackingTarget = 1 << 0,
            IgnoreTarget = 1 << 1
        }

        [SerializeField, TabGroup("Reference")]
        protected Damageable m_damageable;
        [SerializeField, TabGroup("Reference")]
        protected Transform m_centerMass;
        [SerializeField]
        private AggroBoundary m_aggroBoundary;
        [SerializeField, TabGroup("Data")]
        protected CharacterStatsData m_statsData;

        protected AITargetInfo m_targetInfo;

        private Attacker m_attacker;
        private AttackResistance m_attackResistance;
        private StatusInflictor m_statusInflictor;
        private StatusEffectResistance m_statusResistance;

        protected Restriction m_currentRestrictions;


        public event EventAction<EventActionArgs<bool>> ControllerStateChange;

        public virtual void ForcePassiveIdle(bool value)
        {
        }

        public virtual void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (m_targetInfo == null)
            {
                m_targetInfo = new AITargetInfo();
            }
            m_targetInfo.Set(damageable, m_target);
        }

        public virtual void ReactToConflict(IAttackerConflictInfo info)
        {
            if (info.isPlayer)
            {
                SetTarget(info.instance.GetComponent<IDamageable>(), info.instance.GetComponent<Character>());
            }
        }

        protected virtual void OnIgnoreAllTargets()
        {

        }

        public virtual void Enable()
        {
            enabled = true;
            var eventCache =new EventActionArgs<bool>();
            eventCache.Set(true);
            ControllerStateChange?.Invoke(this, eventCache);
        }

        public virtual void Disable()
        {
            enabled = false;
            var eventCache = new EventActionArgs<bool>();
            eventCache.Set(false);
            ControllerStateChange?.Invoke(this, eventCache);
        }

        public bool IsFacing(Vector2 position)
        {
            if (position.x > m_character.transform.position.x)
            {
                return m_character.facing == HorizontalDirection.Right;
            }
            else
            {
                return m_character.facing == HorizontalDirection.Left;
            }
        }

        /// <summary>
        /// AI Essentially Resets as its Returns to Spawn Point
        /// </summary>
        public abstract void ReturnToSpawnPoint();

        /// <summary>
        /// Will still be Notice the Target but cannot attack it
        /// </summary>
        /// <param name="value"></param>
        public void ForbidFromAttackTarget(bool value)
        {
            if (value)
            {
                m_currentRestrictions |= Restriction.IgnoreTarget;
                OnForbidFromAttackTarget();
            }
            else
            {
                m_currentRestrictions &= ~Restriction.IgnoreTarget;

            }
        }

        /// <summary>
        /// Target is null but still can accept new Targets
        /// </summary>
        public void IgnoreCurrentTarget()
        {
            SetTarget(null);
        }

        public void SetPassive()
        {
            if (m_targetInfo.doesTargetExist == false)
            {
                if (m_aggroBoundary != null)
            { 
                    m_aggroBoundary.gameObject.SetActive(false);
            }
                SetTarget(null);
                m_currentRestrictions |= Restriction.IgnoreTarget;
            }
            else
            {
                this.enabled = false;
            }
        }
        public void SetActive()
        {

            if (m_aggroBoundary != null)
            {
                m_aggroBoundary.gameObject.SetActive(true);
                m_currentRestrictions &= ~Restriction.IgnoreTarget;
                this.enabled = true;
            }
            
        }

        /// <summary>
        /// Target is null and cannot accept new Targets if True
        /// </summary>
        /// <param name="value"></param>
        public void IgnoreAllTargets(bool value)
        {
            if (value)
            {
                m_aggroBoundary.gameObject.SetActive(false);
                SetTarget(null);
                m_currentRestrictions |= Restriction.IgnoreTarget;
            }
            else
            {
                m_aggroBoundary.gameObject.SetActive(true);
                m_currentRestrictions &= ~Restriction.IgnoreTarget;
            }
        }



        protected void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        protected bool TargetBlocked()
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)m_character.centerMass.position, m_targetInfo.position - (Vector2)m_character.centerMass.position, 1000, LayerMask.GetMask("Player") + DChildUtility.GetEnvironmentMask());
            var eh = hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
            Debug.DrawRay((Vector2)m_character.centerMass.position, m_targetInfo.position - (Vector2)m_character.centerMass.position);
            //Debug.Log("Shot is " + eh + " by " + LayerMask.LayerToName(hit.transform.gameObject.layer));
            return hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
        }

        protected Vector2 GroundPosition(Vector2 startPoint)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint, Vector2.down, 1000, true, out hitCount, true);
            Debug.DrawRay(startPoint, hit[0].point);
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
            return hit[0].point;
        }

        protected Vector2 RoofPosition(Vector2 startPoint)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint, Vector2.up, 1000, true, out hitCount, true);
            Debug.DrawRay(startPoint, hit[0].point);
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
            return hit[0].point;
        }

        protected Vector2 LookPosition(Transform startPoint)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint.position, startPoint.right, 1000, true, out hitCount, true);
            Debug.DrawRay(startPoint.position, hit[0].point);
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
            return hit[0].point;
        }

        private static ContactFilter2D m_contactFilter;
        private static RaycastHit2D[] m_hitResults;
        private static bool m_isInitialized;


        private static void Initialize()
        {
            if (m_isInitialized == false)
            {
                m_contactFilter.useLayerMask = true;
                m_contactFilter.SetLayerMask(DChildUtility.GetEnvironmentMask());
                //m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(DChildUtility.GetEnvironmentMask()));
                m_hitResults = new RaycastHit2D[16];
                m_isInitialized = true;
            }
        }

        protected static RaycastHit2D[] Cast(Vector2 origin, Vector2 direction, float distance, bool ignoreTriggers, out int hitCount, bool debugMode = false)
        {
            Initialize();
            m_contactFilter.useTriggers = !ignoreTriggers;
            hitCount = Physics2D.Raycast(origin, direction, m_contactFilter, m_hitResults, distance);
#if UNITY_EDITOR
            if (debugMode)
            {
                if (hitCount > 0)
                {
                    Debug.DrawRay(origin, direction * m_hitResults[0].distance, Color.cyan, 1f);
                }
                else
                {
                    Debug.DrawRay(origin, direction * distance, Color.cyan, 1f);
                }
            }
#endif
            return m_hitResults;
        }


        /// <summary>
        /// When its told that it cant attack target
        /// </summary>
        protected virtual void OnForbidFromAttackTarget() { }

        protected bool HasRestriction(Restriction restriction) => m_currentRestrictions.HasFlag(restriction);

        protected bool IsFacingTarget() => IsFacing(m_targetInfo.position);
        protected bool IsTargetInRange(float distance) => Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) <= distance;
        protected Vector2 DirectionToTarget() => (m_targetInfo.position - (Vector2)m_character.transform.position).normalized;

        protected void LoadCharacterStatData(CharacterStatsData statData)
        {
            m_damageable.health.SetMaxValue(statData.maxHealth);
            m_attacker?.SetData(statData.damage);
            m_attacker?.SetDamageModifier(1);
            m_attackResistance?.SetData(statData.attackResistance);
            m_attackResistance?.SetData(statData.attackResistance);
            m_statusInflictor?.SetData(statData.statusInfliction);
            m_statusResistance?.SetData(statData.statusResistanceData);
        }

        protected override void Awake()
        {
            if (m_targetInfo == null)
            {
                m_targetInfo = new AITargetInfo();
            }
            base.Awake();
            GameplaySystem.minionManager.Register(this);
            m_damageable.Destroyed += OnDestroyed;

            m_attacker = GetComponent<Attacker>();
            m_attackResistance = GetComponentInChildren<AttackResistance>();
            m_statusInflictor = GetComponent<StatusInflictor>();
            m_statusResistance = GetComponentInChildren<StatusEffectResistance>();
        }

        protected virtual void OnDestroyed(object sender, EventActionArgs eventArgs) => base.enabled = false;

        protected virtual void Start()
        {
            if (m_statsData != null)
            {
                LoadCharacterStatData(m_statsData);
                m_damageable.health.ResetValueToMax();
            }
        }

        protected virtual void OnDisable()
        {
            //Incase the ai is dead before scene change since OnDestroy is
            //not being called if object is disabled
            if (m_damageable.isAlive == false)
            {
                GameplaySystem.minionManager.Unregister(this);
            }
        }

        protected void OnDestroy()
        {
            //Incase the ai is still alive when scene changes
            GameplaySystem.minionManager.Unregister(this);
        }

        protected virtual void LateUpdate()
        {
            if (m_targetInfo.isValid && m_targetInfo.doesTargetExist == false)
            {
                OnTargetDisappeared();
                m_targetInfo.Set(null);
            }
        }

        protected abstract void OnTargetDisappeared();


#if UNITY_EDITOR
        public Type aiDataType => m_data.GetType();
        public CharacterStatsData statsData { get => m_statsData; set => m_statsData = value; }

        public void InitializeField(Character character, SpineRootAnimation spineRoot, Damageable damageable, Transform centerMass)
        {
            m_character = character;
            m_animation = spineRoot;
            m_damageable = damageable;
            m_centerMass = centerMass;
        }

#endif
    }
}