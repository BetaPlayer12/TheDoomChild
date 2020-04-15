﻿using DChild.Gameplay;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public enum TransitionType
    {
        Enter,
        PostEnter,
        Exit,
        PostExit
    }

    public interface ISwitchHandle
    {
        void DoSceneTransition(Character character, TransitionType type);
        float transitionDelay { get; }

        bool needsButtonInteraction { get; }
        Vector3 promptPosition { get; }
    } 
}
