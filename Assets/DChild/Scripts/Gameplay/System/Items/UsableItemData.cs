using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Items
{

    [CreateAssetMenu(fileName = "UsableItemData", menuName = "DChild/Database/Usable Item Data")]
    public class UsableItemData : ConsumableItemData
    {
        [SerializeField, ToggleGroup("m_enableEdit")]
        private IUsableItemModule[] m_moduleList;

        public override bool CanBeUse(IPlayer player)
        {
            for (int i = 0; i < m_moduleList.Length; i++)
            {
                if (m_moduleList[i].CanBeUse(player) == false)
                    return false;
            }

            return true;
        }

        public override void Use(IPlayer player)
        {
#if UNITY_EDITOR
            Debug.Log($"{itemName} Consumed");
#endif
            if (m_moduleList != null)
            {
                for (int i = 0; i < m_moduleList.Length; i++)
                {
                    m_moduleList[i].Use(player);
                }
            }
        }
    }
}
