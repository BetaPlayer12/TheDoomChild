namespace Holysoft.Event
{
    public struct PrimitiveEventActionArgs<T> : IEventActionArgs
    {
        public T value { get; private set; }
        public void SetValue(T newValue) => value = newValue;
    }
}