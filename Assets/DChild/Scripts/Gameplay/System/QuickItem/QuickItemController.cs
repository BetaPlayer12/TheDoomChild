﻿using DChild.Gameplay.Combat;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Gameplay.Inventories.QuickItem
{
    public class QuickItemController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput m_input;
        [SerializeField]
        private InputActionReference m_itemUse;
        [SerializeField]
        private InputActionReference m_itemCycle;
        [SerializeField]
        private QuickItemHandle m_handle;

        private bool m_allowCycle;
        private IDamageable m_playerDamageable;
        private bool m_isEnabled;

        public bool isEnabled => m_isEnabled;

        public void SetEnable(bool isEnable) => m_isEnabled = isEnable;

        private void OnUseAction(InputAction.CallbackContext obj)
        {
            if (obj.ReadValue<float>() == 1)
            {
                if (m_isEnabled)
                {
                    if (GameplaySystem.isGamePaused == false && m_handle.hideUI == false && m_playerDamageable.isAlive)
                    {
                        m_allowCycle = false;
                        m_handle.UseCurrentItem();
                    }
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
                    if (m_isEnabled)
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
        }

        private void Awake()
        {
            var actionMap = m_input.actions.FindActionMap("Gameplay");
            var itemUse = m_itemUse.action;
            var itemCycle = m_itemCycle.action;
            actionMap.FindAction(itemUse.id).performed += OnUseAction;
            actionMap.FindAction(itemCycle.id).performed += OnCycleAction;
            m_allowCycle = true;
            m_isEnabled = true;

            m_playerDamageable = GameplaySystem.playerManager.player.damageableModule;
        }

    }
}
