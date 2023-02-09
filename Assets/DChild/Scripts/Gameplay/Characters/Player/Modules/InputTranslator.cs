﻿using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class InputTranslator : MonoBehaviour
    {
        public Vector2 m_mousePosition;
        public Vector2 m_mouseDelta;

        //Test
        public float controllerCursorHorizontalInput;
        public float controllerCursorVerticalInput;

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
        public bool blockHeld;

        public bool slashPressed;
        public bool slashHeld;
        public bool earthShakerPressed;
        public bool whipPressed;
        public bool projectileThrowPressed;
        public bool projectileThrowReleased;
        public bool projectileThrowHeld;

        #region BattleAbilities Inputs
        public bool airLungeSlashPressed;
        public bool fireFistPressed;
        public bool reaperHarvestPressed;
        public bool krakenRagePressed;
        public bool finalSlashPressed;
        //public bool finalSlashHeld;
        public bool finalSlashReleased;
        public bool airSlashComboPressed;
        public bool sovereignImpalePressed;
        public bool hellTridentPressed;
        public bool foolsVerdictPressed;
        public bool soulFireBlastPressed;
        public bool edgedFuryPressed;
        public bool edgedFuryReleased;
        public bool ninthCircleSanctionPressed;
        public bool doomsdayKongPressed;
        public bool backDiverPressed;
        public bool barrierPressed;
        public bool barrierReleased;
        public bool fencerFlashPressed;
        public bool diagonalSwordDashPressed;
        public bool championsUprisingPressed;
        public bool eelecktrickPressed;
        public bool eelecktrickReleased;
        #endregion

        private PlayerInput m_input;
        private InputActionMap m_gameplayActionMap;

        public void Disable()
        {
            if (this.enabled)
            {
                Reset();
            }
            if (m_input)
            {
                m_gameplayActionMap.Disable();
            }
            this.enabled = false;
        }

        public void Enable()
        {
            if (this.enabled == false)
            {
                Reset();
            }
            if (m_input)
            {
                m_gameplayActionMap.Enable();
            }
            this.enabled = true;
        }

        private void OnControllerCursorHorizontalInput(InputValue value)
        {
            if (enabled == true)
            {
                controllerCursorHorizontalInput = value.Get<float>();
            }
        }

        private void OnControllerCursorVerticalInput(InputValue value)
        {
            if (enabled == true)
            {
                controllerCursorVerticalInput = value.Get<float>();
            }
        }

        private void OnHorizontalInput(InputValue value)
        {
            if (enabled == true)
            {
                horizontalInput = value.Get<float>();

                if (horizontalInput < 1 && horizontalInput > -1)
                {
                    horizontalInput = 0;
                }
            }
        }

        private void OnVerticalInput(InputValue value)
        {
            if (enabled == true)
            {
                verticalInput = value.Get<float>();
                crouchHeld = value.Get<float>() == -1;

                if (verticalInput < 1 && verticalInput > -1)
                {
                    verticalInput = 0;
                }
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

        private void OnAirLungeSlash(InputValue value)
        {
            if (enabled == true)
            {
                airLungeSlashPressed = value.Get<float>() == 1;
            }
        }

        private void OnFireFist(InputValue value)
        {
            if (enabled == true)
            {
                fireFistPressed = value.Get<float>() == 1;
            }
        }

        private void OnReaperHarvest(InputValue value)
        {
            if (enabled == true)
            {
                reaperHarvestPressed = value.Get<float>() == 1;
            }
        }

        private void OnKrakenRage(InputValue value)
        {
            if (enabled == true)
            {
                krakenRagePressed = value.Get<float>() == 1;
            }
        }

        private void OnFinalSlash(InputValue value)
        {
            if (enabled == true)
            {
                var inputValue = value.Get<float>() == 1;
                finalSlashPressed = inputValue;
            }
        }

        private void OnFinalSlashReleased(InputValue value)
        {
            if (enabled == true)
            {
                finalSlashReleased = value.Get<float>() == 1;
                finalSlashReleased = !finalSlashReleased;
            }
        }

        private void OnAirSlashCombo(InputValue value)
        {
            if (enabled == true)
            {
                airSlashComboPressed = value.Get<float>() == 1;
            }
        }

        private void OnSovereignImpale(InputValue value)
        {
            if (enabled == true)
            {
                sovereignImpalePressed = value.Get<float>() == 1;
            }
        }

        private void OnHellTrident(InputValue value)
        {
            if (enabled == true)
            {
                hellTridentPressed = value.Get<float>() == 1;
            }
        }

        private void OnFoolsVerdict(InputValue value)
        {
            if (enabled == true)
            {
                foolsVerdictPressed = value.Get<float>() == 1;
            }
        }

        private void OnSoulFireBlast(InputValue value)
        {
            if (enabled == true)
            {
                soulFireBlastPressed = value.Get<float>() == 1;
            }
        }

        private void OnEdgedFury(InputValue value)
        {
            if (enabled == true)
            {
                edgedFuryPressed = value.Get<float>() == 1;
            }
        }

        private void OnEdgedFuryReleased(InputValue value)
        {
            if (enabled == true)
            {
                edgedFuryReleased = value.Get<float>() == 1;
                edgedFuryReleased = !edgedFuryReleased;
            }
        }

        private void OnNinthCircleSanction(InputValue value)
        {
            if (enabled == true)
            {
                ninthCircleSanctionPressed = value.Get<float>() == 1;
            }
        }

        private void OnDoomsdayKong(InputValue value)
        {
            if (enabled == true)
            {
                doomsdayKongPressed = value.Get<float>() == 1;
            }
        }

        private void OnBackDiver(InputValue value)
        {
            if (enabled == true)
            {
                backDiverPressed = value.Get<float>() == 1;
            }
        }

        private void OnBarrier(InputValue value)
        {
            if (enabled == true)
            {
                barrierPressed = value.Get<float>() == 1;
            }
        }

        private void OnBarrierReleased(InputValue value)
        {
            if (enabled == true)
            {
                barrierReleased = value.Get<float>() == 1;
                barrierReleased = !barrierReleased;
            }
        }

        private void OnFencerFlash(InputValue value)
        {
            if (enabled == true)
            {
                fencerFlashPressed = value.Get<float>() == 1;
            }
        }

        private void OnDiagonalSwordDash(InputValue value)
        {
            if (enabled == true)
            {
                diagonalSwordDashPressed = value.Get<float>() == 1;
            }
        }

        private void OnChampionsUprising(InputValue value)
        {
            if (enabled == true)
            {
                championsUprisingPressed = value.Get<float>() == 1;
            }
        }

        private void OnEelecktrick(InputValue value)
        {
            if (enabled == true)
            {
                var inputValue = value.Get<float>() == 1;
                eelecktrickPressed = inputValue;
                Debug.Log("Eelecktrick Pressed");
            }
        }

        private void OnEelecktrickReleased(InputValue value)
        {
            if (enabled == true)
            {
                eelecktrickReleased = value.Get<float>() == 1;
                eelecktrickReleased = !eelecktrickReleased;
                Debug.Log("Eelecktrick Released");
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

        private void OnProjectileThrow(InputValue value)
        {
            if (enabled == true)
            {
                var inputValue = value.Get<float>() == 1;
                if (inputValue == false)
                {
                    if (projectileThrowHeld == true)
                    {
                        projectileThrowReleased = true;
                    }
                }
                projectileThrowPressed = inputValue;
                projectileThrowHeld = inputValue;
            }
        }

        private void OnQuickItemUse(InputValue value)
        {
            if (enabled == true)
            {
                projectileThrowHeld = value.Get<float>() == 1;
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

        private void OnBlock(InputValue value)
        {
            if (enabled == true)
            {
                blockHeld = value.Get<float>() == 1;
            }
        }

        private void Awake()
        {
            m_input = GetComponent<PlayerInput>();
            if (m_input)
            {
                m_gameplayActionMap = m_input.actions.FindActionMap("Gameplay");
            }
            m_mousePosition = Mouse.current.position.ReadValue();
        }

        private void Update()
        {
            var mousePosition = Mouse.current.position.ReadValue();
            m_mouseDelta = Mouse.current.delta.ReadValue();
            m_mousePosition = mousePosition;
            m_mousePosition += new Vector2(controllerCursorHorizontalInput, controllerCursorVerticalInput).normalized;
        }

        private void LateUpdate()
        {
            controllerCursorHorizontalInput = 0;
            controllerCursorVerticalInput = 0;

            dashPressed = false;
            jumpPressed = false;
            levitatePressed = false;
            shadowMorphPressed = false;

            interactPressed = false;
            grabPressed = false;

            slashPressed = false;
            earthShakerPressed = false;
            whipPressed = false;
            projectileThrowPressed = false;
            projectileThrowReleased = false;

            airLungeSlashPressed = false;
            fireFistPressed = false;
            reaperHarvestPressed = false;
            krakenRagePressed = false;
            finalSlashPressed = false;
            finalSlashReleased = false;
            airSlashComboPressed = false;
            sovereignImpalePressed = false;
            hellTridentPressed = false;
            foolsVerdictPressed = false;
            soulFireBlastPressed = false;
            edgedFuryPressed = false;
            edgedFuryReleased = false;
            ninthCircleSanctionPressed = false;
            doomsdayKongPressed = false;
            backDiverPressed = false;
            barrierPressed = false;
            barrierReleased = false;
            fencerFlashPressed = false;
            diagonalSwordDashPressed = false;
            championsUprisingPressed = false;
            eelecktrickPressed = false;
            eelecktrickReleased = false;
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
            projectileThrowPressed = false;
            projectileThrowHeld = false;
            projectileThrowReleased = false;

            airLungeSlashPressed = false;
            fireFistPressed = false;
            reaperHarvestPressed = false;
            krakenRagePressed = false;
            finalSlashPressed = false;
            finalSlashReleased = false;
            airSlashComboPressed = false;
            sovereignImpalePressed = false;
            hellTridentPressed = false;
            foolsVerdictPressed = false;
            soulFireBlastPressed = false;
            edgedFuryPressed = false;
            edgedFuryReleased = false;
            ninthCircleSanctionPressed = false;
            doomsdayKongPressed = false;
            backDiverPressed = false;
            barrierPressed = false;
            barrierReleased = false;
            fencerFlashPressed = false;
            diagonalSwordDashPressed = false;
            championsUprisingPressed = false;
            eelecktrickPressed = false;
            eelecktrickReleased = false;
        }
    }
}
