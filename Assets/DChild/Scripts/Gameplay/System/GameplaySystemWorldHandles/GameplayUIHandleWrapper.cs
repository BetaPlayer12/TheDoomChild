using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.Environment;
using DChild.Gameplay.Trade;
using DChild.Gameplay.UI;
using DChild.Menu.Trade;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace DChild.Gameplay.Systems
{
    public class GameplayUIHandleWrapper : IGameplayUIHandle, IGameplaySystemModule, IGameplayInitializable
    {
        public static GameplayUIHandleWrapper Instance { get; private set; }

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

        public void OpenShadowGateMap(Location fromLocation)
        {
            throw new NotImplementedException();
        }

        public void OpenStore()
        {
            UnderworldGameplayUIHandle.Instance.OpenStore();
        }

        public void OpenStoreAtPage(StorePage storePage)
        {
            UnderworldGameplayUIHandle.Instance.OpenStoreAtPage(storePage);
        }

        public void OpenTradeWindow(NPCProfile merchantData, ITradeInventory merchantInventory, TradeAskingPrice merchantBuyingPriceRate)
        {
            UnderworldGameplayUIHandle.Instance.OpenTradeWindow(merchantData, merchantInventory, merchantBuyingPriceRate);
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

        public void ResetGameplayUI()
        {
            UnderworldGameplayUIHandle.Instance.ResetGameplayUI();
        }

        public void RevealBossName()
        {
            UnderworldGameplayUIHandle.Instance.RevealBossName();
        }

        public void ShowCinematicVideo(VideoClip clip, Func<IEnumerator> behindTheSceneRoutine = null, Action OnVideoDone = null)
        {
            BaseGameplayUIHandle.Instance.ShowCinematicVideo(clip, behindTheSceneRoutine, OnVideoDone);
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
            UnderworldGameplayUIHandle.Instance.ShowInteractionPrompt(willshow);
        }

        public void ShowJournalNotificationPrompt(float duration)
        {
            UnderworldGameplayUIHandle.Instance.ShowJournalNotificationPrompt(duration);
        }

        public void ShowMovableObjectPrompt(bool willshow)
        {
            UnderworldGameplayUIHandle.Instance.ShowMovableObjectPrompt(willshow);
        }

        public void ShowSequenceSkip(bool willShow)
        {
            BaseGameplayUIHandle.Instance.ShowSequenceSkip(willShow);
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
            BaseGameplayUIHandle.Instance.ToggleCinematicMode(on);
        }

        public void ToggleFadeUI(bool willshow)
        {
            BaseGameplayUIHandle.Instance.ToggleFadeUI(willshow);
        }

        public void UpdateNavMapConfiguration(Location location, int sceneIndex, Transform inGameReference, Vector2 mapReferencePoint, Vector2 calculationOffset)
        {
            UnderworldGameplayUIHandle.Instance.UpdateNavMapConfiguration(location, sceneIndex, inGameReference, mapReferencePoint, calculationOffset);
        }
    }
}