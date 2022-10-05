
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Combat.UI;
using DChild.Gameplay.Environment;
using DChild.Gameplay.NavigationMap;
using DChild.Gameplay.Systems.Lore;
using DChild.Gameplay.Trade;
using DChild.Gameplay.UI;
using DChild.Menu.Trade;
using DChild.Temp;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class GameplayUIHandle : MonoBehaviour, IGameplayUIHandle, IGameplaySystemModule
    {
        [SerializeField]
        private SignalSender m_cinemaSignal;

        [SerializeField, BoxGroup("Full Screen Notifications")]
        private SignalSender m_fullScreenNotifSignal;
        [SerializeField, BoxGroup("Full Screen Notifications")]
        private UIContainer m_primarySkillNotification;
        [SerializeField, BoxGroup("Full Screen Notifications")]
        private LoreInfoUI m_loreNotification;


        [SerializeField, BoxGroup("Merchant UI")]
        private SignalSender m_tradeWindowSignal;
        [SerializeField, BoxGroup("Merchant UI")]
        private TradeManager m_tradeManager;

        [SerializeField]
        private StoreNavigator m_storeNavigator;
        [SerializeField]
        private BossCombatUI m_bossCombat;
        [SerializeField]
        private WorldMapHandler m_worldMap;
        [SerializeField]
        private NavigationMapManager m_navMap;


        [BoxGroup("Side Notification")]
        [SerializeField, BoxGroup("Side Notification")]
        private LootAcquiredUI m_lootAcquiredUI;
        [SerializeField, BoxGroup("Side Notification")]
        private StoreNotificationHandle m_storeNotification;
        [SerializeField, BoxGroup("Side Notification")]
        private UIContainer m_journalNotification;

        [SerializeField]
        private UIContainer m_skippableUI;

        public void ToggleCinematicMode(bool on)
        {
            m_cinemaSignal.Payload.booleanValue = on;
            m_cinemaSignal.SendSignal();
        }

        public void UpdateNavMapConfiguration(Location location, Transform inGameReference, Vector2 mapReferencePoint, Vector2 calculationOffset)
        {
            m_navMap.UpdateConfiguration(location, inGameReference, mapReferencePoint, calculationOffset);
        }

        public void OpenTradeWindow(NPCProfile merchantData, ITradeInventory merchantInventory, TradeAskingPrice merchantBuyingPriceRate)
        {
            m_tradeManager.SetSellerProfile(merchantData);
            m_tradeManager.SetSellingTradeRates(merchantBuyingPriceRate);
            m_tradeManager.SetupTrade(GameplaySystem.playerManager.player.inventory, merchantInventory);
            m_tradeWindowSignal.SendSignal();
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

        public void OpenStore()
        {
            m_storeNavigator.OpenStore();
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

        [ContextMenu("Prompt/Primary Skill")]
        public void PromptPrimarySkillNotification()
        {
            m_fullScreenNotifSignal.SendSignal();
            m_primarySkillNotification.Show(true);
        }

        public void PromptKeystoneFragmentNotification()
        {
            GameEventMessage.SendEvent("Fragment Acquired"); // Currently Being called via string in ItemPickup
        }

        public void PromptBestiaryNotification()
        {
            //GameEventMessage.SendEvent("Notification");
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

        public void ShowMovableObjectPrompt(bool willshow)
        {
            if (willshow == true)
            {
                GameEventMessage.SendEvent("MovableObject Prompt Show");
            }
            else
            {
                GameEventMessage.SendEvent("MovableObject Prompt Hide");
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
            m_journalNotification.InstantHide();
            m_journalNotification.AutoHideAfterShowDelay = duration;
            m_journalNotification.Show(true);
        }

        public void ShowLoreNote(LoreData data)
        {
            m_loreNotification.SetInfo(data);
            m_fullScreenNotifSignal.SendSignal();
            m_loreNotification.Show();
        }

        public void PromptJournalUpdateNotification()
        {
            GameEventMessage.SendEvent("Show JournalInfo");
        }

        public void ShowLootChestItemAcquired(LootList lootList)
        {
            m_lootAcquiredUI.SetDetails(lootList);
            m_lootAcquiredUI.Show();
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
