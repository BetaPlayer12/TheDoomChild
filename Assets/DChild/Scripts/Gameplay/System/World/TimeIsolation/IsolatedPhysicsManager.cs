using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems.WorldComponents
{
    [System.Serializable]
    public class IsolatedPhysicsHandler
    {
        [SerializeField]
        private List<IIsolatedPhysics> m_isolatedPhysics;

        public IsolatedPhysicsHandler()
        {
            m_isolatedPhysics = new List<IIsolatedPhysics>();
        }

        public void Register(IIsolatedPhysics isolatedPhysicsObject)
        {
            if (m_isolatedPhysics.Contains(isolatedPhysicsObject) == false)
            {
                m_isolatedPhysics.Add(isolatedPhysicsObject);
            }
        }

        public void Unregister(IIsolatedPhysics isolatedPhysicsObject)
        {
            if (m_isolatedPhysics.Contains(isolatedPhysicsObject))
            {
                m_isolatedPhysics.Remove(isolatedPhysicsObject);
            }
        }

        public void UpdatePhysics()
        {
            for (int i = m_isolatedPhysics.Count - 1; i >= 0; i--)
            {
                if (m_isolatedPhysics[i] == null)
                {
                    m_isolatedPhysics.RemoveAt(i);
                }
                else
                {
                    try
                    {
                        m_isolatedPhysics[i].UpdatePhysics();
                    }
                    catch(Exception e)
                    {
                        Debug.LogError(e.Message, (IsolatedPhysics2D)m_isolatedPhysics[i]);
                    }
                }
            }
        }
    }
}