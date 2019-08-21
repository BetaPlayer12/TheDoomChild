using DChild.Gameplay.Characters;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using Holysoft.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    public abstract class FX : MonoBehaviour, IPoolableItem
    {
        [SerializeField]
        private PoolableItemData m_poolableItemData;

        public PoolableItemData poolableItemData => m_poolableItemData;

        public event EventAction<EventActionArgs> Done;
        public event EventAction<PoolItemEventArgs> PoolRequest;
        public event EventAction<PoolItemEventArgs> InstanceDestroyed;

        public abstract void Play();
        public abstract void Stop();
        public abstract void Pause();

        public abstract void SetFacing(HorizontalDirection horizontalDirection);

        public void DestroyInstance()
        {
            InstanceDestroyed?.Invoke(this, new PoolItemEventArgs(this, transform));
            Destroy(gameObject);
        }


        protected void CallPoolRequest() => PoolRequest?.Invoke(this, new PoolItemEventArgs(this, transform));
        protected void CallFXDone() => Done?.Invoke(this, EventActionArgs.Empty);
    }

}