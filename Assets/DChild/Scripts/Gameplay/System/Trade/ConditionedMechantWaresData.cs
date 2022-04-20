using DChild.Gameplay.Inventories;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace DChild.Gameplay.Trade
{
    [CreateAssetMenu(fileName = "Conditioned Mechant Wares Data", menuName = "DChild/Gameplay/Trading/Conditioned Mechant Wares Data")]
    public class ConditionedMechantWaresData : SerializedScriptableObject
    {
        [System.Serializable]
        private class ConditionedWares
        {
            [SerializeField, LuaConditionsWizard]
            private string m_condition;
            [OdinSerialize]
            private IInventoryInfo m_inventoryInfo;

            public string condition => m_condition;
            public IInventoryInfo inventoryInfo => m_inventoryInfo;
        }

        [SerializeField]
        private InventoryData m_initialWare;
        [OdinSerialize, HideReferenceObjectPicker]
        private ConditionedWares[] m_conditionsWares = new ConditionedWares[0];

        public IInventoryInfo GetAppropriateWares()
        {
            BaseStoredItemList itemList = new BaseStoredItemList();

            AddInfoTo(m_initialWare,ref itemList);

            for (int i = 0; i < m_conditionsWares.Length; i++)
            {
                var conditionedWare = m_conditionsWares[i];
                if (Lua.IsTrue(conditionedWare.condition))
                {
                    AddInfoTo(conditionedWare.inventoryInfo, ref itemList);
                }
            }
            return itemList;
        }

        private void AddInfoTo(IInventoryInfo reference, ref BaseStoredItemList target)
        {
            for (int i = 0; i < reference.storedItemCount; i++)
            {
                var item = reference.GetItem(i);
                if (item.hasInfiniteCount)
                {
                    target.SetItemAsInfinite(item.data, true);
                }
                else
                {
                    target.AddItem(item.data, item.count);
                }
            }
        }
    }
}