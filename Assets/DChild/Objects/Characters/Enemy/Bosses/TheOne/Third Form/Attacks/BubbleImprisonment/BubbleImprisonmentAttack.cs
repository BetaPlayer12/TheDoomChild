using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BubbleImprisonmentAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private BubbleImprisonment m_bubbleImprisonment;
        [SerializeField]
        private int m_maxNumberOfBubblesToSpawn;
        [SerializeField]
        private float m_timeBetweenAnimations;
        [SerializeField]
        private float m_timeBetweenBubbleSpawn;
        private int m_bubbleCounter;

        public IEnumerator ExecuteAttack()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(AITargetInfo Target)
        {
            m_bubbleCounter = 0;

            while (m_bubbleCounter < m_maxNumberOfBubblesToSpawn)
            {
                InstantiateBubble(new Vector2(Target.position.x, Target.position.y), m_bubbleImprisonment.gameObject);
                m_bubbleCounter++;
                yield return new WaitForSeconds(m_timeBetweenBubbleSpawn);
            }
            yield return null;
        }

        private void InstantiateBubble(Vector2 spawnPosition, GameObject bubble)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(bubble, gameObject.scene);
            instance.SpawnAt(spawnPosition, Quaternion.identity);
            instance.GetComponent<BubbleImprisonment>().timeBetweenAnimations = m_timeBetweenAnimations;
        }
    }
}

