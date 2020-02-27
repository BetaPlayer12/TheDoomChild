using Cinemachine;
using DChild.Gameplay.Cinematics.Cameras;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public interface ICinema
    {
        Camera mainCamera { get; }
        void EnableCameraShake(bool enable);
        void ClearLists();
        void SetDefaultCam(IVirtualCamera vCam);
        void TransistionTo(IVirtualCamera vCam);
        void TransistionToDefaultCamera();
        void AllowTracking(ITrackingCamera trackingCamera);
        void RemoveTracking(ITrackingCamera trackingCamera);
        void Register(ITrackingCamera trackingCamera);
        void Unregister(ITrackingCamera trackingCamera);

        void ApplyLookAhead(Cinema.LookAhead look);
    }

}