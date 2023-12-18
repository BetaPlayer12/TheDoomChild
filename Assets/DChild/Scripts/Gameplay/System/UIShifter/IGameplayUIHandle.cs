using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.Environment;
using DChild.Gameplay.Items;
using DChild.Gameplay.Systems.Lore;
using DChild.Gameplay.Trade;
using DChild.Gameplay.UI;
using DChild.Menu.Trade;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public interface IGameplayUIHandle
    {
        IUINotificationManager notificationManager { get; }

        void ToggleCinematicMode(bool on, bool instant = false);

        public void ToggleCinematicBars(bool value);

        void UpdateNavMapConfiguration(Location location, int sceneIndex, Transform inGameReference, Vector2 mapReferencePoint, Vector2 calculationOffset);
        void OpenTradeWindow(NPCProfile merchantData, ITradeInventory merchantInventory, TradeAskingPrice merchantBuyingPriceRate);
        void OpenWeaponUpgradeConfirmationWindow();
        void OpenStoreAtPage(StorePage storePage);
        void OpenStore();

        void OpenWorldMap(Location fromLocation);
        void OpenShadowGateMap(Location fromLocation);

        void MonitorBoss(Boss boss);
        void ResetGameplayUI();

        void PromptKeystoneFragmentNotification();
        void ShowJournalNotificationPrompt(float duration);

        void ToggleBossHealth(bool willshow);

        void ToggleBossCombatUI(bool willshow);
        void ToggleFadeUI(bool willshow);
        void RevealBossName();
        void ShowInteractionPrompt(bool willshow);
        void ShowMovableObjectPrompt(bool willshow);
        void ShowGameOverScreen();
        void ShowGameplayUI(bool willshow);
        void ShowSequenceSkip(bool willShow);
        void ActivateHealthRegenEffect(PassiveRegeneration.Handle regenHandle);
        void DeactivateHealthRegenEffect();
        void ActivateShadowRegenEffect();
        void DeactivateShadowRegenEffect();
    }
}
