using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Item
{
    [RequireComponent(typeof(Button))]
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField, OnValueChanged("UpdateInfo")]
        private ItemData m_data;
        [SerializeField]
        private ItemSlotInfo m_info;
        private Button m_button;
        [SerializeField, OnValueChanged("UpdateInteractability")]
        private bool m_isInteractable;

        public ItemData data => m_data;

        private void UpdateInteractability()
        {
#if UNITY_EDITOR
            if (m_button == null)
            {
                m_button = GetComponent<Button>();
            }
#endif
            m_button.interactable = m_isInteractable;
            if (m_isInteractable)
            {
                m_info.Show();
            }
            else
            {
                m_info.Hide();
            }
        }

        private void Awake()
        {
            m_button = GetComponent<Button>();
            UpdateInteractability();
        }

        private void Start()
        {
            if (m_data != null)
            {
                m_info.SetInfo(m_data);
            }
        }

#if UNITY_EDITOR
        [Button]
        private void UpdateInfo()
        {
            m_info.SetInfo(m_data);
        }
#endif
    }
}
