using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class InputTranslator : MonoBehaviour
    {
        public Vector2 m_mousePosition;
        public Vector2 m_mouseDelta;

        public float horizontalInput;
        public float verticalInput;
        public bool crouchHeld;
        public bool dashPressed;
        public bool jumpPressed;
        public bool jumpHeld;
        public bool levitatePressed;
        public bool levitateHeld;
        public bool shadowMorphPressed;

        public bool interactPressed;
        public bool grabPressed;
        public bool grabHeld;

        public bool slashPressed;
        public bool slashHeld;
        public bool earthShakerPressed;
        public bool whipPressed;
        public bool skullThrowPressed;
        public bool skullThrowReleased;
        public bool skullThrowHeld;

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
            if (this.enabled == false)
            {
                Reset();
            }
            m_input.enabled = true;
            this.enabled = true;
        }

        private void OnHorizontalInput(InputValue value)
        {
            if (enabled == true)
            {
                horizontalInput = value.Get<float>();
            }
        }

        private void OnVerticalInput(InputValue value)
        {
            if (enabled == true)
            {
                verticalInput = value.Get<float>();
            }
        }

        private void OnCrouch(InputValue value)
        {
            if (enabled == true)
            {
                crouchHeld = value.Get<float>() == 1;
            }
        }

        private void OnDash(InputValue value)
        {
            if (enabled == true)
            {
                dashPressed = value.Get<float>() == 1;
            }
        }

        private void OnJump(InputValue value)
        {
            if (enabled == true)
            {
                var isTrue = value.Get<float>() == 1;
                jumpPressed = isTrue;
                jumpHeld = isTrue;
            }
        }

        private void OnLevitate(InputValue value)
        {
            if (enabled == true)
            {
                var isTrue = value.Get<float>() == 1;
                levitatePressed = isTrue;
                levitateHeld = isTrue;
            }
        }

        private void OnShadowMorph(InputValue value)
        {
            if (enabled == true)
            {
                shadowMorphPressed = value.Get<float>() == 1;
            }
        }

        private void OnSlash(InputValue value)
        {
            if (enabled == true)
            {
                slashPressed = value.Get<float>() == 1;
            }
        }

        private void OnSlashHeld(InputValue value)
        {
            if (enabled == true)
            {
                slashHeld = value.Get<float>() == 1;
            }
        }

        private void OnSlashReleased(InputValue value)
        {
            if (enabled == true)
            {
                slashHeld = false;
            }
        }

        private void OnEarthShaker(InputValue value)
        {
            if (enabled == true)
            {
                earthShakerPressed = value.Get<float>() == 1;
            }
        }

        private void OnWhip(InputValue value)
        {
            if (enabled == true)
            {
                whipPressed = value.Get<float>() == 1;
            }
        }

        private void OnSkullThrow(InputValue value)
        {
            if (enabled == true)
            {
                var inputValue = value.Get<float>() == 1;
                if (inputValue == false)
                {
                    if (skullThrowHeld == true)
                    {
                        skullThrowReleased = true;
                    }
                }
                skullThrowPressed = inputValue;
                skullThrowHeld = inputValue;
            }
        }

        private void OnQuickItemUse(InputValue value)
        {
            if (enabled == true)
            {
                skullThrowHeld = value.Get<float>() == 1;
            }
        }

        private void OnInteract(InputValue value)
        {
            if (enabled == true)
            {
                interactPressed = value.Get<float>() == 1;
            }
        }

        private void OnGrab(InputValue value)
        {
            if (enabled == true)
            {
                var isTrue = value.Get<float>() == 1;
                grabPressed = isTrue;
                grabHeld = isTrue;
            }
        }

        private void Awake()
        {
            m_input = GetComponent<PlayerInput>();
            m_mousePosition = Input.mousePosition;
        }

        private void Update()
        {
            m_mouseDelta = (Vector2)Input.mousePosition - m_mousePosition;
            m_mousePosition = Input.mousePosition;
        }

        private void LateUpdate()
        {
            dashPressed = false;
            jumpPressed = false;
            levitatePressed = false;
            shadowMorphPressed = false;

            interactPressed = false;
            grabPressed = false;

            slashPressed = false;
            earthShakerPressed = false;
            whipPressed = false;
            skullThrowPressed = false;
            skullThrowReleased = false;
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
            shadowMorphPressed = false;

            interactPressed = false;
            grabPressed = false;
            grabHeld = false;

            slashPressed = false;
            slashHeld = false;
            earthShakerPressed = false;
            whipPressed = false;
            skullThrowPressed = false;
            skullThrowHeld = false;
            skullThrowReleased = false;
        }
    }
}
