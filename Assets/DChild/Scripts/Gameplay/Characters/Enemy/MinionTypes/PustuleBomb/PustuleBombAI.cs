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

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/PustuleBomb")]
    public class PustuleBombAI : CombatAIBrain<PustuleBombAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, BoxGroup("Movement")]
            private Vector2 m_forceVelocity;
            public Vector2 forceVelocity => m_forceVelocity;
            [SerializeField, BoxGroup("Movement")]
            private Vector2 m_minFloatVelocity;
            public Vector2 minFloatVelocity => m_minFloatVelocity;
            [SerializeField, BoxGroup("Movement")]
            private float m_floatTwitchInterval;
            public float floatTwitchInterval => m_floatTwitchInterval;

            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                //EMPTY
#endif
            }
        }

        private enum State
        {
            Floating,
            WaitBehaviourEnd,
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Rigidbody2D m_rigidbody2D;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Health m_health;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_legCollider;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;

        private Vector2 m_lastTargetPos;
        private Vector2 m_startPos;
        private bool m_willTwitch;
        private float m_currentTwitchTimer;

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
        }

        private bool IsInRange(Vector2 position, float distance) => Vector2.Distance(position, m_character.centerMass.position) <= distance;

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();

            m_bodyCollider.enabled = false;
            m_selfCollider.SetActive(false);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_animation.DisableRootMotion();
            var rb2d = GetComponent<Rigidbody2D>();
            rb2d.isKinematic = false;
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            m_hitbox.Disable();
            m_animation.SetEmptyAnimation(0, 0);
        }

        protected override void Start()
        {
            base.Start();

            m_willTwitch = true;
            m_animation.DisableRootMotion();
            m_bodyCollider.enabled = false;
            m_startPos = transform.position;

            m_animation.SetAnimation(0, m_info.idleAnimation, true);
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();
            //m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_stateHandle = new StateHandle<State>(State.Floating, State.WaitBehaviourEnd);
        }

        private void Update()
        {

            switch (m_stateHandle.currentState)
            {
                case State.Floating:
                    if (m_willTwitch
                        /*Mathf.Abs(m_character.physics.velocity.x) < m_info.minFloatVelocity.x && Mathf.Abs(m_character.physics.velocity.y) < m_info.minFloatVelocity.y*/)
                    {
                        m_willTwitch = false;
                        m_character.physics.SetVelocity(Vector2.zero);
                        var forceVelocity = new Vector2(UnityEngine.Random.Range(-m_info.forceVelocity.x, m_info.forceVelocity.x), UnityEngine.Random.Range(-m_info.forceVelocity.y, m_info.forceVelocity.y));
                        m_character.physics.AddForce(forceVelocity, ForceMode2D.Impulse);
                        this.transform.Rotate(new Vector3(0, 0, 1f), UnityEngine.Random.Range(-m_info.forceVelocity.x, m_info.forceVelocity.x));
                    }

                    if (m_currentTwitchTimer < m_info.floatTwitchInterval)
                    {
                        m_currentTwitchTimer += Time.deltaTime;
                    }
                    else
                    {
                        m_willTwitch = true;
                        m_currentTwitchTimer = 0;
                    }
                    break;

                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Floating);
            m_hitbox.gameObject.SetActive(true);
        }

        public void ResetAI()
        {
            m_targetInfo.Set(null, null);
            m_stateHandle.OverrideState(State.Floating);
            enabled = true;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }

        public override void ReturnToSpawnPoint()
        {
            throw new NotImplementedException();
        }
    }
}