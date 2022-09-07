using System;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Pathfinding;
using DarkTonic.MasterAudio;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Holysoft.Collections;
using Holysoft.Pooling;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/FireDragonHeadAI")]
    public class FireDragonHeadAI : CombatAIBrain<FireDragonHeadAI.Info>, ISpawnable
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Attack Behaviours
            [SerializeField, BoxGroup("Attack"), ValueDropdown("GetAnimations")]
            private SimpleAttackInfo m_attackFlame = new SimpleAttackInfo();
            public SimpleAttackInfo attackFlame => m_attackFlame;
            [SerializeField, BoxGroup("Attack"), ValueDropdown("GetAnimations")]
            private string m_attackFlameStartAnimation;
            public string attackFlameStartAnimation => m_attackFlameStartAnimation;
            [SerializeField, BoxGroup("Attack")]
            private float m_attackCD;
            public float attackCD => m_attackCD;

            public override void Initialize()
            {

            }
        }
        private enum State
        {
            Turning,
            WaitBehaviourEnd,
            Attacking,
        }

        private enum Attack
        {
            //Attack,
            AttackFlame,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Rigidbody2D m_rigidbody2D;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_legCollider;
        [SerializeField, TabGroup("Reference")]
        private Transform m_fireBreathPosition;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_roofSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_selfSensor;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;

        private ProjectileLauncher m_projectileLauncher;

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        private float m_currentCD;
        private Vector2 m_lastTargetPos;
        private Vector2 m_startPos;

        private Coroutine m_executeMoveCoroutine;
        private Coroutine m_attackRoutine;
        private Coroutine m_patienceRoutine;

        public event EventAction<PoolItemEventArgs> PoolRequest;
        public event EventAction<PoolItemEventArgs> InstanceDestroyed;

        public PoolableItemData poolableItemData => throw new NotImplementedException();

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(/*new AttackInfo<Attack>(Attack.Attack, m_info.attack.range),*/
                                    new AttackInfo<Attack>(Attack.AttackFlame, m_info.attackFlame.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            if (IsFacingTarget())
                CustomTurn();

            m_bodyCollider.enabled = false;
            m_selfCollider.SetActive(false);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_animation.DisableRootMotion();
            var rb2d = GetComponent<Rigidbody2D>();
            rb2d.isKinematic = false;
            m_hitbox.Disable();
            m_animation.SetEmptyAnimation(0, 0);
        }

        #region Attack
        private void ExecuteAttack(Attack m_attack)
        {
            m_bodyCollider.enabled = true;
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            switch (/*m_attack*/ m_currentAttack)
            {
                //case Attack.Attack:
                //    StartCoroutine(AttackRoutine());
                //    break;
                case Attack.AttackFlame:
                    m_lastTargetPos = m_targetInfo.position;
                    //m_attackHandle.ExecuteAttack(m_info.attackFrost.animation, m_info.idleAnimation);
                    m_attackRoutine = StartCoroutine(FlameAttackRoutine());
                    break;
            }
        }

        private IEnumerator FlameAttackRoutine()
        {
            m_animation.SetAnimation(0, m_info.attackFlameStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackFlameStartAnimation); 
            yield return AdjustDragonHeadInRelationToTarget();
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
            m_attackDecider.hasDecidedOnAttack = false;
            DespawnFireDragonHead();
        }

        private IEnumerator AdjustDragonHeadInRelationToTarget()
        {
            Debug.Log("Adjusting Dragon");
            Vector2 fireBreathPos = new Vector2(m_fireBreathPosition.position.x, m_fireBreathPosition.position.y);
            RaycastHit2D hit = Physics2D.Raycast(fireBreathPos, m_targetInfo.position);
            if (hit.collider != null)
            {
                Debug.DrawRay(fireBreathPos, m_targetInfo.position, Color.red);
                var rad = Mathf.Atan2(m_targetInfo.position.y, fireBreathPos.x);
                transform.rotation = Quaternion.Euler(0f, 0f, rad);
            }
            yield return null;
        }

        public void DespawnFireDragonHead()
        {
            DestroyInstance();
        }

        private void ChooseAttack()
        {
            if (!m_attackDecider.hasDecidedOnAttack)
            {
                IsAllAttackComplete();
                for (int i = 0; i < m_attackCache.Count; i++)
                {
                    m_attackDecider.DecideOnAttack();
                    if (m_attackCache[i] != m_currentAttack && !m_attackUsed[i])
                    {
                        m_attackUsed[i] = true;
                        m_currentAttack = m_attackCache[i];
                        m_currentAttackRange = m_attackRangeCache[i];
                        return;
                    }
                }
            }
        }
        private IEnumerator ExecuteMove(float attackRange, /*float heightOffset,*/ Attack attack)
        {
            bool inRange = false;
            while (!inRange || TargetBlocked())
            {

                bool xTargetInRange = Mathf.Abs(m_targetInfo.position.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(m_targetInfo.position.y - transform.position.y) < attackRange/*1*/ ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    inRange = true;
                }
                yield return null;
            }
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            ExecuteAttack(attack);
            yield return null;
        }

        void AddToAttackCache(params Attack[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackCache.Add(list[i]);
            }
        }

        void AddToRangeCache(params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackRangeCache.Add(list[i]);
            }
        }
        #endregion

        protected override void Start()
        {
            base.Start();
            m_animation.DisableRootMotion();
            m_bodyCollider.enabled = false;
            m_startPos = transform.position;
            Debug.Log("player pos: " + m_targetInfo.position);
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            //m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, transform);
            m_stateHandle = new StateHandle<State>(State.Attacking, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.AttackFlame);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.attackFlame.range);
            m_attackUsed = new bool[m_attackCache.Count];
        }

        private void IsAllAttackComplete()
        {
            for (int i = 0; i < m_attackUsed.Length; ++i)
            {
                if (!m_attackUsed[i])
                {
                    return;
                }
            }
            for (int i = 0; i < m_attackUsed.Length; ++i)
            {
                m_attackUsed[i] = false;
            }
        }

        private void Update()
        {

            switch (m_stateHandle.currentState)
            {
                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                    if (m_executeMoveCoroutine != null)
                    {
                        StopCoroutine(m_executeMoveCoroutine);
                        m_executeMoveCoroutine = null;
                    }
                    m_turnHandle.Execute();
                    break;

                case State.Attacking:
                    //m_animation.SetAnimation(0, m_info.m_attackFlameStartAnimation, true);
                    m_executeMoveCoroutine = StartCoroutine(ExecuteMove(m_currentAttackRange, m_currentAttack));
                    m_attackDecider.hasDecidedOnAttack = false;
                    break;

                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_hitbox.gameObject.SetActive(true);
        }

        public void ResetAI()
        {
            m_targetInfo.Set(null, null);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
            //Patience();
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }

        public void SpawnAt(Vector2 position, Quaternion rotation)
        {
            throw new NotImplementedException();
        }

        public void DestroyInstance()
        {
            throw new NotImplementedException();
        }
    }
}

