﻿using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public interface IButtonToInteract : IInteractable
    {
        bool showPrompt { get; }
        Vector3 promptPosition { get; }

        Transform transform { get; }

        void Interact(Character character);
    }
}