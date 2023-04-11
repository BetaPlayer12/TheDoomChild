
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.Combat.UI;
using DChild.Gameplay.Environment;
using DChild.Gameplay.Items;
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
    public class GameplayUIHandle : SerializedMonoBehaviour, IGameplayUIHandle, IGameplaySystemModule
    {
        [SerializeField, FoldoutGroup("Signals")]
        private SignalSender m_cinemaSignal;
        [SerializeField, FoldoutGroup("Signals")]
        private SignalSender m_gameOverSignal;

        [SerializeField, FoldoutGroup("Full Screen Notifications")]
        private SignalSender m_fullScreenNotifSignal;
        [SerializeField, FoldoutGroup("Full Screen Notifications")]
        private UIContainer m_primarySkillNotification;
        [SerializeField, FoldoutGroup("Full Screen Notifications")]
        private UIContainer m_soulSkillNotification;
        [SerializeField, FoldoutGroup("Full Screen Notifications")]
        private UIContainer m_journalDetailedNotification;
        [SerializeField, FoldoutGroup("Full Screen Notifications")]
        private LoreInfoUI m_loreNotification;
        [SerializeField, FoldoutGroup("Full Screen Notifications")]
        private IItemNotificationUI[] m_itemNotifications;

        [SerializeField, FoldoutGroup("Merchant UI")]
        private SignalSender m_tradeWindowSignal;
        [SerializeField, FoldoutGroup("Merchant UI")]
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
        private RegenerationEffectsHandler m_regen;


        [FoldoutGroup("Side Notification")]
        [SerializeField, FoldoutGroup("Side Notification")]
        private LootAcquiredUI m_lootAcquiredUI;
        [SerializeField, FoldoutGroup("Side Notification")]
        private StoreNotificationHandle m_storeNotification;
        [SerializeField, FoldoutGroup("Side Notification")]
        private UIContainer m_journalNotification;

        [SerializeField]
        private UIContainer m_playerHUD;
        [SerializeField]
        private UIContainer m_skippableUI;

        [SerializeField, FoldoutGroup("Object Prompt")]
        private UIContainer m_interactablePrompt;
        [SerializeField, FoldoutGroup("Object Prompt")]
        private UIContainer m_movableObjectPrompt;

        public void ToggleCinematicMode(bool on, bool instant)
        {
            m_cinemaSignal.Payload.booleanValue = on;
            m_cinemaSignal.SendSignal();
            if (on && instant)
            {
                m_playerHUD.InstantHide();
            }
        }

        public void UpdateNavMapConfiguration(Location location, int sceneIndex, Transform inGameReference, Vector2 mapReferencePoint, Vector2 calculationOffset)
        {
            m_navMap.UpdateConfiguration(location, sceneIndex, inGameReference, mapReferencePoint, calculationOffset);
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
            ToggleBossCombatUI(false);
        }

        [ContextMenu("Prompt/Primary Skill")]
        public void PromptPrimarySkillNotification()
        {
            m_fullScreenNotifSignal.SendSignal();
            m_primarySkillNotification.Show(true);
        }
        [ContextMenu("Prompt/Soul Skill")]
        public void PromptSoulSkillNotification()
        {
            m_fullScreenNotifSignal.SendSignal();
            m_soulSkillNotification.Show(true);
        }

        public void ShowItemNotification(ItemData itemData)
        {
            m_fullScreenNotifSignal.SendSignal();
            for (int i = 0; i < m_itemNotifications.Length; i++)
            {
                var notification = m_itemNotifications[i];
                if (notification.IsNotificationFor(itemData))
                {
                    notification.ShowNotificationFor(itemData);
                }
            }
        }

        public void PromptJournalUpdateNotification()
        {
            m_fullScreenNotifSignal.SendSignal();
            m_journalDetailedNotification.Show(true);
            m_journalNotification.Hide();
        }

        public void PromptKeystoneFragmentNotification()
        {
            GameEventMessage.SendEvent("Fragment Acquired"); // Currently Being called via string in ItemPickup
        }

        public void PromptBestiaryNotification()
        {
            //GameEventMessage.SendEvent("Notification");
        }

        [Button]
        public void ToggleBossCombatUI(bool willshow)
        {
            Debug.Log("Boss UI will show: " + willshow);
            if (willshow)
            {
                Debug.Log("Will show is: " + willshow);
                m_bossCombat.ShowBossName();
                m_bossCombat.ShowBossHealth();
            }
            else
            {
                m_bossCombat.HideBossHealth();
                m_bossCombat.HideBossName();
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
                m_interactablePrompt.Show();
            }
            else
            {
                m_interactablePrompt.Hide();
            }
        }

        public void ShowMovableObjectPrompt(bool willshow)
        {
            if (willshow == true)
            {
                m_movableObjectPrompt.Show();
            }
            else
            {
                m_movableObjectPrompt.Hide();
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

        public void ShowGameOverScreen()
        {
            m_gameOverSignal.SendSignal();
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
        public void ActivateHealthRegenEffect(PassiveRegeneration.Handle handle)
        {
            m_regen.SetHealthRegenReference(handle);
            m_regen.HealthRegenEffect(true);
        }
        public void DeactivateHealthRegenEffect()
        {
            m_regen.HealthRegenEffect(false);
        }
        public void ActivateShadowRegenEffect()
        {
            m_regen.ShadowRegenEffect(true);
        }
        public void DeactivateShadowRegenEffect()
        {
            m_regen.ShadowRegenEffect(false);
        }
    }
}
