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

            [SerializeField]
            private MovementInfo m_moveSideways = new MovementInfo();
            public MovementInfo moveSideways => m_moveSideways;

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
            m_mouthBlastOriginalPosition = transform.position;
            m_mouthBlastOneLaser.SetActive(false);
            m_doMouthBlastIAttack = false;
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

        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_leftWallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_rightWallSensor;

        [SerializeField, BoxGroup("The One Third Form Attacks")]
        private TentacleGroundStabAttack m_tentacleStabAttack;
        [SerializeField, BoxGroup("The One Third Form Attacks")]
        private TentacleCeilingAttack m_tentacleCeilingAttack;
        [SerializeField, BoxGroup("The One Third Form Attacks")]
        private MovingTentacleGroundAttack m_movingTentacleGroundAttack;
        [SerializeField, BoxGroup("The One Third Form Attacks")]
        private ChasingGroundTentacleAttack m_chasingGroundTentacleAttack;
        [SerializeField, BoxGroup("The One Third Form Attacks")]
        private MouthBlastIIAttack m_mouthBlastIIAttack;
        [SerializeField, BoxGroup("The One Third Form Attacks")]
        private SlidingStoneWallAttack m_slidingWallAttack;

        [SerializeField, BoxGroup("Mouth Blast I Stuff")]
        private GameObject m_mouthBlastOneLaser;
        [SerializeField, BoxGroup("Mouth Blast I Stuff")]
        private Transform m_mouthBlastLeftSide;
        [SerializeField, BoxGroup("Mouth Blast I Stuff")]
        private Transform m_mouthBlastRightSide;
        [SerializeField, BoxGroup("Mouth Blast I Stuff")]
        private float m_mouthBlastMoveSpeed;
        [SerializeField, BoxGroup("Mouth Blast I Stuff")]
        private Vector2 m_mouthBlastOriginalPosition;
        [SerializeField, BoxGroup("Mouth Blast I Stuff")]
        private BlackBloodFlood m_blackBloodFlood;
        private bool m_doMouthBlastIAttack;
        private bool m_moveMouth;
        private int m_SideToStart;

        //stuff for tentacle stab attack
        private int m_tentacleStabCount = 0;
        [SerializeField, BoxGroup("Tentacle Stab Attack Stuff")]
        private float m_tentacleStabTimer = 0f;
        private float m_tentacleStabTimerValue;
       
        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                GameEventMessage.SendEvent("Boss Encounter");
            }
        }

        private IEnumerator MouthBlastOneAttack(int side)
        {         
            //transform to mouth
            yield return new WaitForSeconds(2f);
            //move to left or right
            if(side == 0)
            {
                transform.position = m_mouthBlastLeftSide.position;
                m_SideToStart = side;
            }
            else if(side == 1)
            {
                transform.position = m_mouthBlastRightSide.position;
            }

            yield return new WaitForSeconds(1f);

            //spawn blast
            m_mouthBlastOneLaser.SetActive(true);

            m_moveMouth = true;
            m_blackBloodFlood.m_isFlooding = true;

            yield return null;
            
        }

        private IEnumerator MoveMouthBlast(int side)
        {
            if (side == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_mouthBlastRightSide.position, m_mouthBlastMoveSpeed);

                if(transform.position == m_mouthBlastRightSide.position)
                {
                    StartCoroutine(EndMouthBlast());
                }
            }
            else if (side == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_mouthBlastLeftSide.position, m_mouthBlastMoveSpeed);

                if (transform.position == m_mouthBlastLeftSide.position)
                {
                    StartCoroutine(EndMouthBlast());
                }
            }

            yield return new WaitForSeconds(1f);
        }

        private IEnumerator EndMouthBlast()
        {
            m_doMouthBlastIAttack = false;
            m_blackBloodFlood.m_isFlooding = false;
            yield return new WaitForSeconds(2f);

            //end attack, return to original position
            transform.position = m_mouthBlastOriginalPosition;
            m_mouthBlastOneLaser.SetActive(false);
            m_moveMouth = false;
            yield return null;
        }

        void Update()
        {
            //m_tentacleStabTimer -= GameplaySystem.time.deltaTime;

            //if (m_tentacleStabTimer <= 0)
            //{
            //    Debug.Log("Player is detected: " + m_targetInfo.doesTargetExist);
            //    StartCoroutine(m_tentacleStabAttack.ExecuteAttack(m_targetInfo.position));
            //    m_tentacleStabTimer = m_tentacleStabTimerValue;
            //}

            //if (m_doMouthBlastIAttack)
            //{
            //    //var rollSide = Random.Range(0, 2);
            //    m_doMouthBlastIAttack = false;
            //    StartCoroutine(MouthBlastOneAttack(m_SideToStart));
            //    //
            //}

            //if (m_moveMouth)
            //{
            //    StartCoroutine(MoveMouthBlast(m_SideToStart));
            //}
            

            //transform.position = Vector2.MoveTowards(transform.position, m_mouthBlastLeftSide.position, m_mouthBlastMoveSpeed);
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

        [Button]
        private void ForceAttack()
        {
            //StartCoroutine(m_tentacleStabAttack.ExecuteAttack(m_targetInfo.position));
            //StartCoroutine(m_tentacleCeilingAttack.ExecuteAttack());
            //StartCoroutine(m_movingTentacleGroundAttack.ExecuteAttack());
            //StartCoroutine(m_chasingGroundTentacleAttack.ExecuteAttack());
            //StartCoroutine(m_mouthBlastIIAttack.ExecuteAttack());
            //StartCoroutine(MouthBlastOneAttack());

            //m_doMouthBlastIAttack = true;
            //var rollSide = Random.Range(0, 2);
            //m_SideToStart = rollSide;

            StartCoroutine(m_slidingWallAttack.ExecuteAttack());
        }

    }
}

