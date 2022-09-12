using Doozy.Engine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace DChild.Menu.Campaign
{
    public class CampaignSelectController : MonoBehaviour
    {
        [SerializeField]
        private InputSystemUIInputModule m_input;
        [SerializeField]
        private CampaignSelect m_campaignSelect;
        [SerializeField]
        private UIButton m_previousButton;
        [SerializeField]
        private UIButton m_nextButton;

        [SerializeField]
        private GameObject m_newGameFirstSelectedUI;
        [SerializeField]
        private GameObject m_loadGameFirstSelectedUI;

        private bool m_hasSelectedASlot;
        private bool m_usCurrentSlotANewGame;
        private EventSystem m_eventSystem;

        private void OnCampaignSelected(object sender, SelectedCampaignSlotEventArgs eventArgs)
        {
            m_usCurrentSlotANewGame = eventArgs.isNewGame;
            if (m_hasSelectedASlot)
            {
                UpdateSelectedUI();
            }
        }

        private void OnMoveSlotRequest(InputAction.CallbackContext obj)
        {
            if (m_hasSelectedASlot == false)
            {
                var moveInput = obj.ReadValue<Vector2>().x;
                if (moveInput > 0)
                {
                    m_nextButton.ExecuteClick();
                    m_nextButton.Button.onClick.Invoke();
                }
                else if (moveInput < 0)
                {
                    m_previousButton.ExecuteClick();
                    m_previousButton.Button.onClick.Invoke();
                }
            }
        }
        private void OnShoulderButton(InputAction.CallbackContext obj)
        {
            throw new NotImplementedException();
        }

        private void OnCancel(InputAction.CallbackContext obj)
        {
            m_hasSelectedASlot = false;
        }
        private void OnSubmit(InputAction.CallbackContext obj)
        {
            if (m_hasSelectedASlot == false)
            {
                UpdateSelectedUI();
            }
        }

        private void UpdateSelectedUI()
        {
            m_hasSelectedASlot = true;
            if (m_usCurrentSlotANewGame)
            {
                m_eventSystem.SetSelectedGameObject(m_newGameFirstSelectedUI);
            }
            else
            {
                m_eventSystem.SetSelectedGameObject(m_loadGameFirstSelectedUI);
            }
        }

        private void Awake()
        {
            m_campaignSelect.CampaignSelected += OnCampaignSelected;
            m_eventSystem = EventSystem.current;
        }


        private void OnEnable()
        {
            m_input.cancel.action.performed += OnCancel;
            m_input.submit.action.performed += OnSubmit;
            m_input.move.action.performed += OnMoveSlotRequest;
            m_input.actionsAsset.FindAction("UI/ShoulderButton").performed += OnShoulderButton;
        }


        private void OnDisable()
        {
            m_input.cancel.action.performed -= OnCancel;
            m_input.submit.action.performed -= OnSubmit;
            m_input.move.action.performed -= OnMoveSlotRequest;
            m_input.actionsAsset.FindAction("UI/ShoulderButton").performed -= OnShoulderButton;
        }

    }

}