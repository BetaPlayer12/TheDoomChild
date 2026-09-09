using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Inputs
{
    public class PlayerInputReaderBridge : MonoBehaviour
    {
        [SerializeField]
        private InputReader m_reader;

        #region Underworld Actions
        //Underworld Actions
        public void OnVector2(InputAction.CallbackContext context)
        {
            m_reader.OnVector2(context);
        }


        public void OnJump(InputAction.CallbackContext context)
        {
            m_reader.OnJump(context);
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            m_reader.OnDash(context);
        }

        public void OnGrab(InputAction.CallbackContext context)
        {
            m_reader.OnGrab(context);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            m_reader.OnInteract(context);
        }

        public void OnMouseDelta(InputAction.CallbackContext context)
        {
            m_reader.OnMouseDelta(context);
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            m_reader.OnPause(context);
        }

        public void OnTeleportToOverworld(InputAction.CallbackContext context)
        {
            m_reader.OnTeleportToOverworld(context);
        }
        public void OnTeleportToMordenThroneRoom(InputAction.CallbackContext context)
        {
            m_reader.OnTeleportToMordenThroneRoom(context);
        }

        public void OnQuickItemCycle(InputAction.CallbackContext context)
        {
            m_reader.OnQuickItemCycle(context);
        }

        public void OnQuickItemUse(InputAction.CallbackContext context)
        {
            m_reader.OnQuickItemUse(context);
        }

        public void OnSlash(InputAction.CallbackContext context)
        {
            m_reader.OnSlash(context);
        }

        public void OnStore(InputAction.CallbackContext context)
        {
            m_reader.OnStore(context);
        }

        #endregion

        #region Primary Skills
        public void OnShadowMorph(InputAction.CallbackContext context)
        {
            m_reader.OnShadowMorph(context);
        }

        public void OnWhip(InputAction.CallbackContext context)
        {
            m_reader.OnWhip(context);
        }

        public void OnProjectileThrow(InputAction.CallbackContext context)
        {
            m_reader.OnProjectileThrow(context);
        }

        public void OnLevitate(InputAction.CallbackContext context)
        {
            m_reader.OnLevitate(context);
        }

        public void OnSwordThrust(InputAction.CallbackContext context)
        {
            m_reader.OnSwordThrust(context);
        }
        #endregion

        #region Combat Arts
        //Combat Arts
        public void OnAirSlashCombo(InputAction.CallbackContext context)
        {
            m_reader.OnAirSlashCombo(context);
        }

        public void OnBackDiver(InputAction.CallbackContext context)
        {
            m_reader.OnBackDiver(context);
        }

        public void OnBarrier(InputAction.CallbackContext context)
        {
            m_reader.OnBarrier(context);
        }
        public void OnEarthShaker(InputAction.CallbackContext context)
        {
            m_reader.OnEarthShaker(context);
        }

        public void OnReaperHarvest(InputAction.CallbackContext context)
        {
            m_reader.OnReaperHarvest(context);
        }

        public void OnSoulFireBlast(InputAction.CallbackContext context)
        {
            m_reader.OnSoulFireBlast(context);
        }

        public void OnSovereignImpale(InputAction.CallbackContext context)
        {
            m_reader.OnSovereignImpale(context);
        }

        public void OnTeleportingSkull(InputAction.CallbackContext context)
        {
            m_reader.OnTeleportingSkull(context);
        }

        public void OnDiagonalSwordDash(InputAction.CallbackContext context)
        {
            m_reader.OnDiagonalSwordDash(context);
        }

        public void OnEdgedFury(InputAction.CallbackContext context)
        {
            m_reader.OnEdgedFury(context);
        }

        public void OnHellTrident(InputAction.CallbackContext context)
        {
            m_reader.OnHellTrident(context);
        }

        public void OnIcarusWings(InputAction.CallbackContext context)
        {
            m_reader.OnIcarusWings(context);
        }
        #endregion

        #region Overworld Controls
        public void OnMove(InputAction.CallbackContext context)
        {
            m_reader.OnMove(context);
        }
        #endregion

        #region UI Controls
        public void OnNavigate(InputAction.CallbackContext context)
        {
            m_reader.OnNavigate(context);
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            m_reader.OnSubmit(context);
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            m_reader.OnCancel(context);
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            m_reader.OnPoint(context);
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            m_reader.OnClick(context);
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            m_reader.OnScrollWheel(context);
        }

        public void OnResume(InputAction.CallbackContext context)
        {
            m_reader.OnResume(context);
        }

        public void OnCycleTab(InputAction.CallbackContext context)
        {
            m_reader.OnCycleTab(context);
        }

        public void OnCycleSubTab(InputAction.CallbackContext context)
        {
            m_reader.OnCycleSubTab(context);
        }

        public void OnDeleteSave(InputAction.CallbackContext context)
        {
            m_reader.OnDeleteSave(context);
        }
        public void OnToggleMapLegend(InputAction.CallbackContext context)
        {
            m_reader.OnToggleMapLegend(context);
        }
        public void OnHoldToSkip(InputAction.CallbackContext context)
        {
            m_reader.OnHoldToSkip(context);
        }

        #endregion

        #region Army Battle Controls
        public void OnSelectCommand(InputAction.CallbackContext context)
        {
            m_reader.OnSelectCommand(context);
        }
        #endregion
    }
}

