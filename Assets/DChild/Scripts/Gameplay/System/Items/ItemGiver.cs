using UnityEngine;

namespace DChild.Gameplay.Items
{
    public class ItemGiver : MonoBehaviour
    {
        [SerializeField]
        private ItemData m_data;

        public void GiveItem()
        {
            GameplaySystem.playerManager.player.inventory.AddItem(m_data);
        }
    }
}