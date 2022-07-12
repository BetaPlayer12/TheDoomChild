using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Environment;
using DChild.Gameplay.Systems.Lore;
using DChild.Gameplay.Trade;
using DChild.Gameplay.UI;
using DChild.Menu.Trade;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public interface IGameplayUIHandle
    {
        void UpdateNavMapConfiguration(Location location, Transform inGameReference, Vector2 mapReferencePoint, Vector2 calculationOffset);
        void OpenTradeWindow(NPCProfile merchantData,ITradeInventory merchantInventory, TradeAskingPrice merchantBuyingPriceRate);
        void ShowLoreNote(LoreData m_data);
        void OpenStorePage(StorePage storePage);
        void OpenStorePage();

        void OpenWorldMap(Location fromLocation);
        void OpenShadowGateMap(Location fromLocation);

        void MonitorBoss(Boss boss);
        void ResetGameplayUI();
        void PromptPrimarySkillNotification();
        void PromptKeystoneFragmentNotification();
        void PromptBestiaryNotification();

        void ShowJournalNotificationPrompt(float duration);
        void PromptJournalUpdateNotification();

        void ShowQuickItem(bool willshow);
        void ShowBossHealth(bool willshow);
        void RevealBossName();
        void ShowInteractionPrompt(bool willshow);
        void ShowMovableObjectPrompt(bool willshow);
        void ShowSoulEssenceNotify(bool willshow);
        void ShowPromptSoulEssenceChangeNotify();
        void ShowPauseMenu(bool willshow);
        void ShowGameOverScreen(bool willshow);
        void ShowItemAcquired(bool willshow);
        void ShowGameplayUI(bool willshow);
        void ShowLootChestItemAcquired(LootList lootList);
        void ShowNotification(StoreNotificationType storeNotificationType);
        void ShowSequenceSkip(bool willShow);
    }
}
