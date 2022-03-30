using DChild.Gameplay.Inventories;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace DChild.Menu.Trading
{
    [CreateAssetMenu(fileName = "Conditioned Mechant Wares Data", menuName = "DChild/Gameplay/Trading/Conditioned Mechant Wares Data")]
    public class ConditionedMechantWaresData : ScriptableObject
    {
        [System.Serializable]
        private class ConditionedWares
        {
            [SerializeField, LuaConditionsWizard]
            private string m_condition;
            [SerializeField]
            private InventoryData m_inventoryData;

            public string condition => m_condition;
            public InventoryData inventoryData => m_inventoryData;
        }

        [SerializeField]
        private InventoryData m_defaultWare;
        [SerializeField]
        private ConditionedWares[] m_conditionsWares;

        public InventoryData GetAppropriateWares()
        {
            for (int i = 0; i < m_conditionsWares.Length; i++)
            {
                var conditionedWare = m_conditionsWares[i];
               if (Lua.IsTrue(conditionedWare.condition))
                {
                    return conditionedWare.inventoryData;
                }
            }
            return m_defaultWare;
        }
    }
}