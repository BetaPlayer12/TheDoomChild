using DChild.Temp;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DChild.Menu
{
    [RequireComponent(typeof(UIContainer), typeof(CanvasGroup))]
    public class UIViewInteractableNavigationHandle : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_firstSelected;
        private CanvasGroup m_canvasGroup;
        private UIContainer m_uiView;

        private GameObject m_previousSelectedGameObject;
        private void OnHideStart(GameObject obj)
        {
            m_canvasGroup.interactable = false;
            EventSystem.current.SetSelectedGameObject(m_firstSelected);
        }

        private void OnShowFinished(GameObject obj)
        {
            m_previousSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(m_firstSelected);
            m_canvasGroup.interactable = true;
        }

        private void Start()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
            m_uiView = GetComponent<UIContainer>();
            m_canvasGroup.interactable = m_uiView.isHidden == false && m_uiView.isHiding == false;
            GameEventMessage.Send();
            //m_uiView.ShowBehavior.OnFinished.Action += OnShowFinished;
            //m_uiView.HideBehavior.OnStart.Action += OnHideStart;

        }
    }
}