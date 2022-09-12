using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GiantBug02AI : CombatAIBrain<GiantBug02AI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_patrol = new MovementInfo();
            public MovementInfo patrol => m_patrol;
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

          

            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
               
#endif
            }
        }

        private enum State
        {
            Patrol,
            Turning,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

      

        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;

        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        //Patience Handler
        private float m_currentPatience;
        private bool m_enablePatience;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;

        private Vector2 m_startPoint;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                //base.SetTarget(damageable, m_target);
                //m_stateHandle.SetState(State.Chasing);
                m_currentPatience = 0;
                m_enablePatience = false;
            }
            else
            {
                if (!IsTargetInRange(m_info.targetDistanceTolerance))
                {
                    m_enablePatience = true;
                }
            }
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        //Patience Handler
        private void Patience()
        {
            if (m_currentPatience < m_info.patience)
            {
                m_currentPatience += m_character.isolatedObject.deltaTime;
            }
            else
            {
                m_targetInfo.Set(null, null);
                m_enablePatience = false;
                m_stateHandle.SetState(State.Patrol);
            }
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
           
            //m_Audiosource.PlayOneShot(m_Minion_Death_Sound_Clip);
            base.OnDestroyed(sender, eventArgs);
            m_movement.Stop();
        }

        public void SetDirection(float direction)
        {
            transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
        }

       
       

        private IEnumerator Wait()
        {
            while (m_animation.skeletonAnimation.AnimationState.GetCurrent(0).IsComplete)
            {
                yield return null;
            }
        }
        protected override void Start()
        {
            base.Start();
            m_startPoint = transform.position;
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            //m_sound_Q_trigerCollider = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;

        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Patrol:
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(true, false);
                        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    }
                    else
                    {
                        m_movement.Stop();
                        m_turnState = State.ReevaluateSituation;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_movement.Stop();
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    break;
               
                

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    m_stateHandle.SetState(State.Patrol);
                    //if (m_targetInfo.isValid)
                    //{
                    //    m_stateHandle.SetState(State.Chasing);
                    //}
                    //else
                    //{
                    //    m_stateHandle.SetState(State.Patrol);
                    //}
                    break;
                case State.WaitBehaviourEnd:
                   
                    return;
            }

            if (m_enablePatience)
            {
                Patience();
            }

            //m_wallSensor.transform.localScale = new Vector3(transform.localScale.x, m_wallSensor.transform.localScale.y, m_wallSensor.transform.localScale.z);
            //m_groundSensor.transform.localScale = new Vector3(transform.localScale.x, m_groundSensor.transform.localScale.y, m_groundSensor.transform.localScale.z);
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            m_currentPatience = 0;
            m_enablePatience = false;
        }

        public void ResetAI()
        {
            m_targetInfo.Set(null, null);
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
            transform.position = m_startPoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
    }
}
