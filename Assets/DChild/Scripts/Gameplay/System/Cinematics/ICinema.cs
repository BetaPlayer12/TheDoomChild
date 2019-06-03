using Cinemachine;
using DChild.Gameplay.Cinematics.Cameras;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public interface ICinema
    {
        Camera mainCamera { get; }
        PlayerCamera camera { get; }
        void SetDefaultCam(IVirtualCamera vCam);
        void SetBlend(CinemachineBlendDefinition.Style style, float duration);
        void TransistionTo(IVirtualCamera vCam);
        void TransistionToDefaultCamera();
        void Register(ITrackingCamera trackingCamera);
        void Unregister(ITrackingCamera trackingCamera);
    }

}