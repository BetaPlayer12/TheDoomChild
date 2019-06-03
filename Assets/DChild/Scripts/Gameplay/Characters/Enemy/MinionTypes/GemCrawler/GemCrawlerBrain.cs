using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GemCrawlerBrain : MinionAIBrain<GemCrawler>, IAITargetingBrain
    {
        [SerializeField]
        private RaySensor m_wallSensor;
        [SerializeField]
        private RaySensor m_groundSensor;

        [SerializeField]
        [MinValue(1f)]
        private float m_attackRange;
        [SerializeField]
        private float m_crystalSpawn;
        [SerializeField]
        [Range(0f, 1f)]
        private float m_targetNormalYTreshold = 0.85f;
        [SerializeField]
        [MinValue(0f)]
        private float m_chaseRange; //If target is within this range it will chase it;
        private WayPointPatroler m_patrol;
        private bool m_hasTurned;

        private void Patrol()
        {
            var destination = m_patrol.GetInfo(m_minion.position).destination;

            if (m_patrol.isNearDestination)
            {
                m_minion.Scout();
            }
            else if (IsLookingAt(destination))
            {
                m_minion.Patrol();
            }
            else
            {
                m_minion.Turn();
                m_hasTurned = true;
            }

        }

        public override void Enable(bool value)
        {
            if (!value)
            {
                m_target = null;
            }
            else
            {
                m_minion.Scout();
            }
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_target = null;
            m_patrol.Initialize();
        }

        public void SetTarget(IEnemyTarget target)
        {
            if (target != null)
            {
                m_target = target;
            }
        }

        private void MoveToTarget(Vector2 targetPosition)
        {
            var direction = targetPosition - m_minion.position;
            var directionNormal = direction.normalized;
            if (m_wallSensor.isDetecting || m_groundSensor.isDetecting == false)
            {
                m_minion.Idle();
                m_wallSensor.enabled = false;
            }
            else if (Mathf.Abs(directionNormal.y) > m_targetNormalYTreshold)
            {
                m_minion.Idle();
            }
            else
            {
                m_minion.Move();
                m_wallSensor.enabled = true;
            }
        }

        private void CreateGemSpike(Vector2 targetPosition)
        {
            var spikePosition = targetPosition;
            int hitCount;
            Raycaster.SetLayerCollisionMask(LayerMask.NameToLayer("EnvironmentOnly"));
            var hits = Raycaster.Cast(targetPosition, Vector2.down, true, out hitCount);
            m_minion.CreateGemSpike(hits[0].point);
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
            m_patrol.Initialize();
        }

        private void Start()
        {
            if (m_minion.currentFacingDirection == HorizontalDirection.Left)
            {
                m_wallSensor.SetRotation(180f);
                m_groundSensor.SetRotation(-90f);
            }
            else
            {
                m_wallSensor.SetRotation(0f);
                m_groundSensor.SetRotation(90f);
            }
        }

        private void Update()
        {

            if (m_minion.waitForBehaviourEnd)
                return;

            if (m_hasTurned)
            {
                if (m_minion.currentFacingDirection == HorizontalDirection.Left)
                {
                    m_wallSensor.SetRotation(180f);
                    m_groundSensor.SetRotation(-90f);
                }
                else
                {
                    m_wallSensor.SetRotation(0f);
                    m_groundSensor.SetRotation(90f);
                }

                m_hasTurned = false;
            }

            if (m_target == null)
            {
                Patrol();
            }
            else
            {
                var targetPosition = m_target.position;
                var distanceToTarget = Vector2.Distance(targetPosition, m_minion.position);
                if (distanceToTarget <= m_chaseRange)
                {
                    if (IsLookingAt(targetPosition))
                    {
                        if (distanceToTarget <= m_attackRange)
                        {
                            CreateGemSpike(targetPosition);
                        }
                        else
                        {
                            MoveToTarget(targetPosition);
                        }
                    }
                    else
                    {
                        m_minion.Turn();
                        m_hasTurned = true;
                        m_wallSensor.enabled = true;
                    }

                }
                else
                {
                    m_target = null;
                }

            }
        }



#if UNITY_EDITOR
        public float attackRange => m_attackRange;
        public float chaseRange => m_chaseRange;
#endif
    }
}
