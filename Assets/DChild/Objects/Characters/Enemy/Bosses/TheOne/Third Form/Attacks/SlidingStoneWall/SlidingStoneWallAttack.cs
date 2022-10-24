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

        public IEnumerator ExecuteAttack()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        private void InstantiateWall(Vector2 spawnPosition, GameObject wall)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(wall, gameObject.scene);
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
    }

}
