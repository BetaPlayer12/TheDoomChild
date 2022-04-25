using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace DChild.Gameplay.Trade
{
    public class MerchantOneTimeBuyRecorderData : ScriptableObject
    {
        public class Info
        {
            [SerializeField]
            private ItemData m_item;
            [SerializeField, LuaScriptWizard]
            private string m_luaScript;

            public ItemData item => m_item;
            public string luaScript => m_luaScript;
        }

        [SerializeField]
        private Info[] m_info;

        public void RecordTransaction(ItemData data, int count)
        {
            for (int i = 0; i < m_info.Length; i++)
            {
                var info = m_info[i];
                if (info.item == data)
                {
                    var absCount = Mathf.Abs(count);
                    for (int j = 0; j < absCount; j++)
                    {
                Lua.Run(info.luaScript);

                    }
                }
            }
        }
    }

    public class MerchantOneTimeBuyRecorder : MonoBehaviour
    {
        [SerializeField]
        private MerchantOneTimeBuyRecorderData m_data;

        private void OnItemUpdate(object sender, ItemEventArgs eventArgs)
        {
            if (eventArgs.countModification < 0)
            {
                m_data.RecordTransaction(eventArgs.data, eventArgs.countModification);
            }
        }

        private void Awake()
        {
            GetComponent<IMerchantStore>().InventoryItemUpdate += OnItemUpdate;
        }

    }
}