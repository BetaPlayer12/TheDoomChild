using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [CreateAssetMenu(fileName = "UsableItemData", menuName = "DChild/Database/Usable Item Data")]
    public class UsableItemData : ItemData
    {
        [SerializeField, ToggleGroup("m_enableEdit")]
        private IUsableItemModule[] m_moduleList;

        public void Use(IPlayer player)
        {
            for (int i = 0; i < m_moduleList.Length; i++)
            {
            m_moduleList[i].Use(player);
            }
        }
    }
}
