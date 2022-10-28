using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class ShardNotificationUI : MonoBehaviour
    {
        [SerializeField]
        private ItemData m_shardToMonitor;
        [SerializeField, MinValue(1)]
        private int m_amountToComplete;
        [SerializeField, Tooltip("Use " + TAG_CURRENTAMOUNT + " to show Current Amount and " + TAG_MAXAMOUNT + " to show Needed Amount"), OnValueChanged("OnMessageChange"), TextArea]
        private string m_message;
        [SerializeField]
        private TextMeshProUGUI m_messageLabel;
        [SerializeField]
        private GameObject m_completedShardUI;
        [SerializeField]
        private GameObject[] m_incompleteShardUIs;

        private const string TAG_CURRENTAMOUNT = "<amount>";
        private const string TAG_MAXAMOUNT = "<maxAmount>";

        private void OnItemUpdate(object sender, ItemEventArgs eventArgs)
        {
            if (eventArgs.data == m_shardToMonitor && eventArgs.countModification > 0)
            {
                if (eventArgs.currentCount >= m_amountToComplete)
                {
                    m_completedShardUI.SetActive(true);
                    for (int i = 0; i < m_incompleteShardUIs.Length; i++)
                    {
                        m_incompleteShardUIs[i].SetActive(false);
                    }
                }
                else
                {
                    m_completedShardUI.SetActive(false);
                    int i = 0;
                    for (; i < eventArgs.currentCount; i++)
                    {
                        m_incompleteShardUIs[i].SetActive(true);
                    }
                    for (; i < m_incompleteShardUIs.Length; i++)
                    {
                        m_incompleteShardUIs[i].SetActive(false);
                    }
                }

                UpdateMessage(eventArgs.currentCount, m_amountToComplete);
            }
        }

        private void UpdateMessage(int currentAmount, int maxAmount)
        {
            var message = m_message.Replace(TAG_CURRENTAMOUNT, currentAmount.ToString());
            message = message.Replace(TAG_MAXAMOUNT, maxAmount.ToString());
            m_messageLabel.text = message;
        }

        private void OnMessageChange()
        {
            UpdateMessage(1, m_amountToComplete);
        }

        private void OnEnable()
        {
            var inventory = GameplaySystem.playerManager.player.inventory;
            inventory.InventoryItemUpdate += OnItemUpdate;
        }

        private void OnDisable()
        {
            var inventory = GameplaySystem.playerManager.player.inventory;
            inventory.InventoryItemUpdate -= OnItemUpdate;
        }
    }

}