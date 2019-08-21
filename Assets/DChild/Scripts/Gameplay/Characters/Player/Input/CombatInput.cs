﻿using UnityEngine;

namespace DChild.Inputs
{
    [System.Serializable]
    public struct CombatInput : IInput
    {
        private const string INPUT_MAINHAND = "MainHand";
        private const string INPUT_OFFHAND = "OffHand";
        private const string INPUT_THROWPROJECTILE = "ThrowProjectile";

        private bool m_isMainHandPressed;
        private bool m_isOffHandPressed;
        private bool m_isThrowProjectilePressed;
        private bool m_isThrowProjectileHeld;
        private float m_timer;

        public bool isMainHandPressed => m_isMainHandPressed;
        public bool isOffHandPressed => m_isOffHandPressed;
        public bool isThrowProjectilePressed => m_isThrowProjectilePressed;
        public bool isThrowProjectileHeld => m_isThrowProjectileHeld;

        public void Disable()
        {
            m_isMainHandPressed = false;
            m_isOffHandPressed = false;
            m_isThrowProjectilePressed = false;
            m_isThrowProjectileHeld = false;
           
        }

        public void Update()
        {
            m_isMainHandPressed = Input.GetButtonDown(INPUT_MAINHAND);
            m_isOffHandPressed = Input.GetButtonDown(INPUT_OFFHAND);
            var throwBombHeld = Input.GetButton(INPUT_THROWPROJECTILE);
            if (throwBombHeld && !m_isThrowProjectileHeld && !m_isThrowProjectilePressed)
            {
                m_isThrowProjectilePressed = true;
            }
            else
            {
                m_isThrowProjectilePressed = false;
            }
            m_isThrowProjectileHeld = throwBombHeld;

            if (m_isThrowProjectilePressed)
            {

            }
            else if (m_isThrowProjectileHeld)
            {

            }

            if (Input.GetMouseButton(1))
            {
                m_timer += Time.deltaTime;
                Debug.Log("Pressed left click. Timer: " + m_timer);
            }
            else
            {
                
                if(m_timer > 3.0f)
                {
                    Debug.Log("Charge Attack!!! POWER!!!!!!!");
                }
                else
                {
                    Debug.Log("Normal Attack!!!!!!");
                    Debug.Log("Timer: " + m_timer);
                }
                
                m_timer = 0.0f;
            }



        }
    }
}
