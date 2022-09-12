using Sirenix.OdinInspector;

namespace DChild.Gameplay.Characters.AI
{
    [System.Serializable, HideLabel]
    public struct StateHandle<T> where T : System.Enum
    {
        [ShowInInspector]
        public T currentState { get; private set; }
        private T m_waitState;
        private T m_queuedState;

        public StateHandle(T currentState, T waitState) : this()
        {
            this.currentState = currentState;
            this.m_waitState = waitState;
        }

        public void OverrideState(T state) => currentState = state;
        public void SetState(T state)
        {
            if (currentState.Equals(m_waitState))
            {
                m_queuedState = state;
            }
            else
            {
                currentState = state;
            }
        }

        public void Wait(T queuedState)
        {
            currentState = m_waitState;
            m_queuedState = queuedState;
        }

        public void QueueState(T queueState)
        {
            m_queuedState = queueState;
        }

        public void ApplyQueuedState() => currentState = m_queuedState;
    }
}