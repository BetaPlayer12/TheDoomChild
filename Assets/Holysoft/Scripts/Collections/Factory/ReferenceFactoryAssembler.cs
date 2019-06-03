using Holysoft.Event;
using UnityEngine;

namespace Holysoft.Collections
{
    [RequireComponent(typeof(IReferenceFactory))]
    public abstract class ReferenceFactoryAssembler : MonoBehaviour, IEventListener
    {
        private IReferenceFactory m_factory;

        protected abstract void OnInstanceCreated(object sender, ReferenceInstanceEventArgs eventArgs);

        protected virtual void OnEnable()
        {
            m_factory = GetComponent<IReferenceFactory>();
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
            m_factory = GetComponent<IReferenceFactory>();
            m_factory.InstanceCreated += OnInstanceCreated;
        }

        public virtual void UnsubscribeToEvents()
        {
            m_factory = GetComponent<IReferenceFactory>();
            m_factory.InstanceCreated -= OnInstanceCreated;
        }
#endif
    }
}