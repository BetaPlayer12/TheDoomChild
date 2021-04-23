using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Environment;
using DChild.Gameplay.Inventories;
using DChild.Menu.Trading;
using System.Collections;
using System.Collections.Generic;

namespace DChild.Gameplay.Systems
{
    public interface IGameplayUIHandle
    {
        void OpenTradeWindow(NPCProfile merchantData,ITradableInventory merchantInventory, ITraderAskingPrice merchantAskingPrice);
        void OpenStorePage(StorePage storePage);
        void OpenStorePage();

        void OpenWorldMap(Location fromLocation);

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
        void ShowSoulEssenceNotify(bool willshow);
        void ShowPromptSoulEssenceChangeNotify();
        void ShowPauseMenu(bool willshow);
        void ShowGameOverScreen(bool willshow);
        void ShowItemAcquired(bool willshow);
        void ShowGameplayUI(bool willshow);
    }
}
