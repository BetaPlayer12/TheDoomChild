using UnityEngine;

namespace DChild.Gameplay.Items
{
    public class ItemTaker : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private ItemData m_data;

        public void TakeItem()
        {
            GameplaySystem.playerManager.player.inventory.RemoveItem(m_data);
        }
    }
}
