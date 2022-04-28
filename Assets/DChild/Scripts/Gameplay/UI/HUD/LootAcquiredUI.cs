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
            int availableUIIndex = 0;

            for (int index = 0; index < m_individualLootUIs.Length; index++)
            {
                if (index < lootItems.Length)
                {
                    var ui = m_individualLootUIs[index];
                    ui.Show();
                    var item = lootItems[index];
                    ui.SetDetails(item, lootList.GetCountOf(item));
                    availableUIIndex++;
                }
                else
                {
                    m_individualLootUIs[index].Hide();
                }
            }

            if (availableUIIndex < m_individualLootUIs.Length)
            {
                var soulUi = m_individualLootUIs[availableUIIndex];
                if (lootList.soulEssenceAmount > 0)
                {
                    soulUi.SetDetails(null, lootList.soulEssenceAmount);
                    soulUi.Show();
                }
                else
                {
                    soulUi.Hide();
                }
            }
        }

    }
}