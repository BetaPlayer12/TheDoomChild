using DChild.Gameplay.Pooling;
using Holysoft.Event;
using System;
using UnityEngine;

namespace DChild.Gameplay
{
    public interface ISpawnable : IPoolItem
    {
        void SpawnAt(Vector2 position, Quaternion rotation);
        Type GetType();
    }

    public interface IBoundSpawnable : ISpawnable
    {
        event EventAction<SpawnableEventArgs> Pool;
        GameObject gameObject { get; }
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