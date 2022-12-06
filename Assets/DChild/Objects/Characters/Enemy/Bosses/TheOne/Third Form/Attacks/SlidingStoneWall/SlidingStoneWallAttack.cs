using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SlidingStoneWallAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private SlidingStoneWall m_monolithWall;
        [SerializeField]
        private Transform m_leftSpawnPoint;
        [SerializeField]
        private Transform m_rightSpawnPoint;
        [SerializeField]
        private Transform m_arenaCenter;

        public IEnumerator ExecuteAttack()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            if(PlayerPosition.x < m_arenaCenter.position.x)
                InstantiateWall(m_rightSpawnPoint.position, m_monolithWall.gameObject, PlayerPosition);
            else
                InstantiateWall(m_leftSpawnPoint.position, m_monolithWall.gameObject, PlayerPosition);              
            
            yield return null;
        }

        private void InstantiateWall(Vector2 spawnPosition, GameObject wall, Vector2 PlayerPosition)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(wall, gameObject.scene);

            if(PlayerPosition.x < m_arenaCenter.position.x)
            {
                instance.GetComponent<SlidingStoneWall>().slideRight = false;
                instance.GetComponent<SlidingStoneWall>().executeAttack = true;
            }
            else
            {
                instance.GetComponent<SlidingStoneWall>().executeAttack = true;
                instance.GetComponent<SlidingStoneWall>().slideRight = true;
            }
            
            instance.SpawnAt(spawnPosition, Quaternion.identity);           
        }

        private void InstantiateRightWall(Vector2 spawnPosition, GameObject wall)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(wall, gameObject.scene);
            instance.GetComponent<SlidingStoneWall>().slideRight = false;
            instance.GetComponent<SlidingStoneWall>().executeAttack = true;
            instance.SpawnAt(spawnPosition, Quaternion.identity);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public IEnumerator ExecuteAttack(AITargetInfo Target)
        {
            throw new System.NotImplementedException();
        }
    }

}
