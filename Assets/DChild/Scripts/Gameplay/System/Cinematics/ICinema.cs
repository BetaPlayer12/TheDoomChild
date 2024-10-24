﻿using Cinemachine;
using DChild.Gameplay.Cinematics.Cameras;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public interface ICinema
    {
        Camera mainCamera { get; }
        CinemachineBrain currentBrain { get; }

        void SetMainCamera(Camera camera);
        void EnableCameraShake(bool enable);
        void TransistionTo(IVirtualCamera vCam);

        void ResolveCamTransistion(IVirtualCamera vCam);

        void AllowTracking(ITrackingCamera trackingCamera);
        void RemoveTracking(ITrackingCamera trackingCamera);
        void Register(ITrackingCamera trackingCamera);
        void Unregister(ITrackingCamera trackingCamera);

        void SetCameraPeekConfiguration(CameraPeekConfiguration configuration);
        void ApplyCameraPeekMode(CameraPeekMode look);

        void SetCameraShake(float amplitude, float frequency);

        void SetCameraShakeProfile(CameraShakeType shakeType,bool onNextShakeOnly = false);
    }

}