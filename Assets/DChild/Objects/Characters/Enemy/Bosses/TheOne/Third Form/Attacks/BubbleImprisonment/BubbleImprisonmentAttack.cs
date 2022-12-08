using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pooling;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BubbleImprisonmentAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private BubbleImprisonment m_bubbleImprisonment;

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
            throw new System.NotImplementedException();
        }

        private IEnumerator FollowPlayer()
        {
            yield return null;
        }

        private void InstantiateTentacles(Vector2 spawnPosition, GameObject bubble)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(bubble, gameObject.scene);
            instance.SpawnAt(spawnPosition, Quaternion.identity);
        }

        
    }
}

