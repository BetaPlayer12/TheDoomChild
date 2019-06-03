using Holysoft.Event;
using UnityEngine;

namespace Holysoft.Collections
{
    public class ReferenceInstanceEventArgs : UnityEventActionArgs<GameObject>
    {
        public int referenceIndex;
    }

    public interface IReferenceFactory
    {
        event EventAction<ReferenceInstanceEventArgs> InstanceCreated;
    }
}