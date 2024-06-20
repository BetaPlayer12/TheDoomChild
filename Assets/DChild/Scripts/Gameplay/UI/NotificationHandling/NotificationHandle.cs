using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.UI
{
    public abstract class NotificationHandle<T> : SerializedMonoBehaviour, INotificationHandle where T : System.Enum
    {
        #region SubHandles
        protected abstract class SubHandle
        {
            [SerializeField]
            private int m_priority;

            public int priority => m_priority;

            public abstract void AddListenerToOnNotificationHidden(UnityAction action);
        }

        [HideReferenceObjectPicker]
        protected class SubHandle<U> : SubHandle where U : NotificationUI
        {
            [SerializeField]
            private U m_ui;

            public U ui => m_ui;

            public override void AddListenerToOnNotificationHidden(UnityAction action)
            {
                m_ui.container.OnHiddenCallback.Event.AddListener(action);
            }
        }

        #endregion

        protected class NotificationRequest<T> where T : System.Enum
        {
            public NotificationRequest(int priority, T type, object referenceData = null)
            {
                this.priority = priority;
                this.type = type;
                this.referenceData = referenceData;
            }

            public int priority { get; }
            public T type { get; }
            public object referenceData { get; }
        }

        [SerializeField]
        private int m_priority;

        protected List<SubHandle> m_subHandles;
        protected List<NotificationRequest<T>> m_requests;

        public int priority => m_priority;

        protected abstract void InitializeSubHandles(List<SubHandle> subHandles);
        protected abstract void HandleNotification(NotificationRequest<T> notificationRequest);

        public void AddListenerToOnNotificationHidden(UnityAction action)
        {
            for (int i = 0; i < m_subHandles.Count; i++)
            {
                m_subHandles[i].AddListenerToOnNotificationHidden(action);
            }
        }

        public void Initialize()
        {
            if (m_subHandles == null)
            {
                m_subHandles = new List<SubHandle>();
                InitializeSubHandles(m_subHandles);

            }

            if (m_requests == null)
            {
                m_requests = new List<NotificationRequest<T>>();
            }
            m_requests.Sort(SortPriorityComparison);
        }

        public void HandleNextNotification()
        {
            var request = m_requests[0];
            HandleNotification(request);
            m_requests.RemoveAt(0);
        }

        public bool HasNotifications() => m_requests.Count > 0;

        public void RemoveAllQueuedNotifications()
        {
            m_requests.Clear();
        }

        public bool HasNotificationFor(object requestData)
        {
            for (int i = 0; i < m_requests.Count; i++)
            {
                if (m_requests[i].referenceData == requestData)
                    return true;
            }
            return false;
        }

        protected void AddNotificationRequest(NotificationRequest<T> request)
        {
            m_requests.Add(request);
            m_requests.Sort(SortPriorityComparison);
        }

        private int SortPriorityComparison(NotificationRequest<T> x, NotificationRequest<T> y)
        {
            if (x.priority == y.priority)
                return 0;
            return x.priority < y.priority ? 1 : -1;
        }

     

    }
}