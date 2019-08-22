using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay
{
    public struct ObjectIDEventArgs : IEventActionArgs
    {
        public int ID { get; }

        public ObjectIDEventArgs(Object instance)
        {
            ID = instance.GetInstanceID();
        }
    }
}