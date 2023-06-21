using UnityEngine;

namespace DChildDebug.Spine.Tests
{
    public interface IStressTestInstantiator<T> where T: Object
    {
        void DestroyAllInstances();
        void Instantiate(T skeletonData, int index);
    }
}