using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
#endif

namespace DChild.Menu.Bestiary
{
    public class BestiaryIndexButton : MonoBehaviour
    {
        //Bestiary Data and isInteracble should be Not serialized later on

        [SerializeField, OnValueChanged("UpdateInfo")]
        private BestiaryData m_data;
        [SerializeField]
        private BestiaryIndexInfoUI m_info;
        private UIButton m_button;
        private CanvasGroup m_canvas;

        public BestiaryData data => m_data;

        public void SetState(UISelectionState state)
        {
            m_button.SetState(state);
        }

        public void Show()
        {
            m_button.gameObject.SetActive(true);
        }

        public void Hide()
        {
            m_button.gameObject.SetActive(false);
        }

        public void SetData(BestiaryData data)
        {
            if (m_data != data)
            {
                m_data = data;
                m_info?.SetInfo(data);
            }
        }

        public void SetInteractable(bool isInteractable)
        {
#if UNITY_EDITOR
            if (m_button == null)
            {
                m_button = GetComponent<UIButton>();
            }
#endif
            m_button.interactable = isInteractable;
        }

        private void Awake()
        {
            m_button = GetComponent<UIButton>();
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