
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Combat.UI;
using DChild.Gameplay.Environment;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.UI;
using DChild.Menu.Trading;
using Doozy.Engine;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class GameplayUIHandle : MonoBehaviour, IGameplayUIHandle, IGameplaySystemModule
	{
		[SerializeField]
		private MerchantTradingManager m_merchantManager;
		[SerializeField]
		private StoreNavigator m_storeNavigator;
        [SerializeField]
        private BossCombatUI m_bossCombat;
        [SerializeField]
        private WorldMapHandler m_worldMap;

        public void OpenTradeWindow(NPCProfile merchantData,ITradableInventory merchantInventory,ITraderAskingPrice merchantAskingPrice)
		{
			m_merchantManager.SetProfile(merchantData);
			m_merchantManager.SetTradingPool(merchantInventory, merchantAskingPrice,GameplaySystem.playerManager.player.inventory);
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
            GameEventMessage.SendEvent("Notification");
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


    }
}
