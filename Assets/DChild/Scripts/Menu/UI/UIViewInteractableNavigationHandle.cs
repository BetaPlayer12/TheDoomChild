using Doozy.Engine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DChild.Menu
{
    [RequireComponent(typeof(UIView), typeof(CanvasGroup))]
    public class UIViewInteractableNavigationHandle : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_firstSelected;
        private CanvasGroup m_canvasGroup;
        private UIView m_uiView;

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
            m_uiView = GetComponent<UIView>();
            m_canvasGroup.interactable = m_uiView.IsHidden == false && m_uiView.IsHiding == false;

            m_uiView.ShowBehavior.OnFinished.Action += OnShowFinished;
            m_uiView.HideBehavior.OnStart.Action += OnHideStart;
        }
    }
}