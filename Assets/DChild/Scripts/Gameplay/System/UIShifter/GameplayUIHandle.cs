
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Combat.UI;
using DChild.Gameplay.Environment;
using DChild.Gameplay.NavigationMap;
using DChild.Gameplay.Systems.Lore;
using DChild.Gameplay.Trade;
using DChild.Gameplay.UI;
using DChild.Menu.Trade;
using Doozy.Engine;
using Doozy.Engine.UI;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class GameplayUIHandle : MonoBehaviour, IGameplayUIHandle, IGameplaySystemModule
    {
        [SerializeField]
        private TradeManager m_tradeManager;
        [SerializeField]
        private StoreNavigator m_storeNavigator;
        [SerializeField]
        private BossCombatUI m_bossCombat;
        [SerializeField]
        private WorldMapHandler m_worldMap;
        [SerializeField]
        private NavigationMapManager m_navMap;
        [SerializeField]
        private LoreInfoUI m_loreUI;
        [SerializeField]
        private LootAcquiredUI m_lootAcquiredUI;
        [SerializeField]
        private StoreNotificationHandle m_storeNotification;
        [SerializeField]
        private UIView m_skippableUI;

        public void UpdateNavMapConfiguration(Location location, Transform inGameReference, Vector2 mapReferencePoint, Vector2 calculationOffset)
        {
            m_navMap.UpdateConfiguration(location, inGameReference, mapReferencePoint, calculationOffset);
        }

        public void OpenTradeWindow(NPCProfile merchantData, ITradeInventory merchantInventory, TradeAskingPrice merchantBuyingPriceRate)
        {
            m_tradeManager.SetSellerProfile(merchantData);
            m_tradeManager.SetSellingTradeRates(merchantBuyingPriceRate);
            m_tradeManager.SetupTrade(GameplaySystem.playerManager.player.inventory, merchantInventory);
            GameEventMessage.SendEvent("Trade Open");
        }

        public void OpenStorePage(StorePage storePage)
        {
            m_storeNavigator.SetPage(storePage);
            m_storeNavigator.OpenPage();
        }

        public void OpenWorldMap(Location fromLocation)
        {
            GameEventMessage.SendEvent("WorldMap Open");
            m_worldMap.SetFromLocation(fromLocation);
        }

        public void OpenShadowGateMap(Location fromLocation)
        {
            GameEventMessage.SendEvent("ShadowGateMap Open");
        }

        public void OpenStorePage()
        {
            m_storeNavigator.OpenPage();
        }

        public void MonitorBoss(Boss boss)
        {
            m_bossCombat?.SetBoss(boss);
        }

        public void ResetGameplayUI()
        {
            GameEventMessage.SendEvent("UI Reset");
            ShowBossHealth(false);
        }

        public void PromptPrimarySkillNotification()
        {
            GameEventMessage.SendEvent("Primary Skill Acquired");
        }

        public void PromptKeystoneFragmentNotification()
        {
            GameEventMessage.SendEvent("Fragment Acquired"); // Currently Being called via string in ItemPickup
        }

        public void PromptBestiaryNotification()
        {
            //GameEventMessage.SendEvent("Notification");
        }

        public void ShowQuickItem(bool willshow)
        {
            if (willshow == true)
            {
                GameEventMessage.SendEvent("QuickItem Show");
            }
            else
            {
                GameEventMessage.SendEvent("QuickItem Hide");
            }
        }

        public void ShowBossHealth(bool willshow)
        {
            if (willshow == true)
            {
                GameEventMessage.SendEvent("Boss Encounter");
            }
            else
            {
                GameEventMessage.SendEvent("Boss Gone");
            }
        }

        public void RevealBossName()
        {
            GameEventMessage.SendEvent("Boss Encounter");
        }

        public void ShowInteractionPrompt(bool willshow)
        {
            if (willshow == true)
            {
                GameEventMessage.SendEvent("Interaction Prompt Show");
            }
            else
            {
                GameEventMessage.SendEvent("Interaction Prompt Hide");
            }
        }

        public void ShowSoulEssenceNotify(bool willshow)
        {
            if (willshow == true)
            {
                GameEventMessage.SendEvent("Soul Essence Notify");
            }
            else
            {
                GameEventMessage.SendEvent("Soul Essence Hide");
            }
        }
        public void ShowPromptSoulEssenceChangeNotify()
        {
            GameEventMessage.SendEvent("Soul Essence Added");
        }

        public void ShowPauseMenu(bool willshow)
        {
            if (willshow == true)
            {
                GameEventMessage.SendEvent("Pause Game");
            }
            else
            {

            }
        }

        public void ShowGameOverScreen(bool willshow)
        {
            if (willshow == true)
            {
                GameEventMessage.SendEvent("Game Over");
            }
            else
            {

            }
        }

        public void ShowItemAcquired(bool willshow)
        {
            if (willshow == true)
            {
                GameEventMessage.SendEvent("ItemNotify");
            }
            else
            {
                GameEventMessage.SendEvent("Renew ItemNotify");
            }

        }
        public void ShowGameplayUI(bool willshow)
        {
            if (willshow == true)
            {
                GameEventMessage.SendEvent("Show UI");
            }
            else
            {
                GameEventMessage.SendEvent("Hide UI");
            }
        }

        public void ShowJournalNotificationPrompt(float duration)
        {
            GameEventMessage.SendEvent("Hide JournalUpdate");
            StopCoroutine("PromptJournalUpdateRoutine");
            StartCoroutine(PromptJournalUpdateRoutine(duration));
        }

        public void ShowLoreNote(LoreData data)
        {
            m_loreUI.SetInfo(data);
            GameEventMessage.SendEvent("Show LoreNote");
        }

        public void PromptJournalUpdateNotification()
        {
            GameEventMessage.SendEvent("Show JournalInfo");
        }

        private IEnumerator PromptJournalUpdateRoutine(float duration)
        {
            GameEventMessage.SendEvent("Show JournalUpdate");
            yield return new WaitForSeconds(duration);
            GameEventMessage.SendEvent("Hide JournalUpdate");
        }
        public void ShowLootChestItemAcquired(LootList lootList)
        {
            m_lootAcquiredUI.SetDetails(lootList);
            GameEventMessage.SendEvent("Loot Notify");
        }

        public void ShowSequenceSkip(bool willShow)
        {
            if (willShow)
            {
                m_skippableUI.Show();
            }
            else
            {
                m_skippableUI.Hide();
            }
        }

        public void ShowNotification(StoreNotificationType storeNotificationType)
        {
            m_storeNotification.ShowNotification(storeNotificationType);
            switch (storeNotificationType)
            {
                case StoreNotificationType.Bestiary:
                    m_storeNavigator.SetPage(StorePage.Bestiary);
                    break;
                case StoreNotificationType.Lore:
                    break;
                case StoreNotificationType.Extras:
                    break;
            }
        }
    }
}
