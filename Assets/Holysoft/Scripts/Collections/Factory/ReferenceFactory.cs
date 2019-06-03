using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Holysoft.Collections
{
    public class ReferenceFactory : MonoBehaviour, IReferenceFactory
    {
        public event EventAction<ReferenceInstanceEventArgs> InstanceCreated;
        [SerializeField]
        private GameObject m_template;
#if UNITY_EDITOR
        [SerializeField]
        private bool m_instantiateAsPrefab;
#endif
        [SerializeField]
        private bool m_produceOnStart;

        private IReferenceFactoryData m_data;
        private ReferenceInstanceEventArgs m_eventArgs;

        public void StartProduction()
        {
            if (m_data == null)
            {
                m_data = GetComponent<IReferenceFactoryData>();
            }
            if (m_eventArgs == null)
            {
                m_eventArgs = new ReferenceInstanceEventArgs();
            }

            for (int i = 0; i < m_data.instanceCount; i++)
            {
                var instance = CreateInstance(m_template);
                m_eventArgs.SetValue(instance);
                m_eventArgs.referenceIndex = i;
                InstanceCreated?.Invoke(this, m_eventArgs);
#if UNITY_EDITOR
                RecordInstance(instance);
#endif
            }
        }
        private GameObject CreateInstance(GameObject template)
        {
#if UNITY_EDITOR
            if (m_instantiateAsPrefab)
            {
                return (GameObject)PrefabUtility.InstantiatePrefab(template);
            }
            else
            {
                return Instantiate(m_template);
            }
#else
              return Instantiate(m_template);
#endif
        }

        private void Start()
        {
            if (m_produceOnStart)
            {
                StartProduction();
            }
        }

#if UNITY_EDITOR
        [SerializeField]
        [ReadOnly]
        private List<GameObject> m_instances;

        [Button("Clean Up")]
        private void ClearNull()
        {
            m_instances.RemoveAll(x => x == null);
        }

        protected void RecordInstance(GameObject instance)
        {
            m_instances.Add(instance);
        }

        public void RestartProduction()
        {
            for (int i = m_instances.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(m_instances[i]);
            }
            m_instances.Clear();
            StartProduction();
        }
#endif
    }
}