using DChild.Gameplay.Inventories;
using DChild.Gameplay.UI;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Items
{
    public class KeystoneFragmentUI : SerializedMonoBehaviour, IItemNotificationUI
    {
        [System.Serializable]
        private class Info
        {
            [SerializeField]
            private ItemData m_item;
            [SerializeField]
            private UIContainer m_view;

            public ItemData item => m_item;
            public UIContainer view => m_view;
        }

        [System.Serializable]
        private struct Command
        {
            public bool isShown;
            public bool instantAction;
        }

        [SerializeField]
        private UIContainer m_container;
        [SerializeField]
        private Info[] m_infos;
        [SerializeField, ListDrawerSettings(HideRemoveButton = true, HideAddButton = true), HideInEditorMode]
        private Command[] m_commands;

        private int m_previousAcquiredIndex;

        public void AddListenerToOnNotificationHidden(UnityAction action)
        {
            m_container.OnHiddenCallback.Event.AddListener(action);
        }

        public bool IsNotificationFor(ItemData itemData)
        {
            for (int i = 0; i < m_infos.Length; i++)
            {
                if (m_infos[i].item == itemData)
                {
                    return true;
                }
            }
            return false;
        }

        public void ShowNotificationFor(ItemData itemData)
        {
            m_container.Show();
            ExecuteCommands();
        }

        public void ExecuteCommands()
        {
            for (int i = 0; i < m_commands.Length; i++)
            {
                var command = m_commands[i];
                var info = m_infos[i];
                if (command.isShown)
                {
                    if (command.instantAction)
                    {
                        info.view.InstantShow();
                    }
                    else
                    {
                        info.view.Show();
                    }
                }
                else
                {
                    info.view.InstantHide();
                }
            }
        }

        public void HideAll()
        {
            for (int i = 0; i < m_commands.Length; i++)
            {
                m_infos[i].view.InstantHide();
            }
        }

        private void Initialize()
        {
            for (int i = 0; i < m_infos.Length; i++)
            {
                //m_commands[i].isShown = m_inventory.GetCurrentAmount(m_infos[i].item) != 0;
                m_commands[i].instantAction = true;
            }
        }

        private void OnCampaignLoaded(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            Initialize();
        }

        private void ItemUpdated(object sender, ItemEventArgs eventArgs)
        {
            if (eventArgs.currentCount > 0)
            {
                for (int i = 0; i < m_infos.Length; i++)
                {
                    if (m_infos[i].item == eventArgs.data)
                    {
                        if (m_previousAcquiredIndex != -1)
                        {
                            m_commands[m_previousAcquiredIndex].instantAction = true;
                        }

                        m_commands[i].isShown = true;
                        m_commands[i].instantAction = false;
                        m_previousAcquiredIndex = i;
                    }
                }
            }
        }

        private void Awake()
        {
            m_commands = new Command[m_infos.Length];
            m_previousAcquiredIndex = -1;
        }

        private void Start()
        {
            GameplaySystem.playerManager.player.inventory.InventoryItemUpdate += ItemUpdated;
            GameplaySystem.campaignSerializer.PostDeserialization += OnCampaignLoaded;
            Initialize();
        }

       
    }
}
