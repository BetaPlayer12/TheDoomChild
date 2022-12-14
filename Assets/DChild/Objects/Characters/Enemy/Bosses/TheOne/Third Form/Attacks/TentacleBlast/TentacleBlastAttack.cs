using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
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
            if (Target.position.x < m_arenaCenter.position.x)
                yield return m_leftTentacleBlast.TentacleBlastAttack();
            else
                yield return m_rightTentacleBlast.TentacleBlastAttack();
        }
    }
}

