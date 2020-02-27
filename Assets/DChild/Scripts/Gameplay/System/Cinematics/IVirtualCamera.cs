using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public interface IVirtualCamera
    {
        Vector3 currentOffset { get; }
        void Activate();
        void Deactivate();

        void ApplyOffset(Vector3 offset);
    }
}