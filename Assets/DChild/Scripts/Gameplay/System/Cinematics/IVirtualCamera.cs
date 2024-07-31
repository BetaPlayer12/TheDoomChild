using Cinemachine;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public interface IVirtualCamera
    {

        string name { get; }
        Vector3 currentOffset { get; }
        void Activate();
        void Deactivate();

        void ApplyOffset(Vector3 offset);

        CinemachineBasicMultiChannelPerlin noiseModule { get; }
    }
}