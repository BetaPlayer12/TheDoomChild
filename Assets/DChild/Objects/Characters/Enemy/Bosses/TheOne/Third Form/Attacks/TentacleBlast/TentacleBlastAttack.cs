using System;
using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TentacleBlastAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private TentacleBlast m_leftTentacleBlast;
        [SerializeField]
        private TentacleBlast m_rightTentacleBlast;
        [SerializeField]
        private Transform m_arenaCenter;

        public event EventAction<EventActionArgs> AttackStart;
        public event EventAction<EventActionArgs> AttackDone;

        private void Awake()
        {
            m_leftTentacleBlast.AttackDone += OnLeftTentacleBlastDone;
            m_rightTentacleBlast.AttackDone += OnRightTentacleBlastDone;
        }

        private void OnRightTentacleBlastDone(object sender, EventActionArgs eventArgs)
        {
            AttackDone?.Invoke(this, EventActionArgs.Empty);
        }

        private void OnLeftTentacleBlastDone(object sender, EventActionArgs eventArgs)
        {
            AttackDone?.Invoke(this, EventActionArgs.Empty);
        }

        public IEnumerator ExecuteAttack()
        {
            StartCoroutine(m_leftTentacleBlast.TentacleBlastAttack());
            StartCoroutine(m_rightTentacleBlast.TentacleBlastAttack());

            yield return null;
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(AITargetInfo Target)
        {
            AttackStart?.Invoke(this, EventActionArgs.Empty);

            if (Target.position.x < m_arenaCenter.position.x)
                yield return m_leftTentacleBlast.TentacleBlastAttack();
            else
                yield return m_rightTentacleBlast.TentacleBlastAttack();

            AttackDone?.Invoke(this, EventActionArgs.Empty);
        }
    }
}

