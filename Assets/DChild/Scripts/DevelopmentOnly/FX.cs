using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    public abstract class FX : MonoBehaviour, IPoolItem
    {
        [SerializeField]
        [ReadOnly]
        private string m_fxName;
        public string fxName => m_fxName;

        public abstract void Play();

        public void DestroyItem() => Destroy(gameObject);

        public void SetParent(Transform parent) => transform.parent = parent;

        protected void FXValidate()
        {
            if (Application.isPlaying == false)
            {
                m_fxName = gameObject.name;
            }
        }
    }

}