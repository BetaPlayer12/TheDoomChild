using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Items;
using Holysoft.Event;

namespace DChild.Gameplay.Inventories
{
    [System.Serializable]
    public class QuickItemCountRemover
    {
        private IItemContainer m_container;
        private ItemData m_toRemove;
        private int m_removeCount;
        private bool m_executeRemoveOnThrow;

        public QuickItemCountRemover(IPlayer player, IItemContainer container)
        {
            player.character.GetComponentInChildren<ProjectileThrow>().ProjectileThrown += OnProjectileThrow;
            m_container = container;
        }

        public void Remove(ItemData data, int count)
        {
            switch (data.category)
            {
                case ItemCategory.Throwable:
                    m_toRemove = data;
                    m_removeCount = count;
                    m_executeRemoveOnThrow = true;
                    break;
                case ItemCategory.Consumable:
                    m_container.AddItem(data, -count);
                    m_executeRemoveOnThrow = false;
                    break;
            }
        }

        private void OnProjectileThrow(object sender, EventActionArgs eventArgs)
        {
            if (m_executeRemoveOnThrow)
            {
                m_container.AddItem(m_toRemove, -m_removeCount);
                m_executeRemoveOnThrow = false; 
            }
        }
    }
}
