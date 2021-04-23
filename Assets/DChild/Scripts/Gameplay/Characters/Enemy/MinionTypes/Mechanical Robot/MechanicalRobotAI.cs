using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MechanicalRobotAI : CombatAIBrain<MechanicalRobotAI.Info>
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


            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;

            //
            [SerializeField]
            private SimpleProjectileAttackInfo m_plasmaBall = new SimpleProjectileAttackInfo();
            public SimpleProjectileAttackInfo plasmaBall => m_plasmaBall;

            //
            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_damageAnimation;
            public string damageAnimation => m_damageAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_plasmaBall.SetData(m_skeletonDataAsset);
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Idle,
            Patrol,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation, // checks everything on what to do next
            WaitBehaviourEnd,
            DelayTimer,
        }



        [SerializeField, TabGroup("Modules")]
        private SimpleTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private SpineEventListener m_sEventListener;
        //Patience Handler
        private float m_currentPatience;
        private bool m_enablePatience;

        //player reference access


        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private ProjectileLauncher m_fireProjectile;
        [SerializeField, TabGroup("References")]
        private Transform m_projectileSource;

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
                base.SetTarget(damageable, m_target);
                m_stateHandle.SetState(State.Chasing);
                m_currentPatience = 0;
                m_enablePatience = false;
            }
            else
            {
                m_enablePatience = true;
            }
        }



        // benjo's alteration
        public Vector2 GetPlayerTransform()
        {
            var m_playerTransform = m_targetInfo.position;
            return m_playerTransform;
        }

        public float GetProjectileSpeed()
        {
            var m_bulletSpeed = m_info.plasmaBall.projectileInfo.speed;
            return m_bulletSpeed;
        }

        /*
        void handleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            Debug.Log("trigger");
            if(e.Data.Name == m_info.plasmaBall.launchOnEvent)
            {
                if (IsFacingTarget())
                {
                    var target = m_targetInfo.position;
                    target = new Vector2(target.x, target.y - 2);
                    Vector2 plasmaBallPos = m_projectileSource.position;
                    Vector3 v_diff = (target - plasmaBallPos);
                    float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                    Vector2 targ = m_targetInfo.position;
                    float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
                    m_projectileSource.rotation = Quaternion.Euler(new Vector3(0, 0, (angle-50)));
                    GameObject shoot = Instantiate(m_info.plasmaBall.projectileInfo.projectile, plasmaBallPos, Quaternion.Euler(0f, 0f, -atan2 * Mathf.Rad2Deg));
                    shoot.GetComponent<Rigidbody2D>().AddForce((m_info.plasmaBall.projectileInfo.speed + (Vector2.Distance(target,transform.position)* 0.35f))*shoot.transform.right, ForceMode2D.Impulse);

                }
            }
        }
        */

        //  protected override void Start()
        //   {
        //  base.Start();
        //  m_animation.animationState.Event += handleEvent;
        //   }

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
            base.OnDestroyed(sender, eventArgs);
            m_movement.Stop();
        }

        public override void ApplyData()
        {
            base.ApplyData();
        }



        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            m_fireProjectile = new ProjectileLauncher(m_info.plasmaBall.projectileInfo, m_projectileSource);
        }

        protected override void Start()
        {
            base.Start();
            m_sEventListener.Subscribe(m_info.plasmaBall.launchOnEvent, m_fireProjectile.LaunchProjectile);
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            Debug.Log("current state is " + m_stateHandle.currentState);
            switch (m_stateHandle.currentState)
            {

                case State.Idle:

                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (m_targetInfo.isValid == false)
                    {
                        m_stateHandle.SetState(State.Patrol);

                    }
                    else
                    {
                        m_stateHandle.SetState(State.ReevaluateSituation);
                    }
                    break;

                case State.Patrol:

                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    break;

                case State.Turning:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_movement.Stop();
                    m_turnHandle.Execute();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_animation.EnableRootMotion(true, false);
                    m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
                    m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
                    Debug.Log("is combat");
                    break;
                case State.Chasing:
                    {
                        Debug.Log("check if chasing");
                        if (IsFacingTarget())
                        {

                            m_wallSensor.Cast();
                            m_groundSensor.Cast();
                            Debug.Log("check if facing");
                            if (!m_wallSensor.isDetecting && m_groundSensor.allRaysDetecting)
                            {
                                Debug.Log("check if sensor");
                                var target = m_targetInfo.position;
                                target.y -= 10f;
                                m_animation.EnableRootMotion(true, false);
                                m_animation.SetAnimation(0, m_info.move.animation, true);


                                if (IsTargetInRange(m_info.attack.range))
                                {
                                    Debug.Log("check if trigger");
                                    m_stateHandle.SetState(State.Attacking);
                                }
                            }
                            else
                            {
                                m_stateHandle.OverrideState(State.Idle);
                            }
                        }
                        else
                        {
                            m_stateHandle.SetState(State.Turning);
                        }
                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.SetState(State.Chasing);
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Idle);
                    }
                    break;
                case State.WaitBehaviourEnd:
                    //Debug.Log("Still wetting");
                    //m_stateHandle.Wait(State.Attacking);
                    //m_stateHandle.Set(State.Chasing);
                    //m_stateHandle.ApplyQueuedState();
                    return;
            }

            if (m_enablePatience)
            {
                Patience();
            }

            m_wallSensor.transform.localScale = new Vector3(transform.localScale.x, m_wallSensor.transform.localScale.y, m_wallSensor.transform.localScale.z);
            m_groundSensor.transform.localScale = new Vector3(transform.localScale.x, m_groundSensor.transform.localScale.y, m_groundSensor.transform.localScale.z);
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
            m_stateHandle.OverrideState(State.Patrol);
            enabled = true;
        }

        protected override void OnBecomePassive()
        {
            ResetAI();
            m_stateHandle.OverrideState(State.Patrol);
        }
    }
}
