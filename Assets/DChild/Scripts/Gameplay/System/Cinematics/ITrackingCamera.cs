﻿using DChild.Gameplay.Cinematics.Cameras;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public interface ITrackingCamera
    {
        void Track(Transform transform);
        CinemachineNoise noiseModule { get; }
    }
}