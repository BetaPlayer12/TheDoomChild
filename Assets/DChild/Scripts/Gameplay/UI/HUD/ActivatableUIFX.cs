using UnityEngine;

namespace DChild.Gameplay.UI
{
    public abstract class ActivatableUIFX : MonoBehaviour
    {
        public abstract bool isFXEnabled { get; }
        public abstract void Enable();
        public abstract void Disable();
    }
}