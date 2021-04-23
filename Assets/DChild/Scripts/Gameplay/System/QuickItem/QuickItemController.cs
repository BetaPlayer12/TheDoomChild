using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Gameplay.Inventories
{
    public class QuickItemController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput m_input;
        [SerializeField]
        private QuickItemHandle m_handle;

        private bool m_allowCycle;

        private void Awake()
        {
            var actionMap = m_input.actions.FindActionMap("Gameplay");
            var useAction = actionMap.FindAction("QuickItemUse");
            useAction.performed += OnUseAction;
            var cycleAction = actionMap.FindAction("QuickItemCycle");
            cycleAction.performed += OnCycleAction;
            m_allowCycle = true;
        }
        private void OnUseAction(InputAction.CallbackContext obj)
        {
            if (obj.ReadValue<float>() == 1)
            {
                if (GameplaySystem.isGamePaused == false && m_handle.hideUI == false)
                {
                    m_allowCycle = false;
                    m_handle.UseCurrentItem();
                }
            }
            else
            {
                m_allowCycle = true;
            }
        }

        private void OnCycleAction(InputAction.CallbackContext obj)
        {
            if (m_allowCycle)
            {
                if (GameplaySystem.isGamePaused == false && m_handle.hideUI == false)
                {
                    if (obj.ReadValue<float>() == -1)
                    {
                        m_handle.Previous();
                    }
                    else
                    {
                        m_handle.Next();
                    }
                }
            }
        }


        //private void Update()
        //{
        //    if (m_hasPressed)
        //    {
        //        if (Input.GetButtonUp(m_pressedButton))
        //        {
        //            m_hasPressed = false;
        //        }
        //    }
        //    else
        //    {
        //        if (GameplaySystem.isGamePaused == false && m_handle.hideUI == false)
        //        {
        //            if (Input.GetButtonDown(m_useButton))
        //            {
        //                m_handle.UseCurrentItem();
        //                m_pressedButton = m_useButton;
        //            }
        //            if (Input.GetButtonDown(m_prevButton))
        //            {
        //                m_handle.Previous();
        //                m_pressedButton = m_prevButton;
        //            }
        //            else if (Input.GetButtonDown(m_nextButton))
        //            {
        //                m_handle.Next();
        //                m_pressedButton = m_prevButton;
        //            }
        //        }
        //    }
        //}
    }
}
