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
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/QueenBeeAI2")]
    public class QueenBeeAI2 : CombatAIBrain<QueenBeeAI2.Info>
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

        private Transform m_stingerPos;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private ProjectileLauncher m_stingerLauncher;

        [SerializeField]
        private List<Transform> m_spawnPoints;
        [SerializeField]
        private Transform m_tripleDronePoint;

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

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_stateHandle.OverrideState(State.Chasing);
                GameEventMessage.SendEvent("Boss Encounter");
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
            m_agent.Stop();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        public override void ApplyData()
        {
            base.ApplyData();
            if (m_attackDecider != null)
            {
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.GroundSlam, m_info.groundSlam.range),
                                    new AttackInfo<Attack>(Attack.Spit, m_info.spit.range),
                                    new AttackInfo<Attack>(Attack.SkeletonSummon, m_info.skeletonSummon.range)/**/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private void TripleDroneAttackMove()
        {
            //Debug.Log("Move to Attack Position");
            if (Vector2.Distance(transform.position, m_tripleDronePoint.position) < 3f)
            {
                //Debug.Log("Triple Attack!");
                StartCoroutine(TripleBeeDroneShootRoutine());
            }
            else
            {
                //Debug.Log("Move to Destination");
                if (m_tripleDronePoint.position.x > transform.position.x && m_character.facing == HorizontalDirection.Right
                    || m_tripleDronePoint.position.x < transform.position.x && m_character.facing == HorizontalDirection.Left)
                {
                    m_agent.SetDestination(m_tripleDronePoint.position);
                    m_agent.Move(m_info.move.speed);
                }
                else
                {
                    Debug.Log("Mudy TURN");
                    //m_stateHandle.QueueState(State.Attacking);
                    m_stateHandle.Wait(State.Attacking);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    //m_stateHandle.SetState(State.Turning);
                }
            }
        }

        private IEnumerator TripleBeeDroneShootRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            if (!IsFacingTarget())
            {
                //m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
            m_animation.EnableRootMotion(true, false);
            m_attackHandle.ExecuteAttack(m_info.rangeAttack.animation, m_info.idleAnimation);
            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rangeAttack.animation);

            m_stateHandle.ApplyQueuedState();
            //m_stateHandle.SetState(State.ReevaluateSituation);
            yield return null;
        }

        private void LaunchStingerProjectile()
        {
            //if (IsFacingTarget())
            //{
            //    var target = m_targetInfo.position; //No Parabola      
            //    Transform spitPos = m_spawnPoints[UnityEngine.Random.Range(0, 3)];
            //    m_stingerPos = spitPos;
            //    Vector2 spawnPos = new Vector2(spitPos.position.x, spitPos.position.y);
            //    Vector3 v_diff = (target - spawnPos);
            //    float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);

            //    m_stingerLauncher = new ProjectileLauncher(m_info.stingerProjectile.projectileInfo, m_stingerPos);

            //    spitPos.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
            //    GameObject burst = Instantiate(m_info.burstGO, spawnPos, spitPos.rotation);
            //    m_stingerLauncher.LaunchProjectile();
            //}
            //else
            //{
            //    m_stateHandle.OverrideState(State.Turning);
            //}
            if (IsFacingTarget())
            {
                for (int i = 0; i < m_spawnPoints.Count; i++)
                {
                    float rotation = transform.localScale.x < 1 ? 180 : 0;
                    m_spawnPoints[i].localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));
                    GameObject burst = Instantiate(m_info.burstGO, m_spawnPoints[i].position, Quaternion.Euler(new Vector3(0, 0, rotation)));
                    m_stingerLauncher = new ProjectileLauncher(m_info.stingerProjectile.projectileInfo, m_spawnPoints[i]);
                    m_stingerLauncher.LaunchProjectile();
                }
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
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_stingerLauncher = new ProjectileLauncher(m_info.stingerProjectile.projectileInfo, m_spawnPoints[0]);
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            //switch (m_stateHandle.currentState)
            //{
            //    case State.Phasing:
            //        break;

            //    case State.Patrol:
            //        m_animation.EnableRootMotion(false, false);
            //        m_animation.SetAnimation(0, m_info.patrol.animation, true);
            //        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
            //        m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
            //        break;

            //    case State.Turning:
            //        m_stateHandle.Wait(State.ReevaluateSituation);
            //        m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
            //        break;

            //    case State.Attacking:
            //        m_stateHandle.Wait(State.ReevaluateSituation);


            //        switch (m_attackDecider.chosenAttack.attack)
            //        {
            //            case Attack.Bite:
            //                m_animation.EnableRootMotion(true, false);
            //                m_attackHandle.ExecuteAttack(m_info.biteAttack.animation, m_info.idleAnimation);
            //                break;
            //            case Attack.Scratch:
            //                m_animation.EnableRootMotion(true, false);
            //                m_attackHandle.ExecuteAttack(m_info.scratchAttack.animation, m_info.idleAnimation);
            //                break;
            //        }
            //        m_attackDecider.hasDecidedOnAttack = false;

            //        break;
            //    case State.Chasing:
            //        {
            //            if (IsFacingTarget())
            //            {
            //                if (!m_wallSensor.isDetecting && m_groundSensor.allRaysDetecting)
            //                {
            //                    m_attackDecider.DecideOnAttack();
            //                    if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range))
            //                    {
            //                        m_movement.Stop();
            //                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //                        m_stateHandle.SetState(State.Attacking);
            //                    }
            //                    else
            //                    {
            //                        m_animation.EnableRootMotion(false, false);
            //                        m_animation.SetAnimation(0, m_info.move.animation, true);
            //                        m_movement.MoveTowards(m_targetInfo.position, m_info.move.speed * transform.localScale.x);
            //                    }
            //                }
            //                else
            //                {
            //                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //                }
            //            }
            //            else
            //            {
            //                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
            //                    m_stateHandle.SetState(State.Turning);
            //            }
            //        }
            //        break;

            //    case State.ReevaluateSituation:
            //        //How far is target, is it worth it to chase or go back to patrol
            //        if (m_targetInfo.isValid)
            //        {
            //            m_stateHandle.SetState(State.Chasing);
            //        }
            //        else
            //        {
            //            m_stateHandle.SetState(State.Patrol);
            //        }
            //        break;
            //    case State.WaitBehaviourEnd:
            //        return;
            //}
        }

        protected override void OnTargetDisappeared()
        {
            throw new NotImplementedException();
        }
    }
}
