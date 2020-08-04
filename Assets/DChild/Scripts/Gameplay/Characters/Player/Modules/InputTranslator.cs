using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class InputTranslator : MonoBehaviour
    {
        public float horizontalInput;
        public bool crouchHeld;
        public bool dashPressed;

        private void OnHorizontalInput(InputValue value)
        {
            horizontalInput = value.Get<float>();
        }
    }
}
