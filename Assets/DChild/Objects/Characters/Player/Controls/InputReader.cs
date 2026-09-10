using DarkTonic.MasterAudio.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static UnityEngine.InputSystem.InputAction;

namespace DChild.Inputs
{

    [CreateAssetMenu(menuName = "Input Reader")]
    public class InputReader : ScriptableObject
    {
        [SerializeField]
        private PlayerControls m_playerControls;

        private void OnEnable()
        {
            if (m_playerControls == null)
            {
                m_playerControls = new PlayerControls();

                //m_playerControls.Underworld.SetCallbacks(this);
                //m_playerControls.Overworld.SetCallbacks(this);
                //m_playerControls.UI.SetCallbacks(this);
                //m_playerControls.ArmyBattle.SetCallbacks(this);
            }
        }

        public void Disable()
        {
            //var input = GameObject.FindObjectOfType<PlayerInput>();

            //input?.DeactivateInput();

            m_playerControls.Underworld.Disable();
            m_playerControls.Overworld.Disable();
            m_playerControls.UI.Enable();
        }

        public void Enable()
        {
            //var input = GameObject.FindObjectOfType<PlayerInput>();

            //input?.ActivateInput();

            m_playerControls.Underworld.Enable();
            m_playerControls.Overworld.Enable();
            m_playerControls.UI.Disable();

        }

        #region Input Events
        #region Underworld Input
        public event Action<Vector2> Vector2InputPerformedEvent;
        public event Action<Vector2> Vector2CancelledInputEvent;
        public event Action JumpPerformedEvent;
        public event Action JumpStartedEvent;
        public event Action JumpCancelledEvent;
        public event Action PauseStartedEvent;
        public event Action StoreStartedEvent;
        public event Action DashStartedEvent;
        public event Action LevitatePerformedEvent;
        public event Action LevitateStartedEvent;
        public event Action LevitateCancelledEvent;
        public event Action InteractStartedEvent;
        public event Action ShadowMorphStartedEvent;
        public event Action SlashTappedEvent;
        public event Action SlashPressedEvent;
        public event Action SlashHeldEvent;
        public event Action SlashStartedEvent;
        public event Action SlashCancelledEvent;
        public event Action WhipPerformedEvent;
        public event Action WhipCancelledEvent;
        public event Action<float> CycleQuickItemsStartedEvent;
        public event Action UseQuickItemTappedEvent;
        public event Action UseQuickItemHeldEvent;
        public event Action UseQuickItemCancelledEvent;
        public event Action ProjectileThrowStartedEvent;
        public event Action ProjectileThrowCancelledEvent;
        public event Action ProjectileThrowHeldEvent;
        public event Action ProjectileThrowTappedEvent;
        public event Action GrabStartedEvent;
        public event Action GrabCancelledEvent;
        public event Action<Vector2> MouseDeltaPerformedEvent;
        public event Action SwordThrustPerformedEvent;
        public event Action SwordThrustCancelledEvent;
        public event Action TeleportToOverworld;
        public event Action<CallbackContext, bool> TeleportToOverworldStarted;
        public event Action TeleportToMordenThroneRoom;
        public event Action<CallbackContext, bool> TeleportToMordenThroneRoomStarted;
        #endregion
        #region Combat Arts Input
        public event Action BarrierStartedEvent;
        public event Action BarrierPerformedEvent;
        public event Action BarrierCancelledEvent;
        public event Action EarthShakerStartedEvent;
        public event Action EarthShakerPerformedEvent;
        public event Action EarthShakerCancelledEvent;
        public event Action AirSlashStartedEvent;
        public event Action AirSlashCancelledEvent;
        public event Action AirSlashPerformedEvent;
        public event Action HellTridentStartedEvent;
        public event Action HellTridentCancelledEvent;
        public event Action HellTridentPerformedEvent;
        public event Action SoulFireBlastStartedEvent;
        public event Action SoulFireBlastCancelledEvent;
        public event Action SoulFireBlastPerformedEvent;
        public event Action BackDiverStartedEvent;
        public event Action BackDiverCancelledEvent;
        public event Action BackDiverPerformedEvent;
        public event Action SovereignImpaleStartedEvent;
        public event Action SovereignImpaleCancelledEvent;
        public event Action SovereignImpalePerformedEvent;
        public event Action DiagonalSwordDashStartedEvent;
        public event Action DiagonalSwordDashCancelledEvent;
        public event Action DiagonalSwordDashPerformedEvent;
        public event Action EdgedFuryStartedEvent;
        public event Action EdgedFuryCancelledEvent;
        public event Action EdgedFuryPerformedEvent;
        public event Action ReapersHarvestStartedEvent;
        public event Action ReapersHarvestCancelledEvent;
        public event Action ReapersHarvestPerformedEvent;
        public event Action IcarusWingsStartedEvent;
        public event Action IcarusWingsCancelledEvent;
        public event Action IcarusWingsPerformedEvent;
        public event Action TeleportingSkullStartedEvent;
        public event Action TeleportingSkullPerformedEvent;
        public event Action TeleportingSkullCancelledEvent;
        #endregion
        #region Overworld Input
        public event Action<Vector2> OverworldMovePerformedEvent;
        public event Action<Vector2> OverworldMoveCancelledEvent;
        public event Action<Vector2> OverworldMoveStartedEvent;
        #endregion
        #region UI Input
        public event Action<Vector2> UINavigatePerformedEvent;
        public event Action<Vector2> UINavigateCancelledEvent;
        public event Action<Vector2> UINavigateStartedEvent;
        public event Action UISubmitPerformedEvent;
        public event Action UISubmitCancelledEvent;
        public event Action UISubmitStartedEvent;
        public event Action UICancelPerformedEvent;
        public event Action UICancelCancelledEvent;
        public event Action UICancelStartedEvent;
        public event Action<Vector2> UIPointPerformedEvent;
        public event Action<Vector2> UIPointCancelledEvent;
        public event Action<Vector2> UIPointStartedEvent;
        public event Action<Vector2> UIScrollWheelPerformedEvent;
        public event Action<Vector2> UIScrollWheelCancelledEvent;
        public event Action<Vector2> UIScrollWheelStartedEvent;
        public event Action UIResumePerformedEvent;
        public event Action UIResumeCancelledEvent;
        public event Action UIResumeStartedEvent;
        public event Action UIClickPerformedEvent;
        public event Action UIClickCancelledEvent;
        public event Action UIClickStartedEvent;
        public event Action<float> UICycleTabsPerformedEvent;
        public event Action<float> UICycleSubTabsPerformedEvent;
        public event Action UIDeleteSaveEvent;
        public event Action UIToggleMapLegendEvent;
        public event Action UIHoldToSkipPerformedEvent;
        #endregion
        #region Army Battle Input
        public event Action ArmyBattleSelectCommandPerformedEvent;
        public event Action ArmyBattleSelectCommandCancelledEvent;
        public event Action ArmyBattleSelectCommandStartedEvent;
        #endregion
        #endregion

        public event Action ActiveActionMapChanged;

        public void SetInputModeToUnderworldGameplay()
        {
            m_playerControls.Underworld.Enable();
            m_playerControls.Overworld.Disable();
            m_playerControls.UI.Disable();
            m_playerControls.ArmyBattle.Disable();

            ActiveActionMapChanged?.Invoke();
        }

        public void SetInputModeToOverworldGameplay()
        {
            m_playerControls.Overworld.Enable();
            m_playerControls.Underworld.Disable();
            m_playerControls.UI.Disable();
            m_playerControls.ArmyBattle.Disable();

            ActiveActionMapChanged?.Invoke();
        }

        public void SetInputModeToUI()
        {
            m_playerControls.UI.Enable();
            m_playerControls.Underworld.Disable();
            m_playerControls.Overworld.Disable();
            m_playerControls.ArmyBattle.Disable();

            ActiveActionMapChanged?.Invoke();
        }

        public void SetInputModeToArmyBattleGameplay()
        {
            m_playerControls.ArmyBattle.Enable();
            m_playerControls.Underworld.Disable();
            m_playerControls.Overworld.Disable();
            m_playerControls.UI.Disable();

            ActiveActionMapChanged?.Invoke();
        }

        #region Underworld Actions
        //Underworld Actions
        public void OnVector2(InputAction.CallbackContext context)
        {
            OnVector2(context.phase, context.ReadValue<Vector2>());
        }

        public void OnVector2(InputActionPhase phase, Vector2 value)
        {
            if (phase == InputActionPhase.Performed)
            {
                Vector2InputPerformedEvent?.Invoke(value);
            }

            if (phase == InputActionPhase.Canceled)
            {
                Vector2CancelledInputEvent?.Invoke(value);
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                JumpStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                JumpPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                JumpCancelledEvent?.Invoke();
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                DashStartedEvent?.Invoke();
            }
        }

        public void OnGrab(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                GrabStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                GrabCancelledEvent?.Invoke();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                InteractStartedEvent?.Invoke();
            }
        }

        public void OnMouseDelta(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                MouseDeltaPerformedEvent?.Invoke(context.ReadValue<Vector2>());
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                PauseStartedEvent?.Invoke();
            }
        }

        public void OnTeleportToOverworld(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    TeleportToOverworldStarted?.Invoke(context, false);
                    break;
                case InputActionPhase.Canceled:
                    TeleportToOverworldStarted?.Invoke(context, true);
                    break;
                case InputActionPhase.Performed:
                    TeleportToOverworld?.Invoke();
                    break;
            }
        }

        public void OnTeleportToMordenThroneRoom(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    TeleportToMordenThroneRoomStarted?.Invoke(context, false);
                    break;
                case InputActionPhase.Canceled:
                    TeleportToMordenThroneRoomStarted?.Invoke(context, true);
                    break;
                case InputActionPhase.Performed:
                    TeleportToMordenThroneRoom?.Invoke();
                    break;
            }
        }

        public void OnQuickItemCycle(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                CycleQuickItemsStartedEvent?.Invoke(context.ReadValue<float>());
            }
        }

        public void OnQuickItemUse(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                if (context.interaction is HoldInteraction)
                {
                    UseQuickItemHeldEvent?.Invoke();
                }

                if (context.interaction is TapInteraction)
                {
                    UseQuickItemTappedEvent?.Invoke();
                }
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                UseQuickItemCancelledEvent?.Invoke();
            }
        }

        public void OnSlash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                SlashStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                if (context.interaction is HoldInteraction)
                {
                    SlashHeldEvent?.Invoke();
                }

                if (context.interaction is TapInteraction)
                {
                    SlashTappedEvent?.Invoke();
                }

                if (context.interaction is PressInteraction)
                {
                    SlashPressedEvent?.Invoke();
                }
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                SlashCancelledEvent?.Invoke();
            }
        }

        public void OnStore(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                StoreStartedEvent?.Invoke();
            }
        }

        #endregion

        #region Primary Skills
        public void OnShadowMorph(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                ShadowMorphStartedEvent?.Invoke();
            }
        }

        public void OnWhip(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                WhipPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                WhipCancelledEvent?.Invoke();
            }
        }

        public void OnProjectileThrow(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                ProjectileThrowStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                if (context.interaction is HoldInteraction)
                {
                    ProjectileThrowHeldEvent?.Invoke();
                }

                if (context.interaction is TapInteraction)
                {
                    ProjectileThrowTappedEvent?.Invoke();
                }
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                ProjectileThrowCancelledEvent?.Invoke();
            }
        }

        public void OnLevitate(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                LevitateStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                LevitatePerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                LevitateCancelledEvent?.Invoke();
            }
        }

        public void OnSwordThrust(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                if (context.interaction is HoldInteraction)
                {
                    SwordThrustPerformedEvent?.Invoke();
                }
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                SwordThrustCancelledEvent?.Invoke();
            }
        }
        #endregion

        #region Combat Arts
        //Combat Arts
        public void OnAirSlashCombo(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                AirSlashStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                AirSlashPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                AirSlashCancelledEvent?.Invoke();
            }
        }

        public void OnBackDiver(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                BackDiverStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                BackDiverPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                BackDiverCancelledEvent?.Invoke();
            }
        }

        public void OnBarrier(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                BarrierStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                BarrierPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                BarrierCancelledEvent?.Invoke();
            }
        }
        public void OnEarthShaker(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                EarthShakerStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                EarthShakerPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                EarthShakerCancelledEvent?.Invoke();
            }
        }

        public void OnReaperHarvest(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                ReapersHarvestStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                ReapersHarvestPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                ReapersHarvestCancelledEvent?.Invoke();
            }
        }

        public void OnSoulFireBlast(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                SoulFireBlastStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                SoulFireBlastPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                SoulFireBlastCancelledEvent?.Invoke();
            }
        }

        public void OnSovereignImpale(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                SovereignImpaleStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                SovereignImpalePerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                SovereignImpaleCancelledEvent?.Invoke();
            }
        }

        public void OnTeleportingSkull(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                TeleportingSkullStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                TeleportingSkullPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                TeleportingSkullCancelledEvent?.Invoke();
            }
        }

        public void OnDiagonalSwordDash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                DiagonalSwordDashStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                DiagonalSwordDashPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                DiagonalSwordDashCancelledEvent?.Invoke();
            }
        }

        public void OnEdgedFury(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                EdgedFuryStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                EdgedFuryPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                EdgedFuryCancelledEvent?.Invoke();
            }
        }

        public void OnHellTrident(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                HellTridentStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                HellTridentPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                HellTridentCancelledEvent?.Invoke();
            }
        }

        public void OnIcarusWings(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                IcarusWingsStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                IcarusWingsPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                IcarusWingsCancelledEvent?.Invoke();
            }
        }
        #endregion

        #region Overworld Controls
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                OverworldMoveStartedEvent?.Invoke(context.ReadValue<Vector2>());
            }

            if (context.phase == InputActionPhase.Performed)
            {
                OverworldMovePerformedEvent?.Invoke(context.ReadValue<Vector2>());
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                OverworldMoveCancelledEvent?.Invoke(context.ReadValue<Vector2>());
            }
        }
        #endregion

        #region UI Controls
        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                UINavigateStartedEvent?.Invoke(context.ReadValue<Vector2>());
            }

            if (context.phase == InputActionPhase.Performed)
            {
                UINavigatePerformedEvent?.Invoke(context.ReadValue<Vector2>());
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                UINavigateCancelledEvent?.Invoke(context.ReadValue<Vector2>());
            }
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                UISubmitStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                UISubmitPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                UISubmitCancelledEvent?.Invoke();
            }
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                UICancelStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                UICancelPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                UICancelCancelledEvent?.Invoke();
            }
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                UIPointStartedEvent?.Invoke(context.ReadValue<Vector2>());
            }

            if (context.phase == InputActionPhase.Performed)
            {
                UIPointPerformedEvent?.Invoke(context.ReadValue<Vector2>());
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                UIPointCancelledEvent?.Invoke(context.ReadValue<Vector2>());
            }
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                UIClickStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                UIClickPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                UIClickCancelledEvent?.Invoke();
            }
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                UIScrollWheelStartedEvent?.Invoke(context.ReadValue<Vector2>());
            }

            if (context.phase == InputActionPhase.Performed)
            {
                UIScrollWheelPerformedEvent?.Invoke(context.ReadValue<Vector2>());
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                UIScrollWheelCancelledEvent?.Invoke(context.ReadValue<Vector2>());
            }
        }

        public void OnResume(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                UIResumeStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                UIResumePerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                UIResumeCancelledEvent?.Invoke();
            }
        }

        public void OnCycleTab(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                UICycleTabsPerformedEvent?.Invoke(context.ReadValue<float>());
            }
        }

        public void OnCycleSubTab(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                UICycleSubTabsPerformedEvent?.Invoke(context.ReadValue<float>());
            }
        }

        public void OnDeleteSave(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                UIDeleteSaveEvent?.Invoke();
            }
        }
        public void OnToggleMapLegend(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                UIToggleMapLegendEvent?.Invoke();
            }
        }

        public void OnHoldToSkip(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                UIHoldToSkipPerformedEvent?.Invoke();
            }
        }
        #endregion

        #region Army Battle Controls
        public void OnSelectCommand(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                ArmyBattleSelectCommandStartedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Performed)
            {
                ArmyBattleSelectCommandPerformedEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                ArmyBattleSelectCommandCancelledEvent?.Invoke();
            }
        }
        #endregion

    }
}

