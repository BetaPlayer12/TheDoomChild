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
using DChild.Gameplay.Characters.Players.Modules;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/CorpseWeaver")]
    public class CorpseWeaverAI : CombatAIBrain<CorpseWeaverAI.Info>, IResetableAIBrain, IKnockbackable
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
            [SerializeField]
            private MovementInfo m_run = new MovementInfo();
            public MovementInfo run => m_run;
            //Attack Behaviours
            [SerializeField, MinValue(0)]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            [SerializeField]
            private SimpleAttackInfo m_clawAttack = new SimpleAttackInfo();
            public SimpleAttackInfo clawAttack => m_clawAttack;
            [SerializeField]
            private SimpleAttackInfo m_jumpAttack = new SimpleAttackInfo();
            public SimpleAttackInfo jumpAttack => m_jumpAttack;
            [SerializeField]
            private SimpleAttackInfo m_jumpAttackStart = new SimpleAttackInfo();
            public SimpleAttackInfo jumpAttackStart => m_jumpAttackStart;
            [SerializeField]
            private SimpleAttackInfo m_jumpAttackEnd = new SimpleAttackInfo();
            public SimpleAttackInfo jumpAttackEnd => m_jumpAttackEnd;
            [SerializeField]
            private SimpleAttackInfo m_biteAttack = new SimpleAttackInfo();
            public SimpleAttackInfo biteAttack => m_biteAttack;
            [SerializeField]
            private SimpleAttackInfo m_spitAttack = new SimpleAttackInfo();
            public SimpleAttackInfo spitAttack => m_spitAttack;
            //

            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private float m_rampageDuration;

            public float rampageDuration => m_rampageDuration;

            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_damageAnimation;
            public BasicAnimationInfo damageAnimation => m_damageAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_playerDetect1;
            public BasicAnimationInfo playerDetect1 => m_playerDetect1;
            [SerializeField]
            private BasicAnimationInfo m_playerDetect2;
            public BasicAnimationInfo playerDetect2 => m_playerDetect2;



            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;


            //[SerializeField]
            //private GameObject m_spitProjectile;
            //public GameObject spitProjectile => m_spitProjectile;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_run.SetData(m_skeletonDataAsset);
                m_playerDetect1.SetData(m_skeletonDataAsset);
                m_playerDetect2.SetData(m_skeletonDataAsset);
                m_clawAttack.SetData(m_skeletonDataAsset);
                m_jumpAttack.SetData(m_skeletonDataAsset);
                m_jumpAttackStart.SetData(m_skeletonDataAsset);
                m_jumpAttackEnd.SetData(m_skeletonDataAsset);
                m_biteAttack.SetData(m_skeletonDataAsset);
                m_spitAttack.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_damageAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Patrol,
            Idle,
            Detect,
            Turning,
            Stalk,
            Attacking,
            //Cooldown,
            Chasing,
            Flinch,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Spit,
            Bite,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        //Patience Handler
        private float m_currentPatience;
        private bool m_enablePatience;

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private IsolatedCharacterPhysics2D m_charcterPhysics;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private CobWebTrigger m_cobWebTrigger;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField]
        private bool m_willPatrol = true;
        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_muzzleFX;


        [SerializeField, TabGroup("Cannon Values")]
        private float m_speed;
        [SerializeField, TabGroup("Cannon Values")]
        private float m_gravityScale;
        [SerializeField, TabGroup("Cannon Values")]
        private Vector2 m_posOffset;
        [SerializeField, TabGroup("Cannon Values")]
        private float m_velOffset;
        [SerializeField, TabGroup("Cannon Values")]
        private Vector2 m_targetOffset;
        [SerializeField, TabGroup("Cannon Values")]
        private ShadowDrain m_shadowDrain;

        private float m_targetDistance;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;

        private ProjectileLauncher m_projectileLauncher;
        [SerializeField]
        private Transform m_throwPoint;
        [SerializeField]
        private float m_rampageDuration;

        private PlayerDamageable m_playerDamageable;


        private bool m_isDetecting;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentTimeScale;
        private Vector2 m_targetLastPos;
        private Vector2 m_startPoint;
        private Coroutine m_randomTurnRoutine;
        private bool m_isBiting = false;

        protected override void Start()
        {
            base.Start();
            m_cobWebTrigger.CobWebEnterEvent += CobwebEvent;
            m_cobWebTrigger.Onhit += HitCobWeb;
            m_currentTimeScale = UnityEngine.Random.Range(1.0f, 2.0f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);

            //m_randomTurnRoutine = StartCoroutine(RandomTurnRoutine());
            //if (m_willPatrol)
            //{
            //    StopCoroutine(m_randomTurnRoutine);
            //}
            m_spineEventListener.Subscribe(m_info.projectile.launchOnEvent, SpitProjectile);
            m_startPoint = transform.position;
        }

        

        private Vector2 BallisticVel()
        {
            Vector2 targetCenterMass = m_targetLastPos;
            m_info.projectile.projectileInfo.projectile.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale = m_gravityScale;

            m_targetDistance = Vector2.Distance(targetCenterMass, m_throwPoint.position);
            var dir = (targetCenterMass - new Vector2(m_throwPoint.position.x, m_throwPoint.position.y));
            var h = dir.y;
            dir.y = 0;
            var dist = dir.magnitude;
            dir.y = dist;
            dist += h;

            var currentSpeed = m_speed;

            var vel = Mathf.Sqrt(dist * m_info.projectile.projectileInfo.projectile.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale);
            return (vel * new Vector3(dir.x * m_posOffset.x, dir.y * m_posOffset.y).normalized) * m_targetOffset.sqrMagnitude; //closest to accurate
        }

        private float GroundDistance()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_throwPoint.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            if (hit.collider != null)
            {
                return hit.distance;
            }

            return 0;
        }

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        private void SpitProjectile()
        {
            if (m_targetInfo.isValid)
            {
                //if (!IsFacingTarget())
                //{
                //    CustomTurn();
                //}
                //Dirt FX
                //GameObject obj = Instantiate(m_info.mouthSpitFX, m_seedSpitTF.position, Quaternion.identity);
                //obj.transform.localScale = new Vector3(obj.transform.localScale.x * transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
                //obj.transform.parent = m_seedSpitTF;
                //obj.transform.localPosition = new Vector2(4, -1.5f);
                //


                //Shoot Spit
                m_muzzleFX.Play();
                Vector2 target = m_targetLastPos;
                target = new Vector2(target.x, target.y - 2);
                Vector2 spitPos = new Vector2(transform.localScale.x < 0 ? m_throwPoint.position.x - 1.5f : m_throwPoint.position.x + 1.5f, m_throwPoint.position.y - 0.75f);
                Vector3 v_diff = (target - spitPos);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);

                var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_info.projectile.projectileInfo.projectile);
                instance.transform.position = m_throwPoint.position;
                var component = instance.GetComponent<Projectile>();
                component.ResetState();
                //component.GetComponent<IsolatedObjectPhysics2D>().AddForce(BallisticVel(), ForceMode2D.Impulse);
                component.GetComponent<IsolatedObjectPhysics2D>().SetVelocity(BallisticVel());
                //return instance.gameObject;
            }
        }



        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_animation.DisableRootMotion();
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
            
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
          
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_selfCollider.enabled = false;
                m_currentPatience = 0;
                m_enablePatience = false;
                //StopCoroutine(PatienceRoutine()); //for latur

                if (!m_isDetecting && !isPlayerDetected)
                {
                    m_isDetecting = true;
                    m_stateHandle.OverrideState(State.Detect);
                    Debug.Log("Set target");
                }
                else if(m_stateHandle.currentState == State.Patrol)
                {
                    m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //Debug.Log("chase state ");
                    //m_stateHandle.OverrideState(State.Attacking);
                }

                if (transform.localRotation.z != 0)
                {
                    transform.localRotation = Quaternion.Euler(Vector3.zero);
                    GetComponent<IsolatedPhysics2D>().simulateGravity = true;
                    m_animation.DisableRootMotion();
                }
            }
            else
            {
                ///*_enablePatience = true;*/
                //StopAllCoroutines();

                m_isDetecting = false;
                //isPlayerDetected = false;
                if(m_stateHandle.currentState == State.WaitBehaviourEnd)
                {
                    m_stateHandle.Wait(State.Patrol);
                    StopAllCoroutines();
                    StartCoroutine(PatienceRoutine());
                    m_stateHandle.ApplyQueuedState();
                    Debug.Log("State of waitBehaviourEnd");
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(PatienceRoutine());
                }
               
                Debug.Log("cancel target");
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        
        private IEnumerator PatienceRoutine()
        {
     
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_movement.Stop();
            m_isDetecting = false;
            m_rampageDuration = 10f;
            //needs rework
            if (m_wallSensor.isDetecting && m_groundSensor.isDetecting && !m_edgeSensor.isDetecting)
            {
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_movement.Stop();
            }
          
            m_movement.Stop();
            yield return null;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            m_movement.Stop();
            m_selfCollider.enabled = false;
        }
        public void ActivateShadowDrain()
        {
            m_shadowDrain.ActivateShadowDrain();
        }
        public void DeActivateShadowDrain()
        {

        }
        public void ActivateBite()
        {
            if (m_isBiting == false)
            {
                m_isBiting = true;
                StopAllCoroutines();
                //m_stateHandle.SetState(State.Chasing);
            }
        }
        [SerializeField]
        public bool isPlayerDetected = false;
        private IEnumerator DetectRoutine()
        {
            Debug.Log("Detected");
            m_isDetecting = true;
            m_animation.SetAnimation(0, m_info.playerDetect1, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.playerDetect1);
        }

        private IEnumerator FollowRoutine()
        {
            m_stateHandle.Wait(State.Attacking);
            do
            {
                StalkingBehaviourRoutine();
                yield return null;
            } while (!isPlayerDetected);

            m_stateHandle.ApplyQueuedState();
        }

        private void StalkingBehaviourRoutine()
        {
          //  m_animation.EnableRootMotion(false, false);
            if(!m_wallSensor.isDetecting && m_edgeSensor.isDetecting && m_groundSensor.isDetecting)
            {
                if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) < 20f)
                {

                    m_animation.SetAnimation(0, m_info.patrol, true);
                    if (IsFacingTarget())
                    {
                        CustomTurn();
                    }
                    if (m_character.facing == HorizontalDirection.Right)
                    {
                        m_movement.MoveTowards(Vector2.right, m_info.patrol.speed);
                    }
                    else if (m_character.facing == HorizontalDirection.Left)
                    {
                        m_movement.MoveTowards(Vector2.left, m_info.patrol.speed);
                    }
                    Debug.Log("move away");
                }
                else if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > 30f)
                {
                    if (!IsFacingTarget())
                    {
                       
                        CustomTurn();
                    }
                    if (m_character.facing == HorizontalDirection.Right)
                    {

                        m_movement.MoveTowards(Vector2.right, m_info.patrol.speed);
                    }
                    else if (m_character.facing == HorizontalDirection.Left)
                    {
                        m_movement.MoveTowards(Vector2.left, m_info.patrol.speed);
                    }
                    m_animation.SetAnimation(0, m_info.patrol, true);
                    Debug.Log("Move");
                }
                else
                {

                    if (!IsFacingTarget())
                    {
                        CustomTurn();
                    }
                    m_movement.Stop();
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                  
                }
            }else
            {
                if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > 30f)
                {

                    if (!IsFacingTarget())
                    {
                        CustomTurn();
                    }
                }
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);  
            }

            //else
            //{
            //    if (!IsFacingTarget())
            //        CustomTurn();

            //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //    m_movement.Stop();
            //    Debug.Log("else in follow routine");

            //    //if(m_wallSensor.isDetecting || !m_edgeSensor.isDetecting)
            //    //{
            //    //    //if (!IsFacingTarget())
            //    //    //    CustomTurn();
            //    //    m_animation.EnableRootMotion(true, true);
            //    //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //    //    m_movement.Stop();
            //    //    Debug.Log("stop");
            //    //}
            //}
        }

        private IEnumerator DetectionMovement()
        {
            m_stateHandle.Wait(State.Stalk);
            m_animation.EnableRootMotion(false, false);
            yield return DetectRoutine();
            m_stateHandle.ApplyQueuedState();
        }


        #region UnUsedBehaviorRoutines
        //Patience Handler
        private void Patience()
        {
            if (m_currentPatience < m_info.patience)
            {
                m_currentPatience += m_character.isolatedObject.deltaTime;
            }
            else
            {
                StopAllCoroutines();
                m_selfCollider.enabled = false;
                // m_targetInfo.Set(null, null);
                m_flinchHandle.m_autoFlinch = true;
                m_isDetecting = false;
                m_enablePatience = false;
                m_stateHandle.SetState(State.Patrol);
            }
        }
        private IEnumerator RandomTurnRoutine()
        {
            while (true)
            {
                var timer = UnityEngine.Random.Range(5, 10);
                var currentTimer = 0f;
                while (currentTimer < timer)
                {
                    currentTimer += Time.deltaTime;
                    yield return null;
                }
                m_turnState = State.Idle;
                m_stateHandle.SetState(State.Turning);
                yield return null;
            }
        }

        private IEnumerator ProjectileRoutine()
        {

            m_movement.Stop();
            m_targetLastPos = m_targetInfo.transform.GetComponent<Character>().centerMass.position;
            m_animation.SetAnimation(0, m_info.spitAttack.animation, false).MixDuration = 0;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spitAttack.animation);
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.move.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.move.animation);
            m_animation.EnableRootMotion(false, false);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ClawRoutine()
        {
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.clawAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.clawAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private IEnumerator RunRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_isDetecting = true;
            var direction = (int)m_character.facing * Vector2.right;
            do
            {
                m_animation.SetAnimation(0, m_info.run, true);

                m_movement.MoveTowards(direction, m_info.run.speed);
                yield return null;
            } while (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > 10f);

            m_stateHandle.SetState(State.Attacking);
            m_stateHandle.ApplyQueuedState();
            // m_movement.Stop();


        }
        private IEnumerator MoveRoutine()
        {

            var direction = (int)m_character.facing * Vector2.right;
            do
            {
                m_animation.SetAnimation(0, m_info.patrol, true);
                m_movement.MoveTowards(direction, m_info.patrol.speed);
                yield return null;
            } while (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > 30f && !m_wallSensor.isDetecting
            && m_groundSensor.isDetecting);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_movement.Stop();
        }
        private IEnumerator MoveBackRoutine()
        {

            //var direction = (int)m_character.facing * Vector2.left;
            do
            {
                m_animation.SetAnimation(0, m_info.patrol, true);
                //m_movement.MoveTowards(direction, m_info.patrol.speed);
                yield return null;
            } while (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > 20f && !m_wallSensor.isDetecting
            && m_groundSensor.isDetecting);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_movement.Stop();
        }
        #endregion

        private IEnumerator ChaseRoutine()
        {
            
            var direction = (int)m_character.facing * Vector2.right;
            isPlayerDetected = true;
            do
            {
              

                m_animation.SetAnimation(0, m_info.run, true);
                m_movement.MoveTowards(direction, m_info.run.speed);
                yield return null;
            } while (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > 10f && m_rampageDuration >= 0f
            && !m_wallSensor.isDetecting &&
                    m_edgeSensor.isDetecting && m_groundSensor.isDetecting);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_movement.Stop();
            Debug.Log("stop");
        }

        private void RunToCharacter()
        {
         
            var direction = (int)m_character.facing * Vector2.right;
           // isPlayerDetected = true;
            if (!m_wallSensor.isDetecting && m_edgeSensor.isDetecting && m_groundSensor.isDetecting)
            {
                m_animation.SetAnimation(0, m_info.run, true);
                m_movement.MoveTowards(direction, m_info.run.speed);
            }
            else 
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                {
                    m_movement.Stop();
                }
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            Debug.Log("run to character function");
        }

        private IEnumerator RampageRoutine()
        {
            m_flinchHandle.m_autoFlinch = true;
            m_animation.SetAnimation(0, m_info.jumpAttack.animation, false);
           yield return new WaitForAnimationComplete(m_animation.animationState, m_info.jumpAttack.animation);
        }
      
        private IEnumerator RampageDuration()
        {
           
            while(m_rampageDuration >= 0)
            {
                m_rampageDuration -= Time.deltaTime;
                Debug.Log("Duration: " + m_rampageDuration);
                yield return null;
            }
            Debug.Log("Stop timer");
            yield return null;
        }
       
        private IEnumerator RampageAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            StartCoroutine(RampageDuration());
            do
            {
                if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) < 15f)
                {
                    //StartCoroutine(RampageRoutine());
                    m_movement.Stop();
                    //yield return RampageRoutine();
                    m_animation.SetAnimation(0, m_info.jumpAttack.animation, true);
                    //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.jumpAttack.animation);
                    Debug.Log("Attacking");
                }
                else
                {
                    if (!IsFacingTarget())
                        CustomTurn();
                    //m_animation.SetAnimation(0, m_info.idleAnimation.animation, true);
                    //yield return ChaseRoutine();
                    var currentAnimation = m_animation.GetCurrentAnimation(0);
                    yield return new WaitForAnimationComplete(m_animation.animationState, currentAnimation);
                    RunToCharacter();

                }
               
                yield return null;
            } while (m_rampageDuration >= 0f);
            Debug.Log("DONE RAMPAGE!");
            m_animation.SetAnimation(0, m_info.idleAnimation.animation, true);
            yield return new  WaitForSeconds(2f);
            m_rampageDuration = 10f;
           m_stateHandle.ApplyQueuedState();
        }
        private IEnumerator BiteRoutine()
        {
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.biteAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.biteAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
            m_isBiting = false;
            yield return null;
        }

        private IEnumerator PatrolRoutine()
        {

            if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
            {
                //m_turnState = State.ReevaluateSituation;
               // m_animation.EnableRootMotion(false, false);
                m_animation.SetAnimation(0, m_info.patrol.animation, true);
                var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
            }
            //else
            //{

            //    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
            //    {
            //        m_movement.Stop();
                   
            //    }
            //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //}

            yield return null;
        }

        
        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            isPlayerDetected = true;
            if (m_flinchHandle.m_autoFlinch)
            {
                //isPlayerDetected = true;
                //StopAllCoroutines();
               // m_animation.SetAnimation(0, m_info.damageAnimation, false);
              //  m_stateHandle.Wait(State.ReevaluateSituation);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
                   // m_animation.SetEmptyAnimation(0, 0);
                m_selfCollider.enabled = false;
                m_stateHandle.ApplyQueuedState();
            }
        }

        public override void ApplyData()
        {
            base.ApplyData();
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(/*new AttackInfo<Attack>(Attack.Spike, m_info.spikeAttack.range),*/
                                    new AttackInfo<Attack>(Attack.Spit, m_info.spitAttack.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            //m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_throwPoint);
            UpdateAttackDeciderList();
        }

        private void CobwebEvent(object sender, EventActionArgs eventArgs)
        { 
            isPlayerDetected = true;
            var playerDamageable = m_cobWebTrigger.playerDamageable;
            SetTarget(playerDamageable);
            Debug.Log("true");
        }
        private void HitCobWeb(object sender, EventActionArgs eventArgs)
        {
            isPlayerDetected = true;
            var playerDamageable = m_cobWebTrigger.playerDamageable;
            SetTarget(playerDamageable);
            Debug.Log("hit boss?");
        }

        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Ground Sensor is " + m_groundSensor.isDetecting);
           // isPlayerDetected = m_webTriggerStatus.m_isPlayerCaught;
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    StartCoroutine(DetectionMovement());
                    break;
                case State.Idle:
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                    {
                        m_movement.Stop();
                        m_animation.EnableRootMotion(true, true);
                    }
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Stalk:
                    StartCoroutine(FollowRoutine());
                    break;
                case State.Patrol:
                    StartCoroutine(PatrolRoutine());
                    #region OldRoutines
                    //if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    //{
                    //    m_turnState = State.ReevaluateSituation;
                    //    m_animation.EnableRootMotion(false, false);
                    //    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    //    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    //    m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    //}
                    //else
                    //{

                    //    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                    //    {
                    //        m_movement.Stop();
                    //        m_animation.EnableRootMotion(true, true);
                    //    }
                    //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    //}
                    #endregion

                    break;

                case State.Turning:
                    Debug.Log("turning");
                    //m_stateHandle.Wait(m_turnState);
                    //StopAllCoroutines();
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_turnHandle.Execute();
                    break;

                case State.Attacking:
                    //m_stateHandle.Wait(State.Cooldown);
                    m_flinchHandle.m_autoFlinch = false;
                    if (!IsFacingTarget())
                    {
                        CustomTurn();
                    }

                    StartCoroutine(RampageAttack());
                    #region OldAttackRoutine
                    //switch (m_attackDecider.chosenAttack.attack)
                    //{

                    //    case Attack.Spit:
                    //        m_animation.EnableRootMotion(false, false);
                    //        if (m_isBiting == true)
                    //        {
                    //            StartCoroutine(BiteRoutine());
                    //        }
                    //        else
                    //        {
                    //            if (IsTargetInRange(m_info.clawAttack.range))
                    //            {
                    //                StartCoroutine(ClawRoutine());
                    //            }
                    //            else
                    //            {
                    //                if (IsTargetInRange(m_info.jumpAttack.range))
                    //                {
                    //                    StartCoroutine(JumpRoutine());
                    //                }
                    //                else
                    //                {
                    //                    StartCoroutine(ProjectileRoutine());
                    //                }
                    //            }
                    //        }
                    //        break;
                    //}
                    //m_attackDecider.hasDecidedOnAttack = false;
                    #endregion


                    break;
                #region ChaseStateRemoved
                //case State.Cooldown:
                //    //m_stateHandle.Wait(State.ReevaluateSituation);
                //    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                //    if (!IsFacingTarget())
                //    {
                //        m_turnState = State.Cooldown;
                //        m_stateHandle.SetState(State.Turning);
                //    }
                //    else
                //    {
                //        if (m_animation.animationState.GetCurrent(0).IsComplete)
                //        {
                //            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                //        }
                //    }

                //    if (m_currentCD <= m_currentFullCD)
                //    {
                //        m_currentCD += Time.deltaTime;
                //    }
                //    else
                //    {
                //        m_currentCD = 0;
                //        m_stateHandle.OverrideState(State.ReevaluateSituation);
                //    }

                //    break;
                //    case State.Chasing:
                //        {
                //            if (!IsFacingTarget())
                //            {
                //                CustomTurn();
                //            }
                //            StartCoroutine(RunRoutine());
                //            //    var m_currentMoveSpeed = UnityEngine.Random.Range(m_info.move.speed * .75f, m_info.move.speed * 1.25f);
                //            //    var toTarget = m_targetInfo.position - (Vector2)m_character.centerMass.position;
                //            //    if (IsFacingTarget())
                //            //    {
                //            //        if (m_isBiting == true) {
                //            //            m_attackDecider.DecideOnAttack();
                //            //            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_info.biteAttack.range) && !m_wallSensor.allRaysDetecting)
                //            //            {
                //            //                m_movement.Stop();
                //            //                m_selfCollider.enabled = true;
                //            //                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                //            //                m_stateHandle.SetState(State.Attacking);
                //            //            }
                //            //            else
                //            //            {
                //            //                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting /*&& m_edgeSensor.isDetecting*/)
                //            //                {
                //            //                    m_animation.EnableRootMotion(false, false);
                //            //                    m_selfCollider.enabled = false;
                //            //                    m_animation.SetAnimation(0, m_info.move.animation, true);
                //            //                    m_character.physics.SetVelocity(toTarget.normalized.x * m_currentMoveSpeed, m_character.physics.velocity.y);
                //            //                }
                //            //                else
                //            //                {
                //            //                    m_attackDecider.hasDecidedOnAttack = false;
                //            //                    m_movement.Stop();
                //            //                    m_selfCollider.enabled = true;
                //            //                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                //            //                }
                //            //            }
                //            //        }

                //            //    else
                //            //    {
                //            //        m_attackDecider.DecideOnAttack();
                //            //        if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting)
                //            //        {
                //            //            m_movement.Stop();
                //            //            m_selfCollider.enabled = true;
                //            //            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                //            //            m_stateHandle.SetState(State.Attacking);
                //            //        }
                //            //        else
                //            //        {
                //            //            if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting /*&& m_edgeSensor.isDetecting*/)
                //            //            {
                //            //                m_animation.EnableRootMotion(false, false);
                //            //                m_selfCollider.enabled = false;
                //            //                m_animation.SetAnimation(0, m_info.move.animation, true);
                //            //                m_character.physics.SetVelocity(toTarget.normalized.x * m_currentMoveSpeed, m_character.physics.velocity.y);
                //            //            }
                //            //            else
                //            //            {
                //            //                m_attackDecider.hasDecidedOnAttack = false;
                //            //                m_movement.Stop();
                //            //                m_selfCollider.enabled = true;
                //            //                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                //            //            }
                //            //        }
                //            //    }
                //            //}

                //            //    else
                //            //    {
                //            //    m_turnState = State.ReevaluateSituation;
                //            //    m_stateHandle.SetState(State.Turning);
                //            //}
                //        }
                //break;

                #endregion


                case State.ReevaluateSituation:
                  //  How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.SetState(State.Attacking);
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Patrol);
                        Debug.Log("ReevaluationState");
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            //if (m_enablePatience && !IsTargetInRange(100))
            //{
            //    Patience();
            //}
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
           // m_targetInfo.Set(null, null);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.enabled = false;
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = false;
           // m_targetInfo.Set(null, null);
            m_flinchHandle.m_autoFlinch = true;
            m_isDetecting = false;
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

        public void HandleKnockback(float resumeAIDelay)
        {
          //  StopAllCoroutines();
           
            StartCoroutine(KnockbackRoutine(resumeAIDelay));
         
        }

        private IEnumerator KnockbackRoutine(float timer)
        {
            //enabled = false;
            //m_flinchHandle.m_autoFlinch = false;
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_animation.DisableRootMotion();
            m_charcterPhysics.UseStepClimb(false);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
            {
                //m_flinchHandle.enabled = false;
                m_animation.SetAnimation(0, m_info.damageAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.damageAnimation);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            yield return new WaitForSeconds(timer);
            m_charcterPhysics.UseStepClimb(true);
            m_stateHandle.ApplyQueuedState();
            //enabled = true;
            //m_flinchHandle.enabled = true;
            //m_stateHandle.SetState(State.ReevaluateSituation);
        }
    }

    internal interface IEnumator
    {
    }
}


