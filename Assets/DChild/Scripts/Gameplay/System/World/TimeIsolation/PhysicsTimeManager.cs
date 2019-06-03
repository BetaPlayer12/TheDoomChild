using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.Systems.WorldComponents
{
    public class PhysicsTimeHandler
    {
        private float m_timeScale;
        private List<IPhysicObjects> m_physicObjects;

#if UNITY_EDITOR
        public int registeredObjectCount => 0;
#endif

        public PhysicsTimeHandler(float m_timeScale)
        {
            this.m_timeScale = m_timeScale;
            m_physicObjects = new List<IPhysicObjects>();
        }

        public static void AlignTime(Rigidbody2D[] rigidbodies, PhysicsTimeObjectInfo[] objectInfos, float timeScale)
        {
            for (int i = 0; i < rigidbodies.Length; i++)
            {
                objectInfos[i].AlignTime(rigidbodies[i], timeScale);
            }
        }

        public void Register(IPhysicObjects physicsObject)
        {
            physicsObject.CalculateActualVelocity(m_timeScale);
            m_physicObjects.Add(physicsObject);
        }

        public void Unregister(IPhysicObjects physicsObject)
        {
            physicsObject.Revert();
            m_physicObjects.Remove(physicsObject);
        }

        public void AlignTime(float timeScale)
        {
            m_timeScale = timeScale;
            for (int i = 0; i < m_physicObjects.Count; i++)
            {
                m_physicObjects[i].AlignTime(m_timeScale);
            }
        }

        public void CalculateActualVelocity()
        {
            for (int i = 0; i < m_physicObjects.Count; i++)
            {
                m_physicObjects[i].CalculateActualVelocity(m_timeScale);
            }
        }

        public void ClearNull()
        {
            for (int i = m_physicObjects.Count - 1; i >= 0; i--)
            {
                if (m_physicObjects[i] == null)
                {
                    m_physicObjects.RemoveAt(i);
                }
            }
        }
    }

}