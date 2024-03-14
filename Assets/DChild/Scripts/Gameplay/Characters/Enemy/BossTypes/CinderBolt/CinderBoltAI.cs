using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/CinderBolt")]
    public class CinderBoltAI : CombatAIBrain<CinderBoltAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField, BoxGroup("Movement")]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            [SerializeField, BoxGroup("Movement"), ValueDropdown("GetAnimations")]
            private string m_moveTurnAnimation;
            public string moveTurnAnimation => m_moveTurnAnimation;

            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_hoverUpward = new MovementInfo();
            public MovementInfo hoverUpward => m_hoverUpward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_overchargedHoverUpward = new MovementInfo();
            public MovementInfo overchargedHoverUpward => m_overchargedHoverUpward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_hoverBackward = new MovementInfo();
            public MovementInfo hoverBackward => m_hoverBackward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_overchargedHoverBackward = new MovementInfo();
            public MovementInfo overchargedHoverBackward => m_overchargedHoverBackward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_hoverDownward = new MovementInfo();
            public MovementInfo hoverDownward => m_hoverDownward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_overchargedHoverDownward = new MovementInfo();
            public MovementInfo overchargedHoverDownward => m_overchargedHoverDownward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_hoverForward = new MovementInfo();
            public MovementInfo hoverForward => m_hoverForward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_overchargedHoverForward = new MovementInfo();
            public MovementInfo overchargedHoverForward => m_overchargedHoverForward;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement")]
            private MovementInfo m_longDash = new MovementInfo();
            public MovementInfo longDash => m_longDash;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement")]
            private MovementInfo m_overchargedLongDash = new MovementInfo();
            public MovementInfo overchargedLongDash => m_overchargedLongDash;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement"), ValueDropdown("GetAnimations")]
            private string m_longDashBoosterChargeAnimation;
            public string longDashBoosterChargeAnimation => m_longDashBoosterChargeAnimation;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement"), ValueDropdown("GetAnimations")]
            private string m_overchargedLongDashBoosterChargeAnimation;
            public string overchargedLongDashBoosterChargeAnimation => m_overchargedLongDashBoosterChargeAnimation;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement"), ValueDropdown("GetAnimations")]
            private string m_longDashStopAnimation;
            public string longDashStopAnimation => m_longDashStopAnimation;
            [SerializeField, Title("Short Dash"), BoxGroup("Movement")]
            private MovementInfo m_shortDash = new MovementInfo();
            public MovementInfo shortDash => m_shortDash;

            [SerializeField, Title("Straight Left and Uppercut"), BoxGroup("Attack")]
            private SimpleAttackInfo m_straightLeftAndUppercutAttack = new SimpleAttackInfo();
            public SimpleAttackInfo straightLeftAndUppercutAttack => m_straightLeftAndUppercutAttack;
            [SerializeField, Title("Straight Left and Uppercut"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedStraightLeftAndUppercutAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedStraightLeftAndUppercutAttack => m_overchargedStraightLeftAndUppercutAttack;
            [SerializeField, Title("Flame Thrower"), BoxGroup("Attack")]
            private SimpleAttackInfo m_flameThrowerAttack = new SimpleAttackInfo();
            public SimpleAttackInfo flameThrowerAttack => m_flameThrowerAttack;
            [SerializeField, Title("Flame Thrower"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedFlameThrowerAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedFlameThrowerAttack => m_overchargedFlameThrowerAttack;
            [SerializeField, Title("Flame Beam"), BoxGroup("Attack")]
            private SimpleAttackInfo m_flameBeamAttack = new SimpleAttackInfo();
            public SimpleAttackInfo flameBeamAttack => m_flameBeamAttack;
            [SerializeField, Title("Flame Beam"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedFlameBeamAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedFlameBeamAttack => m_overchargedFlameBeamAttack;
            [SerializeField, Title("Puch"), BoxGroup("Attack")]
            private SimpleAttackInfo m_punchAttack = new SimpleAttackInfo();
            public SimpleAttackInfo punchAttack => m_punchAttack;
            [SerializeField, Title("Puch"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedPunchAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedPunchAttack => m_overchargedPunchAttack;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack")]
            private SimpleAttackInfo m_shotgunBlastFireAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shotgunBlastFireAttack => m_shotgunBlastFireAttack;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedShotgunBlastFireAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedShotgunBlastFireAttack => m_overchargedShotgunBlastFireAttack;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack"), ValueDropdown("GetAnimations")]
            private string m_shotgunBlastBackToIdleAnimation;
            public string shotgunBlastBackToIdleAnimation => m_shotgunBlastBackToIdleAnimation;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack"), ValueDropdown("GetAnimations")]
            private string m_shotgunBlastPreAnimation;
            public string shotgunBlastPreAnimation => m_shotgunBlastPreAnimation;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack")]
            private SimpleAttackInfo m_shotgunBlastRapidFireAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shotgunBlastRapidFireAttack => m_shotgunBlastRapidFireAttack;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedShotgunBlastRapidFireAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedShotgunBlastRapidFireAttack => m_overchargedShotgunBlastRapidFireAttack;
            [SerializeField, Title("Uppercut"), BoxGroup("Attack")]
            private SimpleAttackInfo m_uppercutAttack = new SimpleAttackInfo();
            public SimpleAttackInfo uppercutAttack => m_uppercutAttack;
            [SerializeField, Title("Uppercut"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedUppercutAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedUppercutAttack => m_overchargedUppercutAttack;

            //[SerializeField, TitleGroup("Pattern Ranges")]
            //private float m_targetDistanceTolerance;
            //public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField, TabGroup("Phase 1"), BoxGroup("Pattern Ranges")]
            private float m_phase1Pattern1Range;
            public float phase1Pattern1Range => m_phase1Pattern1Range;
            [SerializeField, TabGroup("Phase 1"), BoxGroup("Pattern Ranges")]
            private float m_phase1Pattern2Range;
            public float phase1Pattern2Range => m_phase1Pattern2Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase2Pattern1Range;
            public float phase2Pattern1Range => m_phase2Pattern1Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase2Pattern2Range;
            public float phase2Pattern2Range => m_phase2Pattern2Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase2Pattern3Range;
            public float phase2Pattern3Range => m_phase2Pattern3Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase2Pattern4Range;
            public float phase2Pattern4Range => m_phase2Pattern4Range;


            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedFlinchAnimation;
            public string overchargedFlinchAnimation => m_overchargedFlinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_hydraulicsAnimation;
            public string hydraulicsAnimation => m_hydraulicsAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_malfunctionStateAnimation;
            public string malfunctionStateAnimation => m_malfunctionStateAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_malfunctionStateIdleAnimation;
            public string malfunctionStateIdleAnimation => m_malfunctionStateIdleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_malfunctionRecoveryStateAnimation;
            public string malfunctionRecoveryStateAnimation => m_malfunctionRecoveryStateAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_spinAnimation;
            public string spinAnimation => m_spinAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_spinDropAnimation;
            public string spinDropAnimation => m_spinDropAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_spinPreAnimation;
            public string spinPreAnimation => m_spinPreAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_spinEndAnimation;
            public string spinEndAnimation => m_spinEndAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedSpinAnimation;
            public string overchargedSpinAnimation => m_overchargedSpinAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedSpinDropAnimation;
            public string overchargedSpinDropAnimation => m_overchargedSpinDropAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedSpinPreAnimation;
            public string overchargedSpinPreAnimation => m_overchargedSpinPreAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedSpinEndAnimation;
            public string overchargedSpinEndAnimation => m_overchargedSpinEndAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;

            //[Title("Summoned Assets")]
            //[SerializeField]
            //private GameObject m_skeletalArm;
            //public GameObject skeletalArm => m_skeletalArm;
            //[SerializeField]
            //private GameObject m_totem;
            //public GameObject totem => m_totem;
            //[SerializeField]
            //private GameObject m_curseObject;
            //public GameObject curseObject => m_curseObject;
            //[SerializeField]
            //private GameObject m_summonedMinion;
            //public GameObject summonedMinion => m_summonedMinion;
            //[SerializeField]
            //private GameObject m_summonedZombie;
            //public GameObject summonedZombie => m_summonedZombie;
            //[SerializeField]
            //private GameObject m_summonedZombie2;
            //public GameObject summonedZombie2 => m_summonedZombie2;
            //[SerializeField]
            //private GameObject m_summonedZombie3;
            //public GameObject summonedZombie3 => m_summonedZombie3;
            //[SerializeField]
            //private GameObject m_spike;
            //public GameObject spike => m_spike;

            [Title("Projectiles")]
            
            //[SerializeField]
            //private SimpleProjectileAttackInfo m_ghostOrbProjectile;
            //public SimpleProjectileAttackInfo ghostOrbProjectile => m_ghostOrbProjectile;

            [Title("Events")]
            //[SerializeField, ValueDropdown("GetEvents")]
            //private string m_mapCurseEvent;
            //public string mapCurseEvent => m_mapCurseEvent;
            //[SerializeField, ValueDropdown("GetEvents")]
            //private string m_summonTotemEvent;
            //public string summonTotemEvent => m_summonTotemEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_hoverUpward.SetData(m_skeletonDataAsset);
                m_overchargedHoverUpward.SetData(m_skeletonDataAsset);
                m_hoverBackward.SetData(m_skeletonDataAsset);
                m_overchargedHoverBackward.SetData(m_skeletonDataAsset);
                m_hoverDownward.SetData(m_skeletonDataAsset);
                m_overchargedHoverDownward.SetData(m_skeletonDataAsset);
                m_hoverForward.SetData(m_skeletonDataAsset);
                m_overchargedHoverForward.SetData(m_skeletonDataAsset);
                m_longDash.SetData(m_skeletonDataAsset);
                m_overchargedLongDash.SetData(m_skeletonDataAsset);
                m_shortDash.SetData(m_skeletonDataAsset);
                m_straightLeftAndUppercutAttack.SetData(m_skeletonDataAsset);
                m_overchargedStraightLeftAndUppercutAttack.SetData(m_skeletonDataAsset);
                m_flameThrowerAttack.SetData(m_skeletonDataAsset);
                m_overchargedFlameThrowerAttack.SetData(m_skeletonDataAsset);
                m_flameBeamAttack.SetData(m_skeletonDataAsset);
                m_overchargedFlameBeamAttack.SetData(m_skeletonDataAsset);
                m_punchAttack.SetData(m_skeletonDataAsset);
                m_overchargedPunchAttack.SetData(m_skeletonDataAsset);
                m_shotgunBlastFireAttack.SetData(m_skeletonDataAsset);
                m_overchargedShotgunBlastFireAttack.SetData(m_skeletonDataAsset);
                m_shotgunBlastRapidFireAttack.SetData(m_skeletonDataAsset);
                m_overchargedShotgunBlastRapidFireAttack.SetData(m_skeletonDataAsset);
                m_uppercutAttack.SetData(m_skeletonDataAsset);
                m_overchargedUppercutAttack.SetData(m_skeletonDataAsset);
                //m_ghostOrbProjectile.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private int m_attackCount;
            public int attackCount => m_attackCount;
            //[SerializeField]
            //private List<int> m_patternAttackCount;
            //public List<int> patternAttackCount => m_patternAttackCount;
        }


        private enum State
        {
            Phasing,
            Intro,
            Idle,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Phase1Pattern1,
            Phase1Pattern2,
            Phase2Pattern1,
            Phase2Pattern2,
            Phase2Pattern3,
            Phase2Pattern4,
            WaitAttackEnd,
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            Wait,
        }

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;

        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private Health m_health;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_ghostOrbStartFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_mapCurseFX;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Spawn Points")]
        private List<Transform> m_projectilePoints;
        [SerializeField, TabGroup("Spawn Points")]
        private List<Transform> m_skeletalArmPoints;
        [SerializeField, TabGroup("Spawn Points")]
        private List<Transform> m_lichLordPortPoints;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_playerP3Point;
        [SerializeField, TabGroup("Spawn Points")]
        private GameObject m_flamethrower1SpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private GameObject m_flamethrower2SpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private GameObject m_rightHandSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private GameObject m_leftHandSpawnPoint;


        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        private bool m_phase1Done;
        private bool m_phase2Done;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;
        private ProjectileLauncher m_projectileLauncher;

        //[SerializeField, TabGroup("Spawn Points")]
        //private Transform m_projectilePoint;

        private int m_hitCount;
        private bool m_hasPhaseChanged;
        private PhaseInfo m_phaseInfo;
        //private Vector3 m_totemLastPos;
        private Vector3 m_minionLastPos;
        private Vector3 m_zombieLastPos;
        private List<GameObject> m_minionsCache;
        private List<GameObject> m_zombiesCache;
        private List<GameObject> m_sarcophagusCache;
        private List<GameObject> m_spikeCache;

        private Coroutine m_changeLocationRoutine;
        private bool m_isDetecting;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_phaseInfo = obj;
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    if (!m_phase1Done && !m_phase2Done )
                    {
                        m_attackCache.Clear();
                        m_attackRangeCache.Clear();
                        //if (m_patternCooldown.Count != 0)
                        //    m_patternCooldown.Clear();

                        m_phase1Done = true;
                        //m_canUpdateStats = true;
                        //m_animation.SetAnimation(10, m_info.phase1MixAnimation, false);
                        AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2);
                        AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range);
                        //for (int i = 0; i < m_info.phase1PatternCooldown.Count; i++)
                        //    m_patternCooldown.Add(m_info.phase1PatternCooldown[i]);
                        //m_airProjectileInfo = m_info.airProjectile.projectileInfo;
                        //m_ballisticProjectileInfo = m_info.ballisticProjectile.projectileInfo;
                    }
                    break;
                case Phase.PhaseTwo:
                    if (m_phase1Done && !m_phase2Done)
                    {
                        m_attackCache.Clear();
                        m_attackRangeCache.Clear();
                        //if (m_patternCooldown.Count != 0)
                        //    m_patternCooldown.Clear();

                        m_phase2Done = true;
                        //m_canUpdateStats = true;
                        //m_animation.SetAnimation(10, m_info.phase2MixAnimation, false);
                        AddToAttackCache(Attack.Phase2Pattern1, Attack.Phase2Pattern2, Attack.Phase2Pattern3, Attack.Phase2Pattern4);
                        AddToRangeCache(m_info.phase2Pattern1Range, m_info.phase2Pattern2Range, m_info.phase2Pattern3Range, m_info.phase2Pattern4Range);
                        //for (int i = 0; i < m_info.phase2PatternCooldown.Count; i++)
                        //    m_patternCooldown.Add(m_info.phase2PatternCooldown[i]);
                        //m_airProjectileInfo = m_info.airPhase2Projectile.projectileInfo;
                        //m_ballisticProjectileInfo = m_info.ballisticPhase2Projectile.projectileInfo;
                    }
                    break;
            }
        }

        private void ChangeState()
        {
            if (!m_hasPhaseChanged && m_changeLocationRoutine == null)
            {
                m_hitbox.SetInvulnerability(Invulnerability.Level_1);
                StopAllCoroutines();
                m_stateHandle.OverrideState(State.Phasing);
                m_hasPhaseChanged = true;
                m_phaseHandle.ApplyChange();
                m_animation.DisableRootMotion();
                m_animation.SetEmptyAnimation(0, 0);
            }
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing)
            {
                m_stateHandle.OverrideState(State.Turning);
            }
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            Debug.Log("CinderBolt detected the Player");
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                if (!m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //GameEventMessage.SendEvent("Boss Encounter");
                }
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing)
            {
                m_hitbox.gameObject.SetActive(true);
                m_animation.animationState.TimeScale = 1f;
                m_stateHandle.ApplyQueuedState();
            }
        }

        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            CustomTurn();
            m_animation.SetAnimation(0, m_info.idleAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            enabled = false;
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_hasPhaseChanged = false;
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_hitbox.SetInvulnerability(Invulnerability.None);
                    m_stateHandle.ApplyQueuedState();
                    break;
                case Phase.PhaseTwo:
                    m_hitbox.SetInvulnerability(Invulnerability.None);
                    m_stateHandle.ApplyQueuedState();
                    break;
            }
            //if (m_phaseHandle.currentPhase == Phase.PhaseThree)
            //{
            //}
            //else
            //{
            //    m_hitbox.SetInvulnerability(Invulnerability.None);
            //    m_stateHandle.ApplyQueuedState();
            //}
            yield return null;
            enabled = true;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_agent.Stop();
            m_isDetecting = false;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            //StopAllCoroutines();
            //m_attackDecider.hasDecidedOnAttack = false;
            //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            m_hitCount++;
            if (m_hitCount == 5 && m_phaseHandle.currentPhase == Phase.PhaseOne)
            {
                StopAllCoroutines();
                m_hitCount = 0;
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            //m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        #region Attacks
        #endregion

        #region Movement
        private IEnumerator ExecuteMove(Vector2 target, float attackRange, /*float heightOffset,*/ Attack attack)
        {
            m_animation.DisableRootMotion();
            bool inRange = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            while (!inRange)
            {

                bool xTargetInRange = Mathf.Abs(target.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(target.y - transform.position.y) < 1 ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    inRange = true;
                }
                //Debug.Log("Facing Target " + IsFacingTarget());
                DynamicMovement(new Vector2(m_targetInfo.position.x, target.y));
                yield return null;
            }
            m_agent.Stop();

            ExecuteAttack(attack);
            yield return null;
        }

        private void DynamicMovement(Vector2 target)
        {
            if (IsFacingTarget())
            {
                var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
                var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;
                //Debug.Log("Read Dynamic Movements " + velocityX + " " + velocityY);
                m_agent.SetDestination(target);
                m_agent.Move(m_info.move.speed);

                m_animation.SetAnimation(0, m_info.move.animation, true);
            }
            else
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation && GetComponent<IsolatedPhysics2D>().velocity.y <= 0 && m_stateHandle.currentState != State.Phasing)
                {
                    m_turnState = State.Attacking;
                    m_stateHandle.OverrideState(State.Turning);
                }
            }
        }
        #endregion

        private bool AllowAttack(Phase phase, State state)
        {
            if (m_phaseHandle.currentPhase >= phase)
            {
                return true;
            }
            else
            {
                DecidedOnAttack(false);
                m_stateHandle.OverrideState(state);
                return false;
            }
        }

        private void DecidedOnAttack(bool condition)
        {
            m_attackDecider.hasDecidedOnAttack = condition;
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Phase1Pattern1, m_info.targetDistanceTolerance),
                                     new AttackInfo<Attack>(Attack.Phase1Pattern2, m_info.targetDistanceTolerance),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern1, m_info.targetDistanceTolerance),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern2, m_info.targetDistanceTolerance),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern3, m_info.targetDistanceTolerance),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern4, m_info.targetDistanceTolerance));
            DecidedOnAttack(false);
        }

        public override void ApplyData()
        {
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }

        private void ExecuteAttack(Attack m_attack)
        {
            switch (m_attack)
            {
                case Attack.Phase1Pattern1:
                    //StartCoroutine(GhostOrbAttackRoutine());
                    break;
                case Attack.Phase1Pattern2:
                    //StartCoroutine(Attack2Routine());
                    break;
                case Attack.Phase2Pattern1:
                    //StartCoroutine(Attack2Routine());
                    break;
                case Attack.Phase2Pattern2:
                    //StartCoroutine(Attack2Routine());
                    break;
                case Attack.Phase2Pattern3:
                    //StartCoroutine(Attack2Routine());
                    break;
                case Attack.Phase2Pattern4:
                    //StartCoroutine(Attack2Routine());
                    break;
            }
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

        protected override void Awake()
        {
            base.Awake();
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            //m_projectileLauncher = new ProjectileLauncher(m_info.ghostOrbProjectile.projectileInfo, m_projectilePoint);
            m_attackDecider = new RandomAttackDecider<Attack>();

            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();

            m_minionsCache = new List<GameObject>();
            m_zombiesCache = new List<GameObject>();
            m_sarcophagusCache = new List<GameObject>();
            for (int i = 0; i < 4; i++)
            {
                m_sarcophagusCache.Add(null);
            }
            m_spikeCache = new List<GameObject>();
            m_attackCache = new List<Attack>();
            //m_projectilePoints = new List<Transform>();
            //m_skeletalArmPoints = new List<Transform>();
            AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2, Attack.Phase2Pattern1, Attack.Phase2Pattern2, Attack.Phase2Pattern3, Attack.Phase2Pattern4);
            m_attackUsed = new bool[m_attackCache.Count];
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range, m_info.phase2Pattern1Range, m_info.phase2Pattern2Range, m_info.phase2Pattern3Range, m_info.phase2Pattern4Range);
            m_attackUsed = new bool[m_attackCache.Count];
        }

        protected override void Start()
        {
            base.Start();
            //m_spineListener.Subscribe(m_info.ghostOrbProjectile.launchOnEvent, LaunchOrb);
            //m_spineListener.Subscribe(m_info.mapCurseEvent, MapCurse);
            //m_spineListener.Subscribe(m_info.summonTotemEvent, SummonTotemObject);
            m_animation.DisableRootMotion();

            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();

            //TESTING
            //m_projectilePoint.SetParent(null);
        }

        private void Update()
        {
            if (!m_hasPhaseChanged && m_stateHandle.currentState != State.Phasing)
            {
                m_phaseHandle.MonitorPhase();
            }
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Intro:
                    StartCoroutine(IntroRoutine());
                    break;
                case State.Phasing:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_agent.Stop();
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    //m_animation.animationState.GetCurrent(0).MixDuration = 1;
                    //m_movement.Stop();
                    break;
                case State.Attacking:
                    //m_stateHandle.Wait(State.Attacking);
                    if (IsTargetInRange(m_info.targetDistanceTolerance))
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        m_agent.Stop();
                        switch (m_currentAttack)
                        {
                            case Attack.Phase1Pattern1:
                                //switch (m_phaseHandle.currentPhase)
                                //{
                                //    case Phase.PhaseOne:
                                //        ExecuteAttack(Attack.GhostOrb);
                                //        break;
                                //    case Phase.PhaseTwo:
                                //        for (int i = 0; i < m_sarcophagusCache.Count; i++)
                                //        {
                                //            if (m_sarcophagusCache[i] == null)
                                //            {
                                //                if (m_minionsCache.Count == 0)
                                //                {
                                //                    StopAllCoroutines();
                                //                    ExecuteAttack(Attack.SummonTotem);
                                //                }
                                //                return;
                                //            }
                                //        }
                                //        m_stateHandle.OverrideState(State.ReevaluateSituation);
                                //        break;
                                //}
                                ///////
                                //m_stateHandle.OverrideState(State.ReevaluateSituation);
                                break;
                            case Attack.Phase1Pattern2:
                                //switch (m_phaseHandle.currentPhase)
                                //{
                                //    case Phase.PhaseOne:
                                //        ExecuteAttack(Attack.SkeletalArm);
                                //        break;
                                //    case Phase.PhaseTwo:
                                //        if (m_minionsCache.Count == 0)
                                //        {
                                //            ExecuteAttack(Attack.GhostOrb);
                                //        }
                                //        else
                                //        {
                                //            m_stateHandle.OverrideState(State.ReevaluateSituation);
                                //        }
                                //        break;
                                //}
                                ///////
                                //m_stateHandle.OverrideState(State.ReevaluateSituation);
                                break;
                            case Attack.Phase2Pattern1:
                                //switch (m_phaseHandle.currentPhase)
                                //{
                                //    case Phase.PhaseOne:
                                //        ExecuteAttack(Attack.SkeletalArm);
                                //        break;
                                //    case Phase.PhaseTwo:
                                //        if (m_minionsCache.Count == 0)
                                //        {
                                //            ExecuteAttack(Attack.GhostOrb);
                                //        }
                                //        else
                                //        {
                                //            m_stateHandle.OverrideState(State.ReevaluateSituation);
                                //        }
                                //        break;
                                //}
                                ///////
                                //m_stateHandle.OverrideState(State.ReevaluateSituation);
                                break;
                            case Attack.Phase2Pattern2:
                                //switch (m_phaseHandle.currentPhase)
                                //{
                                //    case Phase.PhaseOne:
                                //        ExecuteAttack(Attack.SkeletalArm);
                                //        break;
                                //    case Phase.PhaseTwo:
                                //        if (m_minionsCache.Count == 0)
                                //        {
                                //            ExecuteAttack(Attack.GhostOrb);
                                //        }
                                //        else
                                //        {
                                //            m_stateHandle.OverrideState(State.ReevaluateSituation);
                                //        }
                                //        break;
                                //}
                                ///////
                                //m_stateHandle.OverrideState(State.ReevaluateSituation);
                                break;
                            case Attack.Phase2Pattern3:
                                //switch (m_phaseHandle.currentPhase)
                                //{
                                //    case Phase.PhaseOne:
                                //        ExecuteAttack(Attack.SkeletalArm);
                                //        break;
                                //    case Phase.PhaseTwo:
                                //        if (m_minionsCache.Count == 0)
                                //        {
                                //            ExecuteAttack(Attack.GhostOrb);
                                //        }
                                //        else
                                //        {
                                //            m_stateHandle.OverrideState(State.ReevaluateSituation);
                                //        }
                                //        break;
                                //}
                                ///////
                                //m_stateHandle.OverrideState(State.ReevaluateSituation);
                                break;
                            case Attack.Phase2Pattern4:
                                //switch (m_phaseHandle.currentPhase)
                                //{
                                //    case Phase.PhaseOne:
                                //        ExecuteAttack(Attack.SkeletalArm);
                                //        break;
                                //    case Phase.PhaseTwo:
                                //        if (m_minionsCache.Count == 0)
                                //        {
                                //            ExecuteAttack(Attack.GhostOrb);
                                //        }
                                //        else
                                //        {
                                //            m_stateHandle.OverrideState(State.ReevaluateSituation);
                                //        }
                                //        break;
                                //}
                                ///////
                                //m_stateHandle.OverrideState(State.ReevaluateSituation);
                                break;
                        }
                    }
                    else
                    {
                        DynamicMovement(m_targetInfo.position);
                    }
                    break;

                case State.Chasing:
                    m_hitbox.SetInvulnerability(Invulnerability.None);
                    if (IsFacingTarget())
                    {
                        DecidedOnAttack(false);
                        ChooseAttack();
                        if (m_attackDecider.hasDecidedOnAttack)
                        {
                            m_stateHandle.OverrideState(State.Attacking);
                        }
                    }
                    else
                    {
                        m_turnState = State.Chasing;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation /*&& m_animation.GetCurrentAnimation(0).ToString() != m_info.attackDaggersIdle.animation*/)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.ReevaluateSituation:
                    m_stateHandle.SetState(State.Chasing);
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
        {
            //m_stickToGround = false;
            //m_currentCD = 0;
        }

        public override void ReturnToSpawnPoint()
        {
        }

        protected override void OnForbidFromAttackTarget()
        {
        }
    }
}