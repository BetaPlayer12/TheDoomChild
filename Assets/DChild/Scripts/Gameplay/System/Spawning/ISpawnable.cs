using DChild.Gameplay.Pooling;
using Holysoft.Event;
using Holysoft.Pooling;
using System;
using UnityEngine;

namespace DChild.Gameplay
{
    public interface ISpawnable : IPoolableItem
    {
        void SpawnAt(Vector2 position, Quaternion rotation);
        Type GetType();
    }

    public struct SpawnableEventArgs : IEventActionArgs
    {
        public ISpawnable spawnable { get; }
        public SpawnableEventArgs(ISpawnable spawnable)
        {
            this.spawnable = spawnable;
        }
    }
}