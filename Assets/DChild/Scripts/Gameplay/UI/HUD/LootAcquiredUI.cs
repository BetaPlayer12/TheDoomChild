using DChild.Gameplay.Systems;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class LootAcquiredUI : MonoBehaviour
    {
        [SerializeField]
        private IndividualLootAcquiredUI[] m_individualLootUIs;

        public void SetDetails(LootList lootList)
        {
            var lootItems = lootList.GetAllItems();
            for (int i = 0; i < m_individualLootUIs.Length; i++)
            {
                if(i < lootItems.Length)
                {
                    var ui = m_individualLootUIs[i];
                    ui.Show();
                    var item = lootItems[i];
                    ui.SetDetails(item, lootList.GetCountOf(item));
                }
                else
                {
                    m_individualLootUIs[i].Hide();
                }
            }
        }
    }
}