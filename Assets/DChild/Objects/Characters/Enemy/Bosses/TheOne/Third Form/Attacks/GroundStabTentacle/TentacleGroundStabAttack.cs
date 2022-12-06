using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Spine.Unity;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.AI;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TentacleGroundStabAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private GameObject m_groundTentacleStab;
        [SerializeField]
        private float m_tentacleSpawnInterval = 2f;

        private int m_tentacleCount = 0;
        private Vector2 m_tentacleOffset = new Vector2(0, 50f);

        [SerializeField]
        private int m_backgroundSortingLayerID = -3;
        [SerializeField]
        private int m_midgroundSortingLayerID = -2;
        [SerializeField]
        private int m_playablegroundSortingLayerID = -1;
        [SerializeField]
        private int m_foregroundSortingLayerID = 2;

        [SerializeField]
        private string m_backgroundSortingLayerName = "Background";
        [SerializeField]
        private string m_midgroundSortingLayerName = "Midground1";
        [SerializeField]
        private string m_playablegroundSortingLayerName = "PlayableGround";
        [SerializeField]
        private string m_foregroundSortingLayerName = "Foreground";


        public IEnumerator ExecuteAttack()
        {
            yield return null;
        }

        public IEnumerator ExecuteAttack(Vector2 target)
        {
            while(m_tentacleCount < 5)
            {
                m_tentacleCount++;
                if (m_tentacleCount == 1)
                {
                    InstantiateTentacles(new Vector2(target.x, target.y) + m_tentacleOffset, m_groundTentacleStab, m_backgroundSortingLayerID, m_backgroundSortingLayerName);
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
                else if(m_tentacleCount == 2)
                {
                    InstantiateTentacles(new Vector2(target.x, target.y) + m_tentacleOffset, m_groundTentacleStab, m_midgroundSortingLayerID, m_midgroundSortingLayerName);
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
                else if (m_tentacleCount == 3)
                {
                    InstantiateTentacles(new Vector2(target.x, target.y) + m_tentacleOffset, m_groundTentacleStab, m_playablegroundSortingLayerID, m_playablegroundSortingLayerName);
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
                else if (m_tentacleCount == 4)
                {
                    InstantiateTentacles(new Vector2(target.x, target.y) + m_tentacleOffset, m_groundTentacleStab, m_foregroundSortingLayerID, m_foregroundSortingLayerName);
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
            }
            m_tentacleCount = 0;

            yield return null;
        }

        public IEnumerator ExecuteAttack(AITargetInfo Target)
        {
            while (m_tentacleCount < 5)
            {
                m_tentacleCount++;
                if (m_tentacleCount == 1)
                {
                    InstantiateTentacles(new Vector2(Target.position.x, Target.position.y) + m_tentacleOffset, m_groundTentacleStab, m_backgroundSortingLayerID, m_backgroundSortingLayerName);
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
                else if (m_tentacleCount == 2)
                {
                    InstantiateTentacles(new Vector2(Target.position.x, Target.position.y) + m_tentacleOffset, m_groundTentacleStab, m_midgroundSortingLayerID, m_midgroundSortingLayerName);
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
                else if (m_tentacleCount == 3)
                {
                    InstantiateTentacles(new Vector2(Target.position.x, Target.position.y) + m_tentacleOffset, m_groundTentacleStab, m_playablegroundSortingLayerID, m_playablegroundSortingLayerName);
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
                else if (m_tentacleCount == 4)
                {
                    InstantiateTentacles(new Vector2(Target.position.x, Target.position.y) + m_tentacleOffset, m_groundTentacleStab, m_foregroundSortingLayerID, m_foregroundSortingLayerName);
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
            }
            m_tentacleCount = 0;

            yield return null;
        }

        private void InstantiateTentacles(Vector2 spawnPosition, GameObject tentacle, int sortingLayerID, string sortingLayerName)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(tentacle, gameObject.scene);
            instance.SpawnAt(spawnPosition, Quaternion.identity);
            instance.GetComponentInChildren<MeshRenderer>().sortingLayerID = sortingLayerID;
            instance.GetComponentInChildren<MeshRenderer>().sortingLayerName = sortingLayerName;
        }
    }
}

