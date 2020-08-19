using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class InputTranslator : MonoBehaviour
    {
        public float horizontalInput;
        public float verticalInput;
        public bool crouchHeld;
        public bool dashPressed;
        public bool jumpPressed;
        public bool jumpHeld;
        public bool levitatePressed;
        public bool levitateHeld;

        public bool interactPressed;

        public bool slashPressed;
        public bool slashHeld;
        public bool earthShakerPressed;

        private PlayerInput m_input;

        public void Disable()
        {
            if (this.enabled)
            {
                Reset();
            }
            m_input.enabled = false;
            this.enabled = false;
        }

        public void Enable()
        {
            if(this.enabled == false)
            {
                Reset();
            }
            m_input.enabled = true;
            this.enabled = true;
        }

        private void OnHorizontalInput(InputValue value)
        {
            horizontalInput = value.Get<float>();
        }
        private void OnVerticalInput(InputValue value)
        {
            verticalInput = value.Get<float>();
        }

        private void OnCrouch(InputValue value)
        {
            crouchHeld = value.Get<float>() == 1;
        }

        private void OnDash(InputValue value)
        {
            dashPressed = value.Get<float>() == 1;
        }

        private void OnJump(InputValue value)
        {
            var isTrue = value.Get<float>() == 1;
            jumpPressed = isTrue;
            jumpHeld = isTrue;
        }

        private void OnLevitate(InputValue value)
        {
            var isTrue = value.Get<float>() == 1;
            levitatePressed = isTrue;
            levitateHeld = isTrue;
        }

        private void OnSlash(InputValue value)
        {
            slashPressed = value.Get<float>() == 1;
        }

        private void OnSlashHeld(InputValue value)
        {
            slashHeld = value.Get<float>() == 1;
        }

        private void OnSlashReleased(InputValue value)
        {
            slashHeld = false;
        }

        private void OnEarthShaker(InputValue value)
        {
            earthShakerPressed = value.Get<float>() == 1;
        }

        private void OnInteract(InputValue value)
        {
            interactPressed = value.Get<float>() == 1;
        }

        private void Awake()
        {
            m_input = GetComponent<PlayerInput>();
        }


        private void LateUpdate()
        {
            dashPressed = false;
            jumpPressed = false;
            levitatePressed = false;

            interactPressed = false;

            slashPressed = false;
            earthShakerPressed = false;
        }

        private void Reset()
        {
            horizontalInput = 0;
            verticalInput = 0;
            crouchHeld = false;
            dashPressed = false;
            jumpPressed = false;
            jumpHeld = false;
            levitatePressed = false;
            levitateHeld = false;

            interactPressed = false;

            slashPressed = false;
            slashHeld = false;
            earthShakerPressed = false;
        }
    }
}
