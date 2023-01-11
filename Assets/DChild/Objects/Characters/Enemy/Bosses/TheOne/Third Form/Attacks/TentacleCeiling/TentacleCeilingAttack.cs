using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using Spine.Unity;
using DChild.Gameplay.Characters.AI;
using Holysoft.Event;
using System;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TentacleCeilingAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private TentacleCeiling m_leftTentacle;
        [SerializeField]
        private TentacleCeiling m_rightTentacle;

        [SerializeField]
        private float m_ceilingDuration;

        public event EventAction<EventActionArgs> AttackStart;
        public event EventAction<EventActionArgs> AttackDone;

        private void Awake()
        {
            m_leftTentacle.AttackDone += OnLeftTentacleCeilingDone;
            m_rightTentacle.AttackDone += OnRightTentacleCeilingDone;
        }

        private void OnRightTentacleCeilingDone(object sender, EventActionArgs eventArgs)
        {
            AttackDone?.Invoke(this, EventActionArgs.Empty);
        }

        private void OnLeftTentacleCeilingDone(object sender, EventActionArgs eventArgs)
        {
            AttackDone?.Invoke(this, EventActionArgs.Empty);
        }

        public IEnumerator ExecuteAttack()
        {
            var rollOdds = UnityEngine.Random.Range(1, 3);

            //Decide whether to use one or two tentacles to create ceiling
            if(rollOdds == 1)
            {
                var rollSide = UnityEngine.Random.Range(1, 3);

                if(rollSide == 1)
                {
                    StartCoroutine(m_leftTentacle.Attack(m_ceilingDuration));
                }
                else if(rollSide == 2)
                {
                    StartCoroutine(m_rightTentacle.Attack(m_ceilingDuration));
                }
            }
            else if(rollOdds == 2)
            {
                StartCoroutine(m_leftTentacle.Attack(m_ceilingDuration));
                StartCoroutine(m_rightTentacle.Attack(m_ceilingDuration));
            }

            yield return null;
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(AITargetInfo Target)
        {
            throw new System.NotImplementedException();
        }
    }
}

