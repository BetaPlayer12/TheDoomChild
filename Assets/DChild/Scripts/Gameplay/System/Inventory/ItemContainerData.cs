using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif
namespace DChild.Gameplay.Inventories
{
    [CreateAssetMenu(fileName = "ItemContainerData", menuName = "DChild/Database/Item Container Data")]
    public class ItemContainerData : ScriptableObject
    {
        //[SerializeField, TableList(ShowIndexLabels = true, NumberOfItemsPerPage = 5, ShowPaging = true)]
        //private List<ItemSlot> m_list;

        //public ItemSlot[] list => m_list.ToArray();
    }
}
