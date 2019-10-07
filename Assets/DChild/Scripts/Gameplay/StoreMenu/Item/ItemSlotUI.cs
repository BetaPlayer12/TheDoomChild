using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Item
{
    [RequireComponent(typeof(Button))]
    public class ItemSlotUI : MonoBehaviour
    {
        [ShowInInspector, OnValueChanged("UpdateInfo")]
        private ItemSlot m_slot;
        [SerializeField]
        private ItemSlotInfo m_info;
        [SerializeField, OnValueChanged("UpdateInteractability")]
        private bool m_isInteractable;
        private Button m_button;
        private Canvas m_canvas;

        public ItemData item => m_slot?.item ?? null;

        public void SetSlot(ItemSlot slot)
        {
            m_slot = slot;
            m_info.SetInfo(m_slot);
        }

        [Button,HideInEditorMode]
        public void UpdateInfo()
        {
            m_info.SetInfo(m_slot);
        }

        public void SetInteractable(bool value)
        {
            if (m_isInteractable != value)
            {
                m_isInteractable = value;
                UpdateInteractability();
            }
        }

        public void Show()
        {
            m_canvas.enabled = true;
        }

        public void Hide()
        {
            m_canvas.enabled = false;
        }

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
            m_canvas = GetComponent<Canvas>();
            UpdateInteractability();
        }

        private void Start()
        {
            if (m_slot != null)
            {
                m_info.SetInfo(m_slot);
            }
        }
    }
}
