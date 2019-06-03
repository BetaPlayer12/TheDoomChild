using System.Collections.Generic;

namespace DChild.Gameplay.Systems.WorldComponents
{
    public class InteractiveEnvironmentHandler
    {
        private List<IInteractiveEnvironment> m_items;

        public InteractiveEnvironmentHandler()
        {
            this.m_items = new List<IInteractiveEnvironment>();
        }

#if UNITY_EDITOR
        public int registeredObjectCount => m_items.Count;
#endif

        public void Register(IInteractiveEnvironment interactiveEnvironment)
        {
            if (m_items.Contains(interactiveEnvironment) == false)
            {
                m_items.Add(interactiveEnvironment);
            }
        }

        public void Unregister(IInteractiveEnvironment interactiveEnvironment)
        {
            if (m_items.Contains(interactiveEnvironment))
            {
                m_items.Remove(interactiveEnvironment);
            }
        }

        public void Update()
        {
            for (int i = m_items.Count - 1; i >= 0; i--)
            {
                if (m_items[i] == null)
                {
                    m_items.RemoveAt(i);
                }
                else
                {
                    m_items[i].UpdateState();
                }
            }
        }
    }
}