﻿using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Menu.Inputs
{
    public class InputControlsDetector : MonoBehaviour
    {
        [SerializeField, OnValueChanged("ForceDeviceChange")]
        private bool m_hasGamepad;
        [SerializeField]
        private bool m_getRecentActiveDevice;
        private InputDevice m_currentGamepad;
        private bool m_isUsingGamepad;

        public event EventAction<EventActionArgs> InputControlChange;

        public bool isUsingGamepad => m_isUsingGamepad;

        private void UpdateCurrentActiveDevice()
        {
            if (m_getRecentActiveDevice)
            {
                if (m_isUsingGamepad)
                {
                    if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.wasUpdatedThisFrame)
                    {
                        m_isUsingGamepad = false;
                        InputControlChange?.Invoke(this, EventActionArgs.Empty);
                    }
                }
                else
                {
                    if (Gamepad.current?.wasUpdatedThisFrame ?? false)
                    {
                        m_isUsingGamepad = true;
                        InputControlChange?.Invoke(this, EventActionArgs.Empty);
                    }
                }
            }
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange arg2)
        {

            switch (arg2)
            {
                case InputDeviceChange.Added:
                case InputDeviceChange.Reconnected:
                case InputDeviceChange.Enabled:
                    if (m_hasGamepad == false)
                    {
                        var currentGamepad = Gamepad.current.device;
                        if (currentGamepad != null)
                        {
                            m_hasGamepad = true;
                            m_isUsingGamepad = true;
                            m_currentGamepad = currentGamepad;
                            InputControlChange?.Invoke(this, EventActionArgs.Empty);
                        }
                    }
                    break;

                case InputDeviceChange.Removed:
                case InputDeviceChange.Disconnected:
                case InputDeviceChange.Disabled:
                    if (m_hasGamepad)
                    {
                        if (m_currentGamepad == device)
                        {
                            m_hasGamepad = false;
                            m_isUsingGamepad = false;
                            m_currentGamepad = null;
                            InputControlChange?.Invoke(this, EventActionArgs.Empty);
                        }
                    }
                    break;
            }
        }

        private void ForceDeviceChange()
        {
            m_isUsingGamepad = m_hasGamepad;
            InputControlChange?.Invoke(this, EventActionArgs.Empty);
        }

        private void OnEnable()
        {
            InputSystem.onDeviceChange += OnDeviceChange;
        }
        public void Update()
        {
            if (m_hasGamepad)
            {
                UpdateCurrentActiveDevice();
            }
        }

        private void OnDisable()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
        }
    }
}