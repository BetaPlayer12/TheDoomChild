using DChild.Gameplay.Pooling;
using Holysoft.Pooling;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public abstract class Loot : PoolableObject
    {
        public abstract void PickUp();
    }
}