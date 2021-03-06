﻿using System.Collections.Generic;

namespace DChild.Gameplay.Systems.WorldComponents
{
    public class IsolatedPhysicsHandler
    {
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
                if(m_isolatedPhysics[i] == null)
                {
                    m_isolatedPhysics.RemoveAt(i);
                }
                else
                {
                    m_isolatedPhysics[i].UpdatePhysics();
                }
            }
        }
    }
}