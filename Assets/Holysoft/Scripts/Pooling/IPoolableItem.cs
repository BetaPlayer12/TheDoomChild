using Holysoft.Event;
using System;
using System.Collections;
using UnityEngine;

namespace Holysoft.Pooling
{
    [System.Serializable]
    public struct PoolItemEventArgs : IEventActionArgs
    {
        public IPoolableItem item { get; }
        public bool hasTransform { get; }
        private Transform m_transform;

        public PoolItemEventArgs(IPoolableItem item, Transform m_transform) : this()
        {
            this.item = item;
            this.m_transform = m_transform;
            hasTransform = m_transform != null;
        }

        public void StoreToPool(Transform parent)
        {
            m_transform.parent = parent;
        }
    }

    [CreateAssetMenu(fileName = "PoolableItemData",menuName = "Holysoft/Basic Pool Data")]
    public class PoolableItemData : ScriptableObject
    {

    }

    public interface IPoolableItem
    {
        PoolableItemData poolableItemData { get; }
        event EventAction<PoolItemEventArgs> PoolRequest;
        event EventAction<PoolItemEventArgs> InstanceDestroyed;
        void DestroyInstance();
    }

   
}