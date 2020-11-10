using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.UI;
using DChild.Menu.Trading;
using Doozy.Engine;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class UIModeHandle : MonoBehaviour, IUIModeHandle, IGameplaySystemModule
	{
		[SerializeField]
		private MerchantTradingManager m_merchantManager;
		[SerializeField]
		private StoreNavigator m_storeNavigator;
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

		public void OpenStorePage()
		{
			m_storeNavigator.OpenPage();
		}

	}
}
