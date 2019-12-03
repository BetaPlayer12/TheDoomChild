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
using Doozy.Engine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class QueenBeeAI : CombatAIBrain<QueenBeeAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_patrol = new MovementInfo();
            public MovementInfo patrol => m_patrol;
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            //Attack Behaviours
            [Title("Attack Behaviours")]
            [SerializeField]
            private SimpleAttackInfo m_rangeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo rangeAttack => m_rangeAttack;
            [SerializeField]
            private SimpleAttackInfo m_groundSlam = new SimpleAttackInfo();
            public SimpleAttackInfo groundSlam => m_groundSlam;
            [SerializeField]
            private SimpleAttackInfo m_spit = new SimpleAttackInfo();
            public SimpleAttackInfo spit => m_spit;
            [SerializeField]
            private SimpleAttackInfo m_skeletonSummon = new SimpleAttackInfo();
            public SimpleAttackInfo skeletonSummon => m_skeletonSummon;
            //

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;

            [SerializeField]
            private SimpleProjectileAttackInfo m_stingerProjectile;
            public SimpleProjectileAttackInfo stingerProjectile => m_stingerProjectile;
            [SerializeField]
            private GameObject m_burstGO;
            public GameObject burstGO => m_burstGO;


            [SerializeField]
            private float m_delayShotTime;
            public float delayShotTimer => m_delayShotTime;




            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_rangeAttack.SetData(m_skeletonDataAsset);
                m_stingerProjectile.SetData(m_skeletonDataAsset);
                //
                m_groundSlam.SetData(m_skeletonDataAsset);
                m_spit.SetData(m_skeletonDataAsset);
                m_skeletonSummon.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private int m_tombVolley;
            public int tombVolley => m_tombVolley;
            [SerializeField]
            private int m_tombSize;
            public int tombSize => m_tombSize;
            [SerializeField]
            private int m_skeletonNum;
            public int skeletonNum => m_skeletonNum;
            [SerializeField, ValueDropdown("GetSkins")]
            private string m_skin;
            public string skin => m_skin;
            [SerializeField]
            private int m_phaseIndex;
            public int phaseIndex => m_phaseIndex;

            [SerializeField, PreviewField]
            protected SkeletonDataAsset m_skeletonDataAsset;

            protected IEnumerable GetSkins()
            {
                ValueDropdownList<string> list = new ValueDropdownList<string>();
                var reference = m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Skins.ToArray();
                for (int i = 0; i < reference.Length; i++)
                {
                    list.Add(reference[i].Name);
                }
                return list;
            }
        }


        private enum State
        {
            Phasing,
            Idle,
            Patrol,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            GroundSlam,
            Spit,
            SkeletonSummon,
            WaitAttackEnd,
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            PhaseThree,
            Wait,
        }

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        //Patience Handler
        [SerializeField]
        private SpineEventListener m_spineListener;
        [SerializeField]
        private Transform m_stingerPos;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private ProjectileLauncher m_stingerLauncher;


        //stored timer
        private float postAtan2;

        //
        private float timeCounter;

        // player position
        private Vector2 playerPosition;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            Debug.Log("Change Phase");
            //m_currentTombVolleys = obj.tombVolley;
            //m_currentTombSize = obj.tombSize;
            //m_currentSkeletonSize = obj.skeletonNum;
            //m_currentSkin = obj.skin;
            //m_currentPhaseIndex = obj.phaseIndex;
        }

        private void ChangeState()
        {
            StopAllCoroutines();
            m_stateHandle.OverrideState(State.Phasing);
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                GameEventMessage.SendEvent("Boss Encounter");
            }
        }

        public override void ApplyData()
        {
            base.ApplyData();
            if (m_attackDecider != null)
            {
                //Debug.Log("Update attack list trigger function");
                UpdateAttackDeciderList();
            }
            if (m_info != null)
            {
                m_spineListener.Unsubcribe(m_info.stingerProjectile.launchOnEvent, m_stingerLauncher.LaunchProjectile);
            }
            if (m_stingerLauncher != null)
            {
                m_stingerLauncher.SetProjectile(m_info.stingerProjectile.projectileInfo);
                m_spineListener.Subscribe(m_info.stingerProjectile.launchOnEvent, m_stingerLauncher.LaunchProjectile);
            }
        }

        private void UpdateAttackDeciderList()
        {
            //Debug.Log("Update attack list trigger");
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.GroundSlam, m_info.groundSlam.range),
                                    new AttackInfo<Attack>(Attack.SkeletonSummon, m_info.skeletonSummon.range),
                                    new AttackInfo<Attack>(Attack.Spit, m_info.spit.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator RangeAttackRoutine()
        {
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.rangeAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rangeAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_RangeBeeDroneDeadClip;
            //m_Audiosource.Play();

            base.OnDestroyed(sender, eventArgs);
            m_agent.Stop();
        }

        // benjo's alteration
        public Vector2 GetPlayerTransform()
        {
            var m_playerTransform = m_targetInfo.position;
            return m_playerTransform;
        }

        public float GetProjectileSpeed()
        {
            var m_bulletSpeed = m_info.stingerProjectile.projectileInfo.speed;
            return m_bulletSpeed;
        }

        private void LaunchStingerProjectile()
        {
            if (IsFacingTarget())
            {
                var target = m_targetInfo.position; //No Parabola      
                Vector2 spitPos = m_stingerPos.position;
                Vector3 v_diff = (target - spitPos);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                if (m_info.delayShotTimer <= timeCounter)
                {
                    postAtan2 = atan2;
                    timeCounter = 0;
                    Debug.Log("update Check");
                }

                m_stingerPos.rotation = Quaternion.Euler(0f, 0f, postAtan2 * Mathf.Rad2Deg);
                GameObject burst = Instantiate(m_info.burstGO, spitPos, m_stingerPos.rotation);
                m_stingerLauncher.LaunchProjectile();
                //m_Audiosource.clip = m_RangeAttackClip;
                //m_Audiosource.Play();
            }
            else
            {
                m_stateHandle.OverrideState(State.Turning);
            }
        }

        protected override void Awake()
        {

            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_stingerLauncher = new ProjectileLauncher(m_info.stingerProjectile.projectileInfo, m_stingerPos);
        }

        protected override void Start()
        {
            base.Start();
            timeCounter = m_info.delayShotTimer + 1;
            m_spineListener.Subscribe(m_info.stingerProjectile.launchOnEvent, LaunchStingerProjectile);
        }

        private void Update()
        {
            timeCounter += 1 * Time.deltaTime;

            switch (m_stateHandle.currentState)
            {
                case State.Phasing:
                    break;
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (m_targetInfo.isValid == false)
                    {
                        m_stateHandle.SetState(State.Patrol);
                    }
                    break;

                case State.Patrol:

                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);

                    break;

                case State.Turning:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    m_agent.Stop();

                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.ReevaluateSituation);


                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.GroundSlam:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.groundSlam.animation, m_info.idleAnimation);
                            break;
                        case Attack.Spit:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.spit.animation, m_info.idleAnimation);
                            break;
                        case Attack.SkeletonSummon:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.skeletonSummon.animation, m_info.idleAnimation);
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;
                case State.Chasing:

                    if (IsFacingTarget())
                    {

                        if (IsTargetInRange(m_info.stingerProjectile.range))
                        {
                            m_stateHandle.SetState(State.Attacking);
                        }
                        else
                        {

                            var target = m_targetInfo.position;
                            //target.y -= 0.5f;
                            m_animation.DisableRootMotion();
                            if (m_character.physics.velocity != Vector2.zero)
                            {
                                m_animation.SetAnimation(0, m_info.move.animation, true);
                            }
                            else
                            {
                                m_animation.SetAnimation(0, m_info.patrol.animation, true);
                            }
                            m_agent.SetDestination(target);

                            m_agent.Move(m_info.move.speed);

                        }

                    }
                    else
                    {
                        m_stateHandle.SetState(State.Turning);
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
                        m_stateHandle.SetState(State.Patrol);
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }


    }
}
