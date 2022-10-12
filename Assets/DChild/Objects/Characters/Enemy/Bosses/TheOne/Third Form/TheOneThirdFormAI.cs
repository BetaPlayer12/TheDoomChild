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
using Spine.Unity.Modules;
using Spine.Unity.Examples;
using DChild.Gameplay.Pooling;
using UnityEngine.Playables;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/TheOneThirdForm")]
    public class TheOneThirdFormAI : CombatAIBrain<TheOneThirdFormAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //[TitleGroup("Phase Info")]

            //[SerializeField]
            //private PhaseInfo<Phase> m_phaseInfo;
            //public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            public override void Initialize()
            {
                
            }

            
        }

        protected override void Awake()
        {
            //base.Awake();

        }

        protected override void Start()
        {
            //base.Start();
            m_tentacleStabTimerValue = m_tentacleStabTimer;
        }

        protected override void LateUpdate()
        {
            //base.LateUpdate();
        }

        public override void Enable()
        {
            //temp solution
        } 

        public override void Disable()
        {
            //temp solution
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            PhaseThree,
            PhaseFour,
            PhaseFive,
            Wait,
        }

        //private enum State
        //{
        //    Intro, 
        //    Phasing,
        //    Attacking,
        //    Idle,
        //    ReevaluateSituation,
        //    WaitBehaviourEnd,
        //}

        [SerializeField, BoxGroup("The One Third Form Attacks")]
        private TentacleGroundStabAttack m_tentacleStabAttack;
        [SerializeField, BoxGroup("The One Third Form Attacks")]
        private TentacleCeilingAttack m_tentacleCeilingAttack;
        [SerializeField, BoxGroup("The One Third Form Attacks")]
        private MovingTentacleGroundAttack m_movingTentacleGroundAttack;

        private int m_tentacleStabCount = 0;
        [SerializeField]
        private float m_tentacleStabTimer = 0f;
        private float m_tentacleStabTimerValue;
        [SerializeField]
        private bool m_playerIsGrounded; //temporary but we need to check someday how to get The One to see player is grounded

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                GameEventMessage.SendEvent("Boss Encounter");
            }
        }

        void Update()
        {
            //m_tentacleStabTimer -= GameplaySystem.time.deltaTime;

            //if(m_tentacleStabTimer <= 0)
            //{
            //    StartCoroutine(m_tentacleStabAttack.ExecuteAttack(m_targetInfo.position));
            //    m_tentacleStabTimer = m_tentacleStabTimerValue;
            //}

            
           
        }

        protected override void OnForbidFromAttackTarget()
        {
            
        }

        protected override void OnTargetDisappeared()
        {
            
        }

        public override void ReturnToSpawnPoint()
        {
            
        }

#if UNITY_EDITOR
        [Button]
        private void ForceAttack()
        {
            //StartCoroutine(m_tentacleStabAttack.ExecuteAttack(m_targetInfo.position));
            //StartCoroutine(m_tentacleCeilingAttack.ExecuteAttack());
            StartCoroutine(m_movingTentacleGroundAttack.ExecuteAttack());
        }
#endif

    }
}

