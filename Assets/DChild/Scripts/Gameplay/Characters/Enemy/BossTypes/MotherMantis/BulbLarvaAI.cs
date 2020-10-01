using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BulbLarvaAI : CombatAIBrain<BulbLarvaAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;
            
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;


            [SerializeField]
            private SimpleAttackInfo m_detonateAttack = new SimpleAttackInfo();
            public SimpleAttackInfo detonateAttack => m_detonateAttack;
            [SerializeField]
            private float m_detonateTime;
            public float detonateTime => m_detonateTime;


            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_spawnAnimation;
            public string spawnAnimation => m_spawnAnimation;
            //[SerializeField, ValueDropdown("GetAnimations")]
            //private string m_idleAnimation;
            //public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_chargeTransitionAnimation;
            public string chargeTransitionAnimation => m_chargeTransitionAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_chargeAnimation;
            public string chargeAnimation => m_chargeAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_detonateAttack.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Spawn,
            Chase,
            Detonate,
            Turning,
            WaitBehaviourEnd,
        }



        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        //[SerializeField, TabGroup("Modules")]
        //private FlinchHandler m_flinchHandle;

        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_explodeFX;
        [SerializeField, TabGroup("AttackBB")]
        private GameObject m_attackBB;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;

        private float m_currentDetonateTime;

        public void GetTarget(AITargetInfo target)
        {
            m_targetInfo = target;
        }


        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.Chase);
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        private IEnumerator SpawnRoutine()
        {
            m_animation.EnableRootMotion(true, true);
            m_animation.SetAnimation(0, m_info.spawnAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spawnAnimation);
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ChargeRoutine()
        {
            m_animation.SetAnimation(0, m_info.chargeTransitionAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chargeTransitionAnimation);
            m_animation.SetAnimation(0, m_info.chargeAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator DetonateRoutine()
        {
            m_animation.SetAnimation(0, m_info.detonateAttack.animation, false);
            yield return new WaitForSeconds(.6f);
            m_attackBB.SetActive(true);
            m_explodeFX.Play();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detonateAttack.animation);
            m_attackBB.SetActive(false);
            m_currentDetonateTime = 0;
            yield return new WaitForSeconds(1);
            this.gameObject.SetActive(false);
            yield return null;
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {

            //m_Audiosource.PlayOneShot(m_Minion_Death_Sound_Clip);
            base.OnDestroyed(sender, eventArgs);
            m_movement.Stop();
            StopAllCoroutines();
            StartCoroutine(DetonateRoutine());
        }

        public void SetDirection(float direction)
        {
            transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
        }

        protected override void Awake()
        {
            base.Awake();
            //m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_stateHandle = new StateHandle<State>(State.Spawn, State.WaitBehaviourEnd);
            //m_sound_Q_trigerCollider = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;

        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Spawn:
                    if (IsFacing(m_targetInfo.position))
                    {
                        m_stateHandle.Wait(State.Chase);
                        StartCoroutine(SpawnRoutine());
                    }
                    else
                    {
                        m_turnState = State.Spawn;
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;
                case State.Chase:
                    if (IsFacing(m_targetInfo.position))
                    {

                        if (IsTargetInRange(m_info.detonateAttack.range))
                        {
                            m_movement.Stop();
                            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            m_stateHandle.Wait(State.Detonate);
                            StartCoroutine(ChargeRoutine());
                        }
                        else
                        {
                            if (m_groundSensor.isDetecting)
                            {
                                m_animation.EnableRootMotion(true, false);
                                m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = 1f;
                                //m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                            }
                            //else
                            //{
                            //    m_movement.Stop();
                            //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            //}
                        }
                    }
                    else
                    {
                        m_turnState = State.Chase;
                        //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;
                case State.Detonate:
                    if (m_currentDetonateTime <= m_info.detonateTime)
                    {
                        m_currentDetonateTime += Time.deltaTime;
                    }
                    else
                    {
                        m_stateHandle.OverrideState(State.WaitBehaviourEnd);
                        StartCoroutine(DetonateRoutine());
                    }
                    break;
                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_movement.Stop();
                    m_turnHandle.Execute();
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            //if (m_enablePatience)
            //{
            //    Patience();
            //}
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Spawn);
        }

        protected override void OnBecomePassive()
        {
        }
    }
}
