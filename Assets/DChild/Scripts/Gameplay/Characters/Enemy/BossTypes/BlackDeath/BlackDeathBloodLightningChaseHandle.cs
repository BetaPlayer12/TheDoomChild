using System;
using System.Collections;
using Holysoft.Event;
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

            bool areLightningDoneExecuting = false;

            for (int i = 0; i < m_spawnCount; i++)
            {
                SpawnLighting(m_poolIndex);
                yield return new WaitForSeconds(m_spawnInterval);

                if (i == m_spawnCount - 1)
                {
                    m_pool[m_poolIndex].IsDone += OnLightingDone;
                }

                m_poolIndex = (int)Mathf.Repeat(m_poolIndex + 1, m_pool.Length);
            }

            while (areLightningDoneExecuting == false)
                yield return null;

            for (int i = 0; i < m_pool.Length; i++)
            {
                m_pool[i].gameObject.SetActive(false);
            }
            m_isExecutinSequence = false;


            void OnLightingDone(object sender, EventActionArgs eventArgs)
            {
                areLightningDoneExecuting = true;
            }
        }

        private void SpawnLighting(int poolIndex)
        {
            BlackDeathBloodLightning instance = m_pool[poolIndex];

            /*var spawnPosition = m_toChase.position;
            spawnPosition.y = transform.position.y;
            instance.transform.position = spawnPosition;
            instance.gameObject.SetActive(true);
            instance.Execute();*/
            var spawnPosition = new Vector3(m_toChase.position.x, transform.position.y, transform.position.z);
            instance.transform.position = spawnPosition;
            instance.gameObject.SetActive(true);
            instance.Execute();
        }
    }

}