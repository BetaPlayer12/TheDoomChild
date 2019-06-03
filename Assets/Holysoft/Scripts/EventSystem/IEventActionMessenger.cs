namespace Holysoft.Event
{
    namespace Collections
    {
        public interface IEventActionMessenger
        {
            void Subscribe<T, U>(EventAction<U> listener) where T :   IGlobalEventAction<U>, new() where U :  IEventActionArgs;

            void Unsubscribe<T, U>(EventAction<U> listener) where T :  IGlobalEventAction<U>, new() where U :  IEventActionArgs;

            void Raise<T, U>(object sender, U eventArgs) where T :  IGlobalEventAction<U>, new() where U :  IEventActionArgs;
        }
    }
}