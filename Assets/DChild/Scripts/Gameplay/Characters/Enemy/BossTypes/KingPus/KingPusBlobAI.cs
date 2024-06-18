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
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/KingPusBlob")]
    public class KingPusBlobAI : CombatAIBrain<KingPusBlobAI.Info>, ISummonedEnemy
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private MovementInfo m_crawlLeft = new MovementInfo();
            public MovementInfo crawlLeft => m_crawlLeft;
            [SerializeField]
            private MovementInfo m_crawlRight = new MovementInfo();
            public MovementInfo crawlRight => m_crawlRight;

            [SerializeField]
            private int m_healValue;
            public int healValue => m_healValue;
            [SerializeField]
            private float m_flySpeed;
            public float flySpeed => m_flySpeed;
            [SerializeField]
            private Vector2 m_flyOffset;
            public Vector2 flyOffset => m_flyOffset;
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_fallAnimation;
            public string fallAnimation => m_fallAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_landAnimation;
            public string landAnimation => m_landAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchLeftAnimation;
            public string flinchLeftAnimation => m_flinchLeftAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchRightAnimation;
            public string flinchRightAnimation => m_flinchRightAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_crawlLeft.SetData(m_skeletonDataAsset);
                m_crawlRight.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Idle,
            //Turning,
            Crawl,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        [SerializeField, TabGroup("Modules")]
        private Rigidbody2D m_rigidbody2D;
        //[SerializeField, TabGroup("Modules")]
        //private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        //[SerializeField, TabGroup("Modules")]
        //private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;

        [SerializeField, TabGroup("Reference")]
        private Transform m_model;
        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Health m_health;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_cielingSensor;
        //[SerializeField, TabGroup("Sensors")]
        //private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_selfSensor;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_healFX;
        [SerializeField, TabGroup("PlaceHolder")]
        private GameObject PlaceholderShake;


        [SerializeField]
        private float m_lifeTime;
        [SerializeField]
        private float m_rotateSpeed;
        [SerializeField]
        private Transform m_master;
        private float m_targetDistance;


        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentMoveSpeed;
        private float m_currentFullCD;
        private float m_currentScale;
        private float m_timeAlive; //
        private bool m_isAlive;
        #region Animation
        private string m_crawlAnimation;
        #endregion

        public int healValue => m_info.healValue;

        private bool m_isNearMaster;
        public event EventAction<EventActionArgs> OnNearToMaster;

        public void SetMaster(Transform master)
        {
            m_master = master;
        }

        public void SummonAt(Vector2 position, AITargetInfo target)
        {
            enabled = false;
            m_flinchHandle.gameObject.SetActive(true);
            m_health.SetHealthPercentage(1f);
            m_hitbox.Enable();
            this.gameObject.SetActive(true);
            this.transform.SetParent(null);
            m_rigidbody2D.velocity = Vector2.zero;
            m_groundSensor.Cast();
            Awake();
            StartCoroutine(FlightRoutine());
            enabled = true;
        }

        public void StartFall()
        {
            m_flinchHandle.gameObject.SetActive(true);
            m_health.SetHealthPercentage(1f);
            m_hitbox.Enable();
            this.gameObject.SetActive(true);
            this.transform.SetParent(null);
            m_rigidbody2D.velocity = Vector2.zero;
            m_groundSensor.Cast();
            StartCoroutine(StartToFallRoutine());

        }

        public void ResetPosition(Vector2 position)
        {
            transform.position = new Vector2(position.x, position.y);
        }
        public IEnumerator ShakeAnimRoutine()
        {
            if (!m_isAlive)
            {
                PlaceholderShake.SetActive(true);
                yield return new WaitForSeconds(0.75f);
                PlaceholderShake.SetActive(false);
            }
            yield return null;
        }

        public void StayDormant(float Randomtime)
        {
            //can call the growth stage here
            enabled = false;
            m_isAlive = false;
            this.gameObject.SetActive(true);
            this.transform.SetParent(null);
            m_rigidbody2D.simulated = false;
            m_rigidbody2D.velocity = Vector2.zero;
            m_isAlive = false;
            m_hitbox.Disable();
            //if the growth animation is made, wait first for it to finish and then mess with this timescale for the pulsing
            m_animation.animationState.TimeScale = Randomtime;
            Awake();
            enabled = true;
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_selfCollider.enabled = false;
                m_currentPatience = 0;
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_hitbox.Disable();
            m_movement.Stop();
            StartCoroutine(ExplodeRoutine());
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            StartCoroutine(FlinchMixRoutine());
        }

        private IEnumerator FlinchMixRoutine()
        {
            //var flinchAnim = m_targetInfo.position.x < transform.position.x ? m_info.flinchRightAnimation : m_info.flinchLeftAnimation;
            m_animation.SetAnimation(1, m_info.flinchRightAnimation, false, 0).MixBlend = MixBlend.First;
            m_animation.AddEmptyAnimation(1, 1, 0).Alpha = 0f;
            m_animation.animationState.GetCurrent(1).Alpha = 1f;
            m_animation.animationState.GetCurrent(1).MixDuration = 1;
            m_animation.animationState.GetCurrent(1).MixTime = 1;
            yield return null;
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                m_flinchHandle.m_autoFlinch = false;
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_stateHandle.OverrideState(State.ReevaluateSituation);
            }
        }
        public IEnumerator StartToFallRoutine()
        {
            PlaceholderShake.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            PlaceholderShake.SetActive(false);
            m_animation.animationState.TimeScale = 1f;
            m_rigidbody2D.simulated = true;
            m_stateHandle.Wait(State.Crawl);
            m_animation.SetAnimation(0, m_info.fallAnimation, true);
            while (!m_groundSensor.isDetecting /*&& !m_cielingSensor.isDetecting*/)
            {
                var dir = m_rigidbody2D.velocity;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                var q = Quaternion.AngleAxis(angle, Vector3.down);
                m_model.rotation = Quaternion.RotateTowards(m_model.rotation, q, m_rotateSpeed * Time.deltaTime);
                yield return null;
            }
            m_movement.Stop();
            m_model.rotation = Quaternion.Euler(Vector3.zero);
            m_animation.SetAnimation(0, m_info.landAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_isAlive = true;
            m_stateHandle.OverrideState(State.Crawl);
            m_animation.animationState.TimeScale = UnityEngine.Random.Range(0.75f, 1.25f);
            yield return null;
        }

        private IEnumerator FlightRoutine()
        {
            m_stateHandle.Wait(State.Crawl);
            m_animation.SetAnimation(0, m_info.fallAnimation, true);
            var offsetAngle = new Vector2(UnityEngine.Random.Range(0, 2) == 1 ? m_info.flyOffset.x : -m_info.flyOffset.x, m_info.flyOffset.y);
            var randomFlySpeed = UnityEngine.Random.Range(m_info.flySpeed - 5f, m_info.flySpeed + 5f);
            m_character.physics.AddForce(Vector2.one * (offsetAngle * randomFlySpeed), ForceMode2D.Impulse);

            while (!m_groundSensor.isDetecting /*&& !m_cielingSensor.isDetecting*/)
            {
                var dir = m_rigidbody2D.velocity;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                var q = Quaternion.AngleAxis(angle, Vector3.down);
                m_model.rotation = Quaternion.RotateTowards(m_model.rotation, q, m_rotateSpeed * Time.deltaTime);
                yield return null;
            }
            m_movement.Stop();
            m_model.rotation = Quaternion.Euler(Vector3.zero);
            m_animation.SetAnimation(0, m_info.landAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.Crawl);
            yield return null;
        }

        public void Explode()
        {
            StopAllCoroutines();
            StartCoroutine(ExplodeRoutine());
        }

        private IEnumerator ExplodeRoutine()
        {
            enabled = false;
            m_hitbox.Disable();
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_animation.SetAnimation(0, m_info.deathAnimation, false);

            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathAnimation);
            m_animation.DisableRootMotion();
            this.gameObject.SetActive(false);
            this.transform.SetParent(m_master);
            this.transform.localPosition = Vector3.zero;
            m_isAlive = false;
            m_timeAlive = 0f;
            yield return null;
        }

        protected override void Start()
        {
            base.Start();
            m_currentMoveSpeed = UnityEngine.Random.Range(m_info.crawlLeft.speed * .75f, m_info.crawlRight.speed * 1.25f);
        }

        protected override void Awake()
        {
            base.Awake();
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
        }


        private void Update()
        {
            if (m_isAlive)
            {
                if (m_lifeTime > m_timeAlive)
                {
                    m_timeAlive += GameplaySystem.time.deltaTime;
                }
                else
                {
                    Explode();
                }
            }
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;

                //case State.Turning:
                //    m_stateHandle.Wait(m_turnState);
                //    m_turnHandle.Execute();
                //    break;

                case State.Crawl:
                    {
                        if (Vector2.Distance(m_master.position, transform.position) <= m_info.targetDistanceTolerance)
                        {
                            if (m_isNearMaster == false)
                            {
                                m_isNearMaster = true;
                                OnNearToMaster?.Invoke(this, EventActionArgs.Empty);
                            }

                            //StopAllCoroutines();
                            //StartCoroutine(ExplodeRoutine(true));
                        }
                        else
                        {
                            m_isNearMaster = false;
                            m_crawlAnimation = transform.position.x < m_master.position.x ? m_info.crawlRight.animation : m_info.crawlLeft.animation;
                            if (IsFacing(m_master.position))
                            {
                                if (/*!m_wallSensor.isDetecting &&*/ m_groundSensor.isDetecting && Mathf.Abs(m_master.position.x - transform.position.x) > 3f)
                                {
                                    m_animation.EnableRootMotion(true, false);
                                    m_animation.SetAnimation(0, m_crawlAnimation, true);
                                }
                                else
                                {
                                    m_movement.Stop();
                                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                }
                                if (m_selfSensor.isDetecting)
                                    m_selfCollider.enabled = true;
                                else
                                    m_selfCollider.enabled = false;
                            }
                            else
                            {
                                m_character.SetFacing(transform.position.x < m_master.position.x ? HorizontalDirection.Right : HorizontalDirection.Left);

                            }
                        }
                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.SetState(State.Crawl);
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Idle);
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Idle);
            m_currentPatience = 0;
            m_selfCollider.enabled = false;
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = false;
            //m_targetInfo.Set(null, null);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
            ResetAI();
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }

        public void DestroyObject()
        {
            throw new NotImplementedException();
        }
    }
}
