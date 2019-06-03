using Holysoft.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Bestiary
{
    public class BestiaryIndexButton : NavigationButton
    {
#if UNITY_EDITOR
        [SerializeField, OnValueChanged("UpdateInfo")]
#endif
        private BestiaryData m_data;
        [SerializeField, OnValueChanged("InteractabilityChanged")]
        private bool m_isInteractable;
        [Title("Visual")]
        [SerializeField]
        private TextMeshProUGUI m_creatureNameText;
        [SerializeField]
        private Image m_creatureImage;
        private Button m_button;

        private IToggleVisual m_visual;

        public int creatureID => m_data.id;

        public void SetInteractability(bool value)
        {
            if (value != m_isInteractable)
            {
                m_isInteractable = value;
                m_button.interactable = value;
                m_visual.Toggle(m_isInteractable);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void SetInfo(BestiaryData info)
        {
            m_data = info;
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            m_buttonID = m_data.id;
            m_creatureNameText.text = m_data.creatureName.ToUpper();
            if (m_data.indexImage == null)
            {
                m_creatureImage.sprite = null;
                m_creatureImage.color = new Color(0, 0, 0, 0.5176471f);
            }
            else
            {
                m_creatureImage.sprite = m_data.indexImage;
                m_creatureImage.color = Color.white;
            }
            //gameObject.name = $"{info.name}IndexButton";
        }

        protected override void Awake()
        {
            base.Awake();
            m_button = GetComponent<Button>();
            m_visual = GetComponent<IToggleVisual>();
        }

#if UNITY_EDITOR
        private void InteractabilityChanged()
        {
            m_visual = GetComponent<IToggleVisual>();
            m_visual.Toggle(m_isInteractable);
        }
#endif
    }
}