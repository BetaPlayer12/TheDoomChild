using System.Collections.Generic;
using System.Linq;

namespace DChild.Gameplay.Systems.WorldComponents
{
    public class IsolatedObjectHandler
    {
        private float m_timeScale;
        private List<IIsolatedObject> m_isolatedObjects;
        private List<IIsolatedObject> m_toUpdateComponents;
        private List<IIsolatedObject> m_toUpdatePhysicsComponents;

#if UNITY_EDITOR
        public int registeredObjectCount => m_isolatedObjects.Count;
        public int registeredComponentsCount
        {
            get
            {
                int componentCount = 0;
                for (int i = 0; i < m_isolatedObjects.Count; i++)
                {
                    componentCount += m_isolatedObjects[i].componentCount;
                }
                return componentCount;
            }
        }
#endif

        public IsolatedObjectHandler(float timeScale)
        {
            this.m_timeScale = timeScale;
            m_isolatedObjects = new List<IIsolatedObject>();
            m_toUpdateComponents = new List<IIsolatedObject>();
            m_toUpdatePhysicsComponents = new List<IIsolatedObject>();
        }

        public void Register(IIsolatedObject isolatedObject)
        {
            isolatedObject.SetTimeScale(m_timeScale);
            isolatedObject.UpdateDeltaTime();
            m_isolatedObjects.Add(isolatedObject);
        }

        public void Unregister(IIsolatedObject isolatedObject)
        {
            isolatedObject.SetTimeScale(1f);
            m_isolatedObjects.Remove(isolatedObject);
        }

        public void UpdateComponentOf(IIsolatedObject isolatedObject)
        {
            if (m_toUpdateComponents.Contains(isolatedObject) == false)
            {
                m_toUpdateComponents.Add(isolatedObject);
            }
        }

        public void UpdatePhysicsComponentOf(IIsolatedObject isolatedObject)
        {
            if (m_toUpdatePhysicsComponents.Contains(isolatedObject) == false)
            {
                m_toUpdatePhysicsComponents.Add(isolatedObject);
            }
        }

        public void AlignTime(float timeScale)
        {
            m_timeScale = timeScale;
            for (int i = 0; i < m_isolatedObjects.Count; i++)
            {
                var isolatedObject = m_isolatedObjects[i];
                isolatedObject.SetTimeScale(m_timeScale);
                isolatedObject.UpdateComponents();
                isolatedObject.UpdatePhysicsComponent();
                //Isolated Objects might request to be updated during the for loop
                m_toUpdateComponents.Remove(isolatedObject);
            }
        }

        public void UpdateComponents()
        {
            for (int i = 0; i < m_toUpdateComponents.Count; i++)
            {
                m_toUpdateComponents[i].UpdateComponents();
            }
            m_toUpdateComponents.Clear();
        }

        public void UpdatePhysicsComponents()
        {
            for (int i = 0; i < m_toUpdatePhysicsComponents.Count; i++)
            {
                m_toUpdatePhysicsComponents[i].UpdatePhysicsComponent();
            }
            m_toUpdatePhysicsComponents.Clear();
        }

        public void UpdateDeltaTime()
        {
            for (int i = 0; i < m_isolatedObjects.Count; i++)
            {
                m_isolatedObjects[i].UpdateDeltaTime();
            }
        }

        public void CalculateActualVelocity()
        {
            for (int i = 0; i < m_isolatedObjects.Count; i++)
            {
                m_isolatedObjects[i].CalculateActualVelocity();
            }
        }

        public void ClearNull() => m_isolatedObjects = m_isolatedObjects.Where(item => item != null).ToList();
    }
}