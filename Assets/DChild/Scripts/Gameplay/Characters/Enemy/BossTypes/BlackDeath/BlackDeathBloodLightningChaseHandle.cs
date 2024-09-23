using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BlackDeathBloodLightningChaseHandle : BlackDeathBloodLightingBehaviourHandle
    {
        [SerializeField]
        private BlackDeathBloodLightning[] m_pool;
        [SerializeField]
        private int m_spawnCount;
        [SerializeField]
        private float m_spawnInterval;

        private int m_poolIndex;

        private bool m_isExecutinSequence;
        private Transform m_toChase;

        [Button, HideInEditorMode]
        public override void Execute()
        {
            if (m_isExecutinSequence)
                return;

            StopAllCoroutines();
            StartCoroutine(PlaySequencesRoutine());
        }

        public void SetTarget(Transform target)
        {
            m_toChase = target;
        }

        private IEnumerator PlaySequencesRoutine()
        {
            m_isExecutinSequence = true;

            for (int i = 0; i < m_spawnCount; i++)
            {
                SpawnLighting(m_poolIndex);
                yield return new WaitForSeconds(m_spawnInterval);
                m_poolIndex = (int)Mathf.Repeat(m_poolIndex + 1, m_pool.Length);
            }

            m_isExecutinSequence = false;
        }

        private void SpawnLighting(int poolIndex)
        {
            BlackDeathBloodLightning instance = m_pool[poolIndex];

            var spawnPosition = m_toChase.position;
            spawnPosition.y = transform.position.y;
            instance.transform.position = spawnPosition;
            instance.Execute();
        }
    }

}