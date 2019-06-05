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
        [ReadOnly]
        private string m_fxName;

        public event EventAction<PoolItemEventArgs> PoolRequest;
        public event EventAction<PoolItemEventArgs> InstanceDestroyed;

        public string fxName => m_fxName;

        public abstract void Play();

        protected void FXValidate()
        {
            if (Application.isPlaying == false)
            {
                m_fxName = gameObject.name;
            }
        }

        public void DestroyInstance()
        {
            InstanceDestroyed?.Invoke(this, new PoolItemEventArgs(this, transform));
            Destroy(gameObject);
        }

        protected void CallPoolRequest() => PoolRequest?.Invoke(this, new PoolItemEventArgs(this, transform));
    }

}