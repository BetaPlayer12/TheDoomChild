using Holysoft.Event;
using Sirenix.Utilities;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class Collision2DEventSender : MonoBehaviour
    {
        [SerializeField]
        private bool m_enableOnEnterEvent;
        [SerializeField]
        private bool m_enableOnExitEvent;

        public event EventAction<CollisionEventActionArgs> OnEnter;
        public event EventAction<CollisionEventActionArgs> OnExit;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_enableOnEnterEvent)
            {
                using (Cache<CollisionEventActionArgs> cacheEvent = Cache<CollisionEventActionArgs>.Claim())
                {
                    cacheEvent.Value.Set(collision);
                    OnEnter?.Invoke(this, cacheEvent.Value);
                    Cache<CollisionEventActionArgs>.Release(cacheEvent);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (m_enableOnExitEvent)
            {
                using (Cache<CollisionEventActionArgs> cacheEvent = Cache<CollisionEventActionArgs>.Claim())
                {
                    cacheEvent.Value.Set(collision);
                    OnExit?.Invoke(this, cacheEvent.Value);
                    Cache<CollisionEventActionArgs>.Release(cacheEvent);
                }
            }
        }
    }
}
