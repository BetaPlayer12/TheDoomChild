using System;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Holysoft.Pooling;

namespace DChild.Gameplay.Systems
{
    public abstract class Loot : PoolableObject
    {
        public abstract void PickUp();
    }
}