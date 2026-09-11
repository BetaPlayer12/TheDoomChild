using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.Environment;
using DChild.Gameplay.FastTravel;
using DChild.Gameplay.LevelFinish.UI;
using DChild.Gameplay.Systems.Serialization;
using DChild.Gameplay.Trade;
using DChild.Gameplay.UI;
using DChild.Gameplay.UI.Alerts;
using DChild.Menu.Trade;
using DChild.Scripts.Gameplay.Environment.Interactables.Elevator;
using DChild.UI;
using Doozy.Runtime.Signals;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

namespace DChild.Gameplay.Systems
{
    public class GameplayUIHandle : IGameplayUIHandle, IGameplaySystemModule, IGameplayInitializable
    {
        public static GameplayUIHandle Instance { get; private set; }

        public bool isInCutsceneMode { get; private set; }

        public UIAlertManager alertManager => BaseGameplayUIHandle.Instance.uiAlertManager;
        public IUINotificationManager notificationManager => UnderworldGameplayUIHandle.Instance.notificationManager;

        public void ActivateHealthRegenEffect(PassiveRegeneration.Handle regenHandle)
        {
            UnderworldGameplayUIHandle.Instance.ActivateHealthRegenEffect(regenHandle);
        }

        public void ActivateShadowRegenEffect()
        {
            UnderworldGameplayUIHandle.Instance.ActivateShadowRegenEffect();
        }

        public void DeactivateHealthRegenEffect()
        {
            UnderworldGameplayUIHandle.Instance.DeactivateHealthRegenEffect();
        }

        public void DeactivateShadowRegenEffect()
        {
            UnderworldGameplayUIHandle.Instance.DeactivateShadowRegenEffect();
        }

        public void Initialize()
        {
            BaseGameplayUIHandle.Instance.Initialize();
        }

        public void MonitorBoss(Boss boss)
        {
            UnderworldGameplayUIHandle.Instance.MonitorBoss(boss);
        }

        public void OpenFastTravel(Location startingLocation, FastTravelData playerLocation)
        {
            BaseGameplayUIHandle.Instance.OpenFastTravel(startingLocation, playerLocation);
        }

        public void OpenShadowGateMap(Location fromLocation)
        {
            throw new NotImplementedException();
        }

        public void OpenStore()
        {
            if (GameplaySystem.isGamePaused)
                return;

            if (GameSystem.CurrentGameMode == GameMode.Underworld)
            {
                UnderworldGameplayUIHandle.Instance.OpenStore();

            }
            else if (GameSystem.CurrentGameMode == GameMode.Overworld)
            {
                OverworldGameplayUIHandle.Instance.OpenStore();
            }
        }

        public void OpenStoreAtPage(StorePage storePage)
        {
            if (GameSystem.CurrentGameMode == GameMode.Underworld)
            {
                UnderworldGameplayUIHandle.Instance.OpenStoreAtPage(storePage);

            }
            else if (GameSystem.CurrentGameMode == GameMode.Overworld)
            {
                OverworldGameplayUIHandle.Instance.OpenStoreAtPage(storePage);
            }

        }

        public void OpenTradeWindow(NPCProfile merchantData, ITradeInventory merchantInventory, TradeAskingPrice merchantBuyingPriceRate, CurrencyType type)
        {
            UnderworldGameplayUIHandle.Instance.OpenTradeWindow(merchantData, merchantInventory, merchantBuyingPriceRate, type);
        }

        public void OpenWeaponUpgradeConfirmationWindow()
        {
            UnderworldGameplayUIHandle.Instance.OpenWeaponUpgradeConfirmationWindow();
        }

        public void OpenWorldMap(Location fromLocation)
        {
            throw new NotImplementedException();
        }

        public void PromptKeystoneFragmentNotification()
        {
            throw new NotImplementedException();
        }

        public void RequestTeleportConfirmation(LocationData destinationData)
        {
            BaseGameplayUIHandle.Instance.RequestTeleportConfirmation(destinationData);
        }

        public void NotifyUnlockedLocation(AvailableLocations location, InputActionConfiguration input)
        {
            BaseGameplayUIHandle.Instance.NotifyUnlockedLocation(location, input);
        }

        public void ResetGameplayUI()
        {
            UnderworldGameplayUIHandle.Instance.ResetGameplayUI();
        }

        public void RevealBossName()
        {
            UnderworldGameplayUIHandle.Instance.RevealBossName();
        }

        public void ShowCinematicVideo(VideoClip clip, Func<IEnumerator> behindTheSceneRoutine = null, Action OnVideoDone = null , bool hasEventOnVideoEnd = false, float secondsBeforeVideoEnds = 0f, float audiTansistionDuration = 0f)
        {
            BaseGameplayUIHandle.Instance.ShowCinematicVideo(clip, behindTheSceneRoutine, OnVideoDone,hasEventOnVideoEnd,secondsBeforeVideoEnds,audiTansistionDuration);
        }

        public void ForceStopCinematicVideo()
        {
            BaseGameplayUIHandle.Instance.ForceStopCinematicVideo();
        }

        public void ShowGameOverScreen()
        {
            BaseGameplayUIHandle.Instance.ShowGameOverScreen();
        }

        public void ShowGameplayUI(bool willshow)
        {
            UnderworldGameplayUIHandle.Instance.ShowGameplayUI(willshow);
        }

        public void ShowInteractionPrompt(bool willshow)
        {
            switch (GameplaySystem.GetCurrentWorldType())
            {
                case WorldType.Underworld:
                    UnderworldGameplayUIHandle.Instance.ShowInteractionPrompt(willshow);
                    break;
                case WorldType.Overworld:
                    OverworldGameplayUIHandle.Instance.ShowInteractionPrompt(willshow);
                    break;
                case WorldType.ArmyBattle:
                    break;
            }
        }

        public void ShowJournalNotificationPrompt(float duration)
        {
            UnderworldGameplayUIHandle.Instance.ShowJournalNotificationPrompt(duration);
        }

        public void ShowMordenElevatorUI(ElevatorLocation location, ElevatorLevelInfo[] labels, MovingPlatform elevator)
        {
            UnderworldGameplayUIHandle.Instance.OpenElevator(location, labels, elevator);
        }

        public void ShowMovableObjectPrompt(bool willshow)
        {
            UnderworldGameplayUIHandle.Instance.ShowMovableObjectPrompt(willshow);
        }

        public void ToggleSequenceSkip(bool willShow)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (BaseGameplayUIHandle.Instance == null)
            {
                Debug.LogWarning(
                    $"Skipping sequence-skip UI toggle ({willShow}) because the Base Gameplay UI is not loaded. " +
                    "This is expected when debugging the ArmyBattle scene directly.");
                return;
            }
#endif

            BaseGameplayUIHandle.Instance.ToggleSequenceSkip(willShow);
        }

        public void ShowHoldToTeleportSequence(InputAction.CallbackContext context, bool isCanceled)
        {
            UnderworldGameplayUIHandle.Instance.ShowHoldToTeleportSequence(context, isCanceled);
        }

        public void ToggleBossCombatUI(bool willshow)
        {
            UnderworldGameplayUIHandle.Instance.ToggleBossCombatUI(willshow);
        }

        public void ToggleBossHealth(bool willshow)
        {
            UnderworldGameplayUIHandle.Instance.ToggleBossHealth(willshow);
        }

        public void ToggleCinematicBars(bool value)
        {
            BaseGameplayUIHandle.Instance.ToggleCinematicBars(value);
        }

        public void ToggleCinematicMode(bool on, bool instant = false)
        {
            isInCutsceneMode = on;
            BaseGameplayUIHandle.Instance.ToggleCinematicMode(on);
        }

        public void ToggleFadeUI(bool willshow)
        {
            BaseGameplayUIHandle.Instance.ToggleFadeUI(willshow);
        }

        public void TogglePause(bool toggle)
        {
            BaseGameplayUIHandle.Instance.TogglePause(toggle);
        }

        public void UpdateNavMapConfiguration(Location location, int sceneIndex, Transform inGameReference, Vector2 mapReferencePoint, Vector2 calculationOffset)
        {
            if (location == Location.Overworld)
            {
                OverworldGameplayUIHandle.Instance.UpdateNavMapConfiguration(location, sceneIndex, inGameReference, mapReferencePoint, calculationOffset);
            }
            else
            {
                UnderworldGameplayUIHandle.Instance.UpdateNavMapConfiguration(location, sceneIndex, inGameReference, mapReferencePoint, calculationOffset);
            }
        }

        public UIHandlerExtraReference GetReference()
        {
            return UnderworldGameplayUIHandle.Instance.getReference();
        }

        public CharacterRecruitmentUI ConfirmationRequest()
        {
            return BaseGameplayUIHandle.Instance.GetRecruitmentConfirmation();
        }

        public void OpenPauseMenu()
        {
            BaseGameplaySystem.gamplayUIHandle.OpenPauseMenu();
        }

        public void UIBack()
        {
            BaseGameplaySystem.gamplayUIHandle.UIBack();
        }

        public void OverrideCurrentUIState(GameplayUIState state)
        {
            BaseGameplaySystem.gamplayUIHandle.OverrideCurrentUIState(state);
        }
    }
}
