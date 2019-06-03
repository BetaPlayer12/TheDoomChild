namespace Holysoft.Event
{
    namespace Collections
    {
        public interface IGlobalEventAction<T> where T :  IEventActionArgs
        {
            void Subscribe(EventAction<T> listener);

            void Unsubscribe(EventAction<T> listener);

            T Raise(object sender, T eventActionArgs);

            EventAction<T> GetEventAction();
        }
    }
}