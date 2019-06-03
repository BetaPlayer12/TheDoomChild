using Holysoft.Event;
using UnityEngine;

namespace Holysoft.Collections
{
    [RequireComponent(typeof(IFactory))]
    public abstract class FactoryAssembler : MonoBehaviour, IEventListener
    {
        private IFactory m_factory;

        protected abstract void OnInstanceCreated(object sender, UnityEventActionArgs<GameObject> eventArgs);

        protected virtual void OnEnable()
        {
            m_factory = GetComponent<IFactory>();
            m_factory.InstanceCreated += OnInstanceCreated;
        }

        protected void OnDisable()
        {
            if (m_factory != null)
            {
                m_factory.InstanceCreated -= OnInstanceCreated;
            }
        }


#if UNITY_EDITOR
        public virtual void SubscribeToEvents()
        {
            m_factory = GetComponent<IFactory>();
            m_factory.InstanceCreated += OnInstanceCreated;
        }

        public virtual void UnsubscribeToEvents()
        {
            m_factory = GetComponent<IFactory>();
            m_factory.InstanceCreated -= OnInstanceCreated;
        }
#endif
    }
}