using UnityEngine;

namespace Holysoft.Event
{
    public class UnityEventActionArgs<T> : IEventActionArgs where T : Object
    {
        public T value { get; private set; }
        public void SetValue(T newValue) => value = newValue;
    }
}