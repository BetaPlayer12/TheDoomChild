using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Spine.Unity;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.AI;
using Holysoft.Event;
using System;
using DChild.Gameplay.Combat;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TentacleGroundStabAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private GameObject m_groundTentacleStab;
        [SerializeField]
        private float m_tentacleSpawnInterval = 2f;
        [SerializeField] 
        private float m_spawnOffsetMin;
        [SerializeField]
        private float m_spawnOffsetMax;
        [SerializeField]
        private Transform m_tentacleSpawnHeight;

        private int m_tentacleCount = 0;

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

        [SerializeField]
        private List<PoolableObject> m_tentaclesSpawned = new List<PoolableObject>();
        [SerializeField]
        private List<float> m_tentaclesSpawnedXPositions = new List<float>();


        public event EventAction<EventActionArgs> AttackStart;
        public event EventAction<EventActionArgs> AttackDone;

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
                    InstantiateTentacles(new Vector2(target.x, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_backgroundSortingLayerID, m_backgroundSortingLayerName);
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
                else if(m_tentacleCount == 2)
                {
                    InstantiateTentacles(new Vector2(target.x, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_midgroundSortingLayerID, m_midgroundSortingLayerName);
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
                else if (m_tentacleCount == 3)
                {
                    InstantiateTentacles(new Vector2(target.x, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_playablegroundSortingLayerID, m_playablegroundSortingLayerName);
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
                else if (m_tentacleCount == 4)
                {
                    InstantiateTentacles(new Vector2(target.x, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_foregroundSortingLayerID, m_foregroundSortingLayerName);
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
            }
            m_tentacleCount = 0;

            yield return null;
        }

        public IEnumerator ExecuteAttack(AITargetInfo Target)
        {
            AttackStart?.Invoke(this, EventActionArgs.Empty);
            while (m_tentacleCount < 5)
            {
                m_tentacleCount++;
                if (m_tentacleCount == 1)
                {
                    InstantiateTentacles(new Vector2(Target.position.x, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_backgroundSortingLayerID, m_backgroundSortingLayerName);
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
                else if (m_tentacleCount == 2)
                {
                    if (m_tentaclesSpawnedXPositions.Contains(Target.position.x))
                    {
                        int randomRoll = UnityEngine.Random.Range(0, 2);
                        float randomOffset = UnityEngine.Random.Range(m_spawnOffsetMin, m_spawnOffsetMax);
                        if (randomRoll == 0)
                        {
                            InstantiateTentacles(new Vector2(m_tentaclesSpawnedXPositions[m_tentaclesSpawnedXPositions.Count - 1] + randomOffset, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_midgroundSortingLayerID, m_midgroundSortingLayerName);
                        }
                        else
                        {
                            InstantiateTentacles(new Vector2(m_tentaclesSpawnedXPositions[m_tentaclesSpawnedXPositions.Count - 1] - randomOffset, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_midgroundSortingLayerID, m_midgroundSortingLayerName);
                        }
                    }
                    else
                    {
                        InstantiateTentacles(new Vector2(Target.position.x, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_midgroundSortingLayerID, m_midgroundSortingLayerName);
                    }
                    //InstantiateTentacles(new Vector2(Target.position.x, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_midgroundSortingLayerID, m_midgroundSortingLayerName);
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);

                }
                else if (m_tentacleCount == 3)
                {
                    if (m_tentaclesSpawnedXPositions.Contains(Target.position.x))
                    {
                        int randomRoll = UnityEngine.Random.Range(0, 2);
                        float randomOffset = UnityEngine.Random.Range(m_spawnOffsetMin, m_spawnOffsetMax);
                        {
                            if (randomRoll == 0)
                            {
                                InstantiateTentacles(new Vector2(m_tentaclesSpawnedXPositions[m_tentaclesSpawnedXPositions.Count - 1] + randomOffset, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_playablegroundSortingLayerID, m_playablegroundSortingLayerName);
                            }
                            else
                            {
                                InstantiateTentacles(new Vector2(m_tentaclesSpawnedXPositions[m_tentaclesSpawnedXPositions.Count - 1] - randomOffset, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_playablegroundSortingLayerID, m_playablegroundSortingLayerName);
                            }
                        }
                    }
                    else
                    {
                        InstantiateTentacles(new Vector2(Target.position.x, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_playablegroundSortingLayerID, m_playablegroundSortingLayerName);
                    }
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
                else if (m_tentacleCount == 4)
                {
                    if (m_tentaclesSpawnedXPositions.Contains(Target.position.x))
                    {
                        int randomRoll = UnityEngine.Random.Range(0, 2);
                        float randomOffset = UnityEngine.Random.Range(m_spawnOffsetMin, m_spawnOffsetMax);
                        {
                            if (randomRoll == 0)
                            {
                                InstantiateTentacles(new Vector2(m_tentaclesSpawnedXPositions[m_tentaclesSpawnedXPositions.Count - 1] + randomOffset, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_foregroundSortingLayerID, m_foregroundSortingLayerName);
                            }
                            else
                            {
                                InstantiateTentacles(new Vector2(m_tentaclesSpawnedXPositions[m_tentaclesSpawnedXPositions.Count - 1] - randomOffset, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_foregroundSortingLayerID, m_foregroundSortingLayerName);
                            }
                        }
                    }
                    else
                    {
                        InstantiateTentacles(new Vector2(Target.position.x, m_tentacleSpawnHeight.position.y), m_groundTentacleStab, m_foregroundSortingLayerID, m_foregroundSortingLayerName);
                    }
                    yield return new WaitForSeconds(m_tentacleSpawnInterval);
                }
            }
            m_tentacleCount = 0;
            m_tentaclesSpawned.Clear();
            AttackDone?.Invoke(this, EventActionArgs.Empty);

            yield return null;
        }

        private void InstantiateTentacles(Vector2 spawnPosition, GameObject tentacle, int sortingLayerID, string sortingLayerName)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(tentacle, gameObject.scene);
            instance.SpawnAt(spawnPosition, Quaternion.identity);
            //instance.transform.SetParent(this.transform);

            instance.GetComponent<Damageable>().OvverideHealthPointer(GetComponentInParent<Health>());
            //instance.GetComponent<TentacleGroundStab>().GetHurtBox().GetComponent<Hitbox>().OvverideDamageable(GetComponentInParent<Damageable>());
            Debug.Log("i have i have");

            instance.GetComponentInChildren<MeshRenderer>().sortingLayerID = sortingLayerID;
            instance.GetComponentInChildren<MeshRenderer>().sortingLayerName = sortingLayerName;

            if(sortingLayerName == "PlayableGround")
            {
                instance.GetComponent<TentacleGroundStab>().isOnPlayableGround = true;
            }

            Component[] spriteRenderers;
            spriteRenderers = instance.GetComponentsInChildren(typeof(SpriteRenderer), true);
            
            
            foreach(SpriteRenderer safeZone in spriteRenderers)
            {
                safeZone.sortingLayerID = sortingLayerID;
                safeZone.sortingLayerName = sortingLayerName;
                if(sortingLayerName != m_playablegroundSortingLayerName)
                    safeZone.gameObject.layer = 12;
            }

            //instance.transform.SetParent(null);
            m_tentaclesSpawned.Add(instance);
            m_tentaclesSpawnedXPositions.Add(instance.transform.position.x);
        }
    }
}

