using Holysoft;
using DChild.Gameplay.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holysoft.Event;

namespace DChild.UI
{
    public abstract class UIObject : Actor, IPoolableItem
    {
        public event EventAction<PoolItemEventArgs> PoolRequest;

        public abstract void DestroyItem();
        public abstract void SetParent(Transform parent);
    }

}