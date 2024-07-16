
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.Combat.UI;
using DChild.Gameplay.Environment;
using DChild.Gameplay.NavigationMap;
using DChild.Gameplay.Trade;
using DChild.Gameplay.UI;
using DChild.Menu;
using DChild.Menu.Trade;
using DChild.Temp;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Containers;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace DChild.Gameplay.Systems
{
    public class GameplayUIHandle : SerializedMonoBehaviour, IGameplayUIHandle, IGameplaySystemModule, IGameplayInitializable
    {
        [SerializeField, FoldoutGroup("Signals")]
        private SignalSender m_cinemaSignal;
        [SerializeField, FoldoutGroup("Signals")]
        private SignalSender m_gameOverSignal;
        [SerializeField, FoldoutGroup("Signals")]
        private SignalSender m_confirmationWindowSignal;

        [SerializeField]
        private UINotificationManager m_notificationManager;

        [SerializeField, FoldoutGroup("Merchant UI")]
        private SignalSender m_tradeWindowSignal;
        [SerializeField, FoldoutGroup("Merchant UI")]
        private TradeManager m_tradeManager;

        [SerializeField]
        private ConfirmationHandler m_confirmationWindow;

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

        [SerializeField, FoldoutGroup("Side Notification")]
        private UIContainer m_journalNotification;

        [SerializeField]
        private UIContainer m_playerHUD;
        [SerializeField]
        private UIContainer m_skippableUI;
        [SerializeField]
        private UIContainer m_fadeUI;

        [SerializeField, FoldoutGroup("Object Prompt")]
        private UIContainer m_interactablePrompt;
        [SerializeField, FoldoutGroup("Object Prompt")]
        private UIContainer m_movableObjectPrompt;

        [SerializeField]
        private WeaponUpgradeHandle m_upgradeWeaponHandler;
        [SerializeField]
        private CinematicVideoHandle m_cinematicVideoHandle;

        [SerializeField]
        private UIView m_cinematicBars;

        public IUINotificationManager notificationManager => m_notificationManager;

        public void ToggleCinematicMode(bool on, bool instant)
        {
            m_cinemaSignal.Payload.booleanValue = on;
            m_cinemaSignal.SendSignal();
            if (on && instant)
            {
                m_playerHUD.InstantHide();
            }
        }

        public void ToggleCinematicBars(bool value)
        {
            if (value)
            {
                m_cinematicBars.Show();
                m_playerHUD.InstantHide();
            }
            else
            {
                m_cinematicBars.Hide();
                m_playerHUD.InstantShow();
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

        public void OpenStoreAtPage(StorePage storePage)
        {
            m_storeNavigator.SetPage(storePage);
            m_storeNavigator.OpenStore();
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

        public void ToggleBossHealth(bool willshow)
        {
            if (willshow)
            {
                m_bossCombat?.ShowBossHealth();
            }
            else
            {
                m_bossCombat?.HideBossHealth();
            }
        }

        public void ResetGameplayUI()
        {
            GameEventMessage.SendEvent("UI Reset");
            ToggleBossCombatUI(false);
        }

        public void PromptKeystoneFragmentNotification()
        {
            GameEventMessage.SendEvent("Fragment Acquired"); // Currently Being called via string in ItemPickup
        }

        [Button]
        public void ToggleBossCombatUI(bool willshow)
        {
            if (willshow)
            {
                m_bossCombat.ShowBossName();
                // m_bossCombat.ShowBossHealth();
            }
            else
            {
                m_bossCombat.HideBossHealth();
            }
        }

        public void ToggleFadeUI(bool willshow)
        {
            if (willshow)
            {
                m_fadeUI.Show();
            }
            else
            {
                m_fadeUI.Hide();
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

        public void ShowGameOverScreen()
        {
            m_gameOverSignal.SendSignal();
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

        public void Initialize()
        {
            m_notificationManager.InitializeFullPriorityHandling();
            m_notificationManager.InitializePromptPriorityHandling();
            m_cinematicVideoHandle.Initialize();
            GameplaySystem.campaignSerializer.PostDeserialization += OnPostDeserialization;
        }

        public void OpenWeaponUpgradeConfirmationWindow()
        {
            m_upgradeWeaponHandler.RequestUpgrade();
            m_confirmationWindowSignal.SendSignal();
        }

        private void OnPostDeserialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            m_navMap.ForceMapUpdateOnNextOpen();
        }

        public void ShowCinematicVideo(VideoClip clip, Func<IEnumerator> behindTheSceneRoutine = null, Action OnVideoDone = null)
        {
            m_cinematicVideoHandle.ShowCinematicVideo(clip, behindTheSceneRoutine, OnVideoDone);
        }
    }
}