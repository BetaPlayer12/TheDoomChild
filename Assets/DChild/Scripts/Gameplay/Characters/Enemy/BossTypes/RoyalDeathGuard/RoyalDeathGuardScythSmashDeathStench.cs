﻿using UnityEngine;
using System.Collections.Generic;
using Holysoft.Event;
using System.Collections;
using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Characters.Enemies
{
    public class RoyalDeathGuardScythSmashDeathStench : MonoBehaviour
    {
        [SerializeField]
        private RaySensor m_toWallRaycast;
        [SerializeField]
        private float m_groundPosition;

        [SerializeField]
        private GameObject m_segment;
        [SerializeField]
        private float m_segmentDistanceInterval;
        [SerializeField]
        private float m_segmentSpawnInterval;
        [SerializeField]
        private float m_maxStenchShadowDistance;

        private List<RoyalDeathGuardDeathStenchSegment> segment;

        public event EventAction<EventActionArgs> Done;
        private bool m_isSpawningSegments;

        [Button]
        public void Execute()
        {
            if (m_isSpawningSegments)
                return;

            m_toWallRaycast.multiRaycast.SetCastDistance(m_maxStenchShadowDistance);
            m_toWallRaycast.Cast();
            var hasHitWall = m_toWallRaycast.isDetecting;
            var distanceToWall = hasHitWall ? m_toWallRaycast.GetUniqueHits()[0].distance : m_maxStenchShadowDistance;
            var instancesToCreate = Mathf.CeilToInt(distanceToWall / m_segmentDistanceInterval);
            Debug.Log("Spawn number of death stench: " + instancesToCreate);
            StartCoroutine(SpawnSegments(instancesToCreate));
        }

        private IEnumerator SpawnSegments(int numberToSpawn)
        {
            m_isSpawningSegments = true;
            var direction = transform.right;
            var startSpawnPosition = transform.position;
            startSpawnPosition.y = m_groundPosition;
            RoyalDeathGuardDeathStenchSegment lastSegment = null;
            for (int i = 0; i < numberToSpawn; i++)
            {
                var spawnPosition = startSpawnPosition + (direction * m_segmentDistanceInterval * i);
                var segment = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_segment, spawnPosition, Quaternion.identity);
                lastSegment = segment.GetComponent<RoyalDeathGuardDeathStenchSegment>();
                lastSegment.Execute();
                yield return new WaitForSeconds(m_segmentSpawnInterval);
            }

            bool isLastSegmentDone = false;
            lastSegment.Done += OnLastSegmentDone;
            while (isLastSegmentDone == false)
                yield return null;
            lastSegment.Done -= OnLastSegmentDone;

            m_isSpawningSegments = false;
            Done?.Invoke(this, EventActionArgs.Empty);

            void OnLastSegmentDone(object sender, EventActionArgs eventActionArgs)
            {
                isLastSegmentDone = true;
            }
        }

        private void OnDrawGizmosSelected()
        {
            var direction = transform.right;
            var startSpawnPosition = transform.position;
            for (int i = 0; i < 5; i++)
            {
                var spawnPosition = startSpawnPosition + (direction * m_segmentDistanceInterval * i);
                Gizmos.DrawCube(spawnPosition, Vector3.one * 0.5f);
            }
        }
    }
}