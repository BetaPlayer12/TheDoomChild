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

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/PetridBulb")]
    public class PetridBulbAI : CombatAIBrain<PetridBulbAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
            //

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
            

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_spawnProjectileEvent;
            public string spawnProjectileEvent => m_spawnProjectileEvent;

            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_attack.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Idle,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        //Patience Handler
        private float m_currentPatience;
        private bool m_enablePatience;

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;

        private float m_targetDistance;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        //[ShowInInspector]
        //private RandomAttackDecider<Attack> m_attackDecider;

        private ProjectileLauncher m_projectileLauncher;

        [SerializeField]
        private Transform m_projectileStart;

        [SpineBone]
        public string m_boneName;
        [SerializeField]
        private Bone m_bone;

        //protected override void Start()
        //{
        //    base.Start();

        //    m_spineEventListener.Subscribe(m_info.spawnProjectileEvent, LaunchProjectile);
        //    //GameplaySystem.SetBossHealth(m_character);
        //}

        private void LaunchProjectile()
        {
            if (m_targetInfo.isValid)
            {
                var target = m_targetInfo.position; //No Parabola      
                Vector2 spitPos = m_projectileStart.position;
                Vector3 v_diff = (target - spitPos);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);

                //m_stingerPos.rotation = Quaternion.Euler(0f, 0f, postAtan2 * Mathf.Rad2Deg);
                m_projectileLauncher.LaunchProjectile();
                //m_Audiosource.clip = m_RangeAttackClip;
                //m_Audiosource.Play();
            }
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

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
                m_stateHandle.SetState(State.Idle);
            }
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            base.OnDestroyed(sender, eventArgs);
        }

        public override void ApplyData()
        {
            if (m_info != null)
            {
                m_spineEventListener.Unsubcribe(m_info.projectile.launchOnEvent, m_projectileLauncher.LaunchProjectile);
            }
            base.ApplyData();

            if (m_projectileLauncher != null)
            {
                m_projectileLauncher.SetProjectile(m_info.projectile.projectileInfo);
                m_spineEventListener.Subscribe(m_info.projectile.launchOnEvent, m_projectileLauncher.LaunchProjectile);
            }
        }

        void SkeletonAnimation_UpdateLocal(ISkeletonAnimation animated)
        {
            //Debug.Log("FKING AIM");
            if (m_targetInfo.isValid)
            {
                var localPositon = transform.InverseTransformPoint(m_targetInfo.position);
                localPositon = new Vector2(-localPositon.x, localPositon.y);
                m_bone.SetLocalPosition(localPositon);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_attackHandle.AttackDone += OnAttackDone;
            //m_deathHandle.SetAnimation(m_info.deathAnimation);
            //m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectileStart);

            m_bone = m_animation.skeletonAnimation.Skeleton.FindBone(m_boneName);
            m_animation.skeletonAnimation.UpdateLocal += SkeletonAnimation_UpdateLocal;
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.EnableRootMotion(false, false);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    //m_animation.SetEmptyAnimation(0, 0);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.ReevaluateSituation);


                    m_animation.EnableRootMotion(true, false);
                    m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);

                    break;
                case State.Chasing:
                    {
                        if (m_groundSensor.allRaysDetecting)
                        {
                            if (IsTargetInRange(m_info.attack.range))
                            {
                                //StartCoroutine(AttackRoutine());
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            }
                        }
                        //if (IsFacingTarget())
                        //{
                        //    if (!m_wallSensor.isDetecting && m_groundSensor.allRaysDetecting)
                        //    {
                        //        if (IsTargetInRange(m_info.attack.range))
                        //        {
                        //            //StartCoroutine(AttackRoutine());
                        //            m_stateHandle.SetState(State.Attacking);
                        //        }
                        //        else
                        //        {
                        //            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                        //        m_stateHandle.SetState(State.Turning);
                        //}
                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    //if (m_targetInfo.isValid)
                    //{
                    //    m_stateHandle.SetState(State.Chasing);
                    //}
                    m_stateHandle.SetState(State.Chasing);
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_enablePatience)
            {
                Patience();
            }
        }
    }
}
