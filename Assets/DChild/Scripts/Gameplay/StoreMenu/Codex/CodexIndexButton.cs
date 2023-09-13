using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Menu.Codex
{
    public abstract class CodexIndexButton<DatabaseAssetType> : MonoBehaviour where DatabaseAssetType : DatabaseAsset
    {
        [SerializeField, OnValueChanged("UpdateInfo")]
        protected DatabaseAssetType m_data;

        private UIToggle m_button;
        private CanvasGroup m_canvas;


        public bool isAvailable => m_button.gameObject.activeInHierarchy && m_button.interactable;
        public DatabaseAssetType data => m_data;
        public abstract void SetData(DatabaseAssetType data);

        public void SetIsOn(bool isOn)
        {
            if (m_button.isOn != isOn)
            {
                m_button.SetIsOn(isOn);
                if (isOn)
                {
                    m_button.Select();
                }
                m_button.SendSignal(isOn);
            }
        }

        public void Show()
        {
            m_button.gameObject.SetActive(true);
        }

        public void Hide()
        {
            m_button.gameObject.SetActive(false);
        }


        public void SetInteractable(bool isInteractable)
        {
#if UNITY_EDITOR
            if (m_button == null)
            {
                m_button = GetComponent<UIToggle>();
            }
#endif
            m_button.interactable = isInteractable;
        }

        private void Awake()
        {
            m_button = GetComponent<UIToggle>();
        }
    }

    public abstract class CodexIndexButton<DatabaseAssetType, IndexInfoType> : CodexIndexButton<DatabaseAssetType> where DatabaseAssetType : DatabaseAsset, IndexInfoType
    {
        [SerializeReference]
        private CodexIndexInfoUI<IndexInfoType> m_info;

        public override void SetData(DatabaseAssetType data)
        {
            if (m_data != data)
            {
                m_data = data;
                m_info?.SetInfo(data);
            }
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