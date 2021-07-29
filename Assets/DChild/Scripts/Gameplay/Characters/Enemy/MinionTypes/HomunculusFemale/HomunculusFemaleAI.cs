using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Environment;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class HomunculusFemaleAI : CombatAIBrain<HomunculusFemaleAI.Info>, IAmbushingAI
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
#if UNITY_EDITOR
            private const string ATTACKNAME = "LightningSphere";
#endif

            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Dormant State")]
            private string m_dormantAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Dormant State")]
            private string m_awakenAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Dormant State")]
            private string m_fallAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Dormant State")]
            private string m_landAnimation;

            [SerializeField, Range(0, 100), BoxGroup("Idle During Patrol")]
            private int m_chanceToIdleDuringPatrol;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Idle During Patrol")]
            private string m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Idle During Patrol")]
            private string m_flinchAnimation;
            [SerializeField, MinValue(1), BoxGroup("Idle During Patrol")]
            private RangeFloat m_idleDuringPatrolDuration;
            [SerializeField, MinValue(1), BoxGroup("Idle During Patrol")]
            private RangeFloat m_idleDuringPatrolCooldown;

            [SerializeField]
            private MovementInfo m_walkInfo = new MovementInfo();
            [SerializeField]
            private MovementInfo m_lightningArmorWalkInfo = new MovementInfo();

            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_lightningCoreBurstAnimation;
            [SerializeField, BoxGroup(ATTACKNAME)]
            private SimpleAttackInfo m_lightningSphereAttackInfo = new SimpleAttackInfo();
            [SerializeField, MinValue(1), BoxGroup(ATTACKNAME)]
            private int m_maxLightningSphereUse = 1;
            [SerializeField, MinValue(1), BoxGroup(ATTACKNAME)]
            private float m_lightningSphereDuration;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup(ATTACKNAME)]
            private string m_postLightningSphereIdleAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup(ATTACKNAME)]
            private string m_postLightningSphereFlinchAnimation;
            [SerializeField, MinValue(1), BoxGroup(ATTACKNAME)]
            private float m_postLightningSphereIdleDuration;



            public int chanceToIdleDuringPatrol => m_chanceToIdleDuringPatrol;
            public string dormantAnimation => m_dormantAnimation;
            public string awakenAnimation => m_awakenAnimation;
            public string landAnimation => m_landAnimation;
            public string fallAnimation => m_fallAnimation;
            public string idleAnimation => m_idleAnimation;
            public string flinchAnimation => m_flinchAnimation;
            public string lightningCoreBurstAnimation => m_lightningCoreBurstAnimation;
            public SimpleAttackInfo lightningSphereAttackInfo => m_lightningSphereAttackInfo;
            public int maxLightningSphereUse => m_maxLightningSphereUse;
            public string postLightningSphereIdleAnimation => m_postLightningSphereIdleAnimation;
            public string postLightningSphereFlinchAnimation => m_postLightningSphereFlinchAnimation;
            public float postLightningSphereIdleDuration => m_postLightningSphereIdleDuration;
            public float lightningSphereDuration => m_lightningSphereDuration;


            public MovementInfo walkInfo => m_walkInfo;
            public MovementInfo lightningArmorWalkInfo => m_lightningArmorWalkInfo;
            public float GetRandomIdleDuringPatrolCooldown() => m_idleDuringPatrolCooldown.GenerateRandomValue();
            public float GetRandomIdleDuringPatrolDuration() => m_idleDuringPatrolDuration.GenerateRandomValue();

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_coreburstEvent;
            public string coreburstEvent => m_coreburstEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_lightningsphereEvent;
            public string lightningsphereEvent => m_lightningsphereEvent;

            public override void Initialize()
            {
                m_walkInfo.SetData(m_skeletonDataAsset);
                m_lightningArmorWalkInfo.SetData(m_skeletonDataAsset);
                m_lightningSphereAttackInfo.SetData(m_skeletonDataAsset);
            }
        }

        private enum State
        {
            Idle,
            Dormant,
            Detect,
            PatrolIdle,
            Patrol,
            Flinch,
            Attack,
            Chase,
            ReevaluateSituation,
            WaitForBehaviour
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;

        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField, TabGroup("BoundingBox")]
        private Collider2D m_corebustBB;
        [SerializeField, TabGroup("BoundingBox")]
        private Collider2D m_lightningshieldBB;
        [SerializeField, TabGroup("BoundingBox")]
        private Collider2D m_lightningshieldsmallBB;

        [SerializeField, TabGroup("FX")]
        private ParticleFX m_coreburstFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_lightningshieldFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_lightningshieldsmallFX;

        [SerializeField]
        private WayPointPatrol m_patrolHandle;
        [SerializeField]
        private MovementHandle2D m_moveHandle;
        [SerializeField]
        private TransformTurnHandle m_turnHandle;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private bool m_canIdleDuringPatrol;
        private float m_idleDuringPatrolCooldownTimer;
        private float m_idleDuringPatrolDurationTimer;
        private bool m_isInRageMode;
        private int m_availableLightningSphereUse;

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (m_target == null && m_targetInfo.doesTargetExist)
            {

            }
            else
            {
                base.SetTarget(damageable, m_target);
                if (m_target != null)
                {
                    //if (m_isInRageMode == false)
                    //{
                    //    StartCoroutine(LightningCoreBurstRoutine());
                    //}
                    if (m_stateHandle.currentState == State.Dormant)
                    {
                        m_stateHandle.OverrideState(State.Detect);
                    }
                }
            }
        }

        protected override void OnBecomePassive()
        {

        }

        protected override void OnTargetDisappeared()
        {

        }


        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {

        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            m_hitbox.Disable();
            m_animation.EnableRootMotion(true, false);
            LightningShieldDeactivate();
            LightningShieldSmallDeactivate();
            m_coreburstFX.Stop();
            m_corebustBB.enabled = false;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation 
                || m_animation.GetCurrentAnimation(0).ToString() == m_info.postLightningSphereIdleAnimation)
            {
                StopAllCoroutines();
                StartCoroutine(FlinchRoutine());
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            }
        }

        private IEnumerator FlinchRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            var flinchAnim = !m_isInRageMode ? m_info.flinchAnimation : m_info.postLightningSphereFlinchAnimation;
            m_animation.SetAnimation(0, flinchAnim, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, flinchAnim);
            m_animation.SetAnimation(0, !m_isInRageMode ? m_info.idleAnimation : m_info.postLightningSphereIdleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Patrol)
            {
                m_stateHandle.Wait(State.ReevaluateSituation);
            }
            m_turnHandle.Execute();
        }

        private void HandleChanceToIdle(float chance, float deltaTime)
        {
            if (m_canIdleDuringPatrol)
            {
                if (UnityEngine.Random.Range(0, 100) >= chance)
                {
                    m_stateHandle.SetState(State.PatrolIdle);
                    m_idleDuringPatrolDurationTimer = m_info.GetRandomIdleDuringPatrolDuration();
                }
            }
            else
            {
                m_idleDuringPatrolCooldownTimer -= deltaTime;
                if (m_idleDuringPatrolCooldownTimer <= 0)
                {
                    m_canIdleDuringPatrol = true;
                }
            }
        }

        private IEnumerator LightningCoreBurstRoutine()
        {
            m_isInRageMode = true;
            m_availableLightningSphereUse = m_info.maxLightningSphereUse;
            m_moveHandle.Stop();
            ResetIdleChanceData();
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_animation.SetAnimation(0, m_info.lightningCoreBurstAnimation, false);
            //yield return new WaitForSeconds(.5f);
            //CoreBurstEvenTrigger();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningCoreBurstAnimation);
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator LightningSphereRoutine()
        {
            m_availableLightningSphereUse--;
            m_moveHandle.Stop();
            m_stateHandle.Wait(State.Idle);
            m_animation.SetAnimation(0, m_info.lightningSphereAttackInfo.animation, false);
            //yield return new WaitForSeconds(.75f);
            //LightningShieldActivate();
            //yield return new WaitForSeconds(1.75f);
            //LightningShieldDeactivate();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningSphereAttackInfo.animation);

            if (m_availableLightningSphereUse <= 0)
            {
                m_isInRageMode = false;
            }
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator IdleRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (m_isInRageMode)
            {
                m_animation.SetAnimation(0, m_info.postLightningSphereIdleAnimation, true);
            }
            else
            {
                LightningShieldSmallDeactivate();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            yield return new WaitForSeconds(m_info.postLightningSphereIdleDuration);

            if(m_isInRageMode == false)
            {
                m_isInRageMode = true;
                StartCoroutine(LightningCoreBurstRoutine());
            }
            else
            {
                m_stateHandle.ApplyQueuedState();
            }
        }

        private void CoreBurstEvenTrigger()
        {
            StartCoroutine(CoreBurstRoutine());
        }

        private IEnumerator CoreBurstRoutine()
        {
            m_coreburstFX.Play();
            m_corebustBB.enabled = true;
            yield return new WaitForSeconds(.25f);
            m_coreburstFX.Stop();
            m_corebustBB.enabled = false;
            LightningShieldSmallActivate();
            yield return null;
        }

        private void LightningShieldEvent()
        {
            StartCoroutine(LightningShieldRoutine());
        }

        private void LightningShieldActivate()
        {
            m_lightningshieldFX.Play();
            m_lightningshieldBB.enabled = true;
        }

        private void LightningShieldDeactivate()
        {
            m_lightningshieldFX.Stop();
            m_lightningshieldBB.enabled = false;
        }

        private IEnumerator LightningShieldRoutine()
        {
            m_lightningshieldFX.Play();
            m_lightningshieldBB.enabled = true;
            yield return new WaitForSeconds(m_info.lightningSphereDuration);
            m_lightningshieldFX.Stop();
            m_lightningshieldBB.enabled = false;
            yield return null;
        }

        private void LightningShieldSmallEvent()
        {
            StartCoroutine(LightningShieldSmallRoutine());
        }

        private void LightningShieldSmallActivate()
        {
            m_lightningshieldsmallFX.Play();
            m_lightningshieldsmallBB.enabled = true;
        }

        private void LightningShieldSmallDeactivate()
        {
            m_lightningshieldsmallFX.Stop();
            m_lightningshieldsmallBB.enabled = false;
        }

        private IEnumerator LightningShieldSmallRoutine()
        {
            m_lightningshieldsmallFX.Play();
            m_lightningshieldsmallBB.enabled = true;
            yield return new WaitForSeconds(m_info.lightningSphereDuration);
            m_lightningshieldsmallFX.Stop();
            m_lightningshieldsmallBB.enabled = false;
            yield return null;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.EnableRootMotion(true, true);
            m_character.physics.simulateGravity = true;
            m_animation.SetAnimation(0, m_info.awakenAnimation, false);
            //m_animation.AddAnimation(0, m_info.idleAnimation, false, 0)/*.TimeScale = 5f*/;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.awakenAnimation);
            m_animation.DisableRootMotion();
            m_animation.SetAnimation(0, m_info.fallAnimation, true).MixDuration = 0;
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            //yield return new WaitForSeconds(0.5f);
            m_animation.SetAnimation(0, m_info.landAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
            m_hitbox.Enable();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            StartCoroutine(LightningCoreBurstRoutine());
            yield return null;
        }

        private void ResetIdleChanceData()
        {
            m_canIdleDuringPatrol = true;
            m_idleDuringPatrolCooldownTimer = m_info.GetRandomIdleDuringPatrolCooldown();
            m_idleDuringPatrolDurationTimer = m_info.GetRandomIdleDuringPatrolDuration();
        }

        protected override void Start()
        {
            base.Start();
            m_hitbox.Disable();
            m_character.physics.simulateGravity = false;
            m_spineEventListener.Subscribe(m_info.coreburstEvent, CoreBurstEvenTrigger);
            m_spineEventListener.Subscribe(m_info.lightningsphereEvent, LightningShieldEvent);
            //m_spineEventListener.Subscribe(m_info.coreburstEvent, m_coreburstFX.Play);
            //m_spineEventListener.Subscribe(m_info.lightningsphereEvent, m_lightningshieldFX.Play);
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.Initialize();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_stateHandle = new StateHandle<State>(State.Dormant, State.WaitForBehaviour);
            m_turnHandle.TurnDone += OnTurnDone;
            m_canIdleDuringPatrol = true;
            m_idleDuringPatrolCooldownTimer = m_info.GetRandomIdleDuringPatrolCooldown();
            //m_availableLightningSphereUse = m_info.maxLightningSphereUse;
            m_flinchHandle.FlinchStart += OnFlinchStart;
        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(DetectRoutine());
                    break;

                case State.Dormant:
                    m_animation.SetAnimation(0, m_info.dormantAnimation, true);
                    break;

                case State.Patrol:
                    var patrolCharacterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_moveHandle, m_info.walkInfo.speed, patrolCharacterInfo);
                    m_animation.SetAnimation(0, m_info.walkInfo.animation, true);
                    HandleChanceToIdle(m_info.chanceToIdleDuringPatrol, GameplaySystem.time.deltaTime);
                    break;
                case State.PatrolIdle:
                    m_moveHandle.Stop();
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_idleDuringPatrolDurationTimer -= GameplaySystem.time.deltaTime;
                    if (m_idleDuringPatrolDurationTimer <= 0)
                    {
                        m_stateHandle.SetState(State.Patrol);
                        m_idleDuringPatrolCooldownTimer = m_info.GetRandomIdleDuringPatrolCooldown();
                        m_canIdleDuringPatrol = false;
                    }
                    break;
                case State.Flinch:
                    //uf animation is finish 
                    StartCoroutine(LightningSphereRoutine());
                    break;
                case State.Idle:
                    StopAllCoroutines();
                    StartCoroutine(IdleRoutine());
                    break;
                case State.Attack:
                    StopAllCoroutines();
                    if (m_isInRageMode)
                    {
                        StartCoroutine(LightningSphereRoutine());
                    }
                    else
                    {
                        StartCoroutine(LightningCoreBurstRoutine());
                    }
                    break;

                case State.Chase:
                    var toTarget = m_targetInfo.position - (Vector2)m_character.centerMass.position;
                    if (toTarget.magnitude > m_info.lightningSphereAttackInfo.range)
                    {
                        if (Mathf.Sign(toTarget.x) == (int)m_character.facing)
                        {
                            if (!m_wallSensor.isDetecting && m_edgeSensor.isDetecting && m_groundSensor.isDetecting)
                            {
                                m_moveHandle.MoveTowards(toTarget.normalized, m_info.lightningArmorWalkInfo.speed);
                                m_animation.SetAnimation(0, m_info.lightningArmorWalkInfo.animation, true);
                            }
                            else
                            {
                                m_moveHandle.Stop();
                                m_animation.SetAnimation(0, !m_isInRageMode? m_info.idleAnimation : m_info.postLightningSphereIdleAnimation, true);
                            }
                        }
                        else
                        {
                            m_turnHandle.Execute();
                        }
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Attack);
                    }
                    break;

                case State.ReevaluateSituation:
                    if (m_targetInfo.doesTargetExist)
                    {
                        m_stateHandle.SetState(State.Chase);
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Patrol);
                    }
                    break;
                case State.WaitForBehaviour:
                    break;
            }
        }

        public void LaunchAmbush(Vector2 position)
        {
            enabled = true;
            m_stateHandle.OverrideState(State.Detect);
        }

        public void PrepareAmbush(Vector2 position)
        {
            enabled = false;
            StopAllCoroutines();
            m_stateHandle.OverrideState(State.Dormant);
        }
    }

}