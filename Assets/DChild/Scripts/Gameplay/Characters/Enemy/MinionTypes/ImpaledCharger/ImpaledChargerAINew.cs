using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    /* This is an AI that manages 2 more AIs for ImpaledCharger to Work as designed
     * This should not have its own Animation and Damageable because it uses the Other AI's References
     * 
     * 
     */

    public class ImpaledChargerAINew : CombatAIBrain<ImpaledChargerAINew.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, BoxGroup()]
            private PusherInfo m_pusherInfo;
            [SerializeField, BoxGroup()]
            private ImpaledOneInfo m_impaledOne;

            public PusherInfo pusherInfo => m_pusherInfo;
            public ImpaledOneInfo impaledOne => m_impaledOne;

            public override void Initialize()
            {
                m_pusherInfo.Initialize();
                m_impaledOne.Initialize();
            }
        }

        [System.Serializable]
        public class PusherInfo : BaseInfo
        {
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;

            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;

            public override void Initialize()
            {
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
            }
        }

        [System.Serializable]
        public class ImpaledOneInfo : BaseInfo
        {
            [SerializeField]
            private MovementInfo m_moveForwardInfo;
            [SerializeField]
            private MovementInfo m_moveBackwardInfo;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;

            public override void Initialize()
            {

                m_idleAnimation.SetData(m_skeletonDataAsset);
            }
        }

        private enum State
        {
            Idle,
            Attack,
            ReevaluateSituation,
            WaitForBehaviour
        }

        [SerializeField]
        private ImpaledCharger_ImpaledOneAI m_impaledOne;
        [SerializeField]
        private ImpaledCharger_PusherAI m_pusher;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        public override void ReturnToSpawnPoint()
        {

        }

        protected override void OnTargetDisappeared()
        {
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            base.SetTarget(damageable, m_target);
            if (m_stateHandle.currentState == State.Idle)
            {
                StopAllCoroutines();
                StartCoroutine(DetectTargetRoutine());
            }
        }

        private IEnumerator DetectTargetRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_pusher.TurnTowards(m_targetInfo.position);
            var detectTrack = m_pusher.SetAnimation(0, m_info.pusherInfo.detectAnimation, false);
            yield return new WaitForSpineAnimationComplete(detectTrack);
            m_pusher.SetAnimation(0, m_info.pusherInfo.idleAnimation, true);
            m_pusher.TurnTowards(m_impaledOne.position);
            m_stateHandle.ApplyQueuedState();
        }

        #region OnDeath Reactions
        private void OnImpaledOneDeath(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            RemoveChildAIsFromSelf();
            m_pusher.SetMode(ImpaledCharger_PusherAI.Mode.Alone);
            m_pusher.SetTarget(m_targetInfo.transform.GetComponent<Damageable>(), m_targetInfo.transform.GetComponent<Character>());
            m_pusher.BeAnnoyed();
            Destroy(gameObject);
        }

        private void OnPusherDeath(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            RemoveChildAIsFromSelf();
            m_impaledOne.SetTarget(m_targetInfo.transform.GetComponent<Damageable>(), m_targetInfo.transform.GetComponent<Character>());
            m_impaledOne.SetMode(ImpaledCharger_ImpaledOneAI.Mode.Alone);
            Destroy(gameObject);
        }

        private void RemoveChildAIsFromSelf()
        {
            m_impaledOne.transform.parent = null;
            m_impaledOne.GetComponent<Damageable>().Destroyed -= OnImpaledOneDeath;
            m_pusher.transform.parent = null;
            m_pusher.GetComponent<Damageable>().Destroyed -= OnPusherDeath;
        }
        #endregion

        private IEnumerator DelayedStartRoutine()
        {
            yield return null;
            m_impaledOne.SetMode(ImpaledCharger_ImpaledOneAI.Mode.Manipulated);
            m_pusher.SetMode(ImpaledCharger_PusherAI.Mode.Manipulated);
        }

        protected override void Awake()
        {
            //base.Awake();
            m_impaledOne.GetComponent<Damageable>().Destroyed += OnImpaledOneDeath;
            m_pusher.GetComponent<Damageable>().Destroyed += OnPusherDeath;
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitForBehaviour);
        }

        protected override void Start()
        {
            StartCoroutine(DelayedStartRoutine());//Ensure that the other members are initialized first;
        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_pusher.SetAnimation(0, m_info.pusherInfo.idleAnimation, true);
                    break;
                case State.Attack:
                    break;
                case State.ReevaluateSituation:
                    break;
                case State.WaitForBehaviour:
                    break;
            }
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}
