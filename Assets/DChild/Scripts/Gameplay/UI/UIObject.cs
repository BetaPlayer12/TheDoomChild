using Holysoft;
using DChild.Gameplay.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.UI
{
    public abstract class UIObject : Actor, IPoolItem
    {
        public abstract void DestroyItem();
        public abstract void SetParent(Transform parent);
    }

}