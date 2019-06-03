using Holysoft.Event;

namespace Holysoft
{
    public struct DestroyableObjectEventArgs : IEventActionArgs
    {
        public DestroyableObjectEventArgs(IDestroyableObject destroyedObject) : this()
        {
            this.destroyedObject = destroyedObject;
        }

        public IDestroyableObject destroyedObject { get; }
    }

    public interface IDestroyableObject
    {
        event EventAction<DestroyableObjectEventArgs> ObjectDestroy;
    }
}