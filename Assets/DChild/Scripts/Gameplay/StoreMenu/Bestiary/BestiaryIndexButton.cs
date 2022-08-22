using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
#endif

namespace DChild.Menu.Bestiary
{
    [RequireComponent(typeof(Button))]
    public class BestiaryIndexButton : MonoBehaviour
    {
        //Bestiary Data and isInteracble should be Not serialized later on

        [SerializeField, OnValueChanged("UpdateInfo")]
        private BestiaryData m_data;
        [SerializeField]
        private BestiaryIndexInfoThing m_info;
        private Button m_button;
        [SerializeField, OnValueChanged("UpdateInteractability")]
        private bool m_isInteractable;

        public BestiaryData data => m_data;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
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
            m_isInteractable = isInteractable;
            UpdateInteractability();
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
                m_info.SetAsKnownCreature();
            }
            else
            {
                m_info.SetAsUnknownCreature();
            }
        }

        private void Awake()
        {
            m_button = GetComponent<Button>();
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