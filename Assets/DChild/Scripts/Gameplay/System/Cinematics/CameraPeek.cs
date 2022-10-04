using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static DChild.Gameplay.Cinematics.Cinema;

namespace DChild.Gameplay.Cinematics
{
    public class CameraPeek : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference m_verticalInput;
        [SerializeField, MinValue(0)]
        private float m_holdForPeek;
        private float m_holdTime;

        private bool m_isPeeking;

        //private void LateUpdate()
        //{
        //    var value = Input.GetAxisRaw("Vertical");
        //    switch (value)
        //    {
        //        case 0:
        //            m_holdTime = 0;
        //            if (m_isPeeking)
        //            {
        //                GameplaySystem.cinema.ApplyCameraPeekMode(CameraPeekMode.None);
        //                m_isPeeking = false;
        //            }
        //            break;
        //        case 1:
        //            if (m_isPeeking == false)
        //            {
        //                EvaluatePeek(CameraPeekMode.Up);
        //            }
        //            break;
        //        case -1:
        //            if (m_isPeeking == false)
        //            {
        //                EvaluatePeek(CameraPeekMode.Down);
        //            }
        //            break;
        //    }
        //}

        private void EvaluatePeek(CameraPeekMode peek)
        {
            m_holdTime += Time.deltaTime;
            if (m_holdTime >= m_holdForPeek)
            {
                GameplaySystem.cinema.ApplyCameraPeekMode(peek);
                m_isPeeking = true;
            }
        }

        private void OnActionPerformed(InputAction.CallbackContext obj)
        {
            var value = obj.ReadValue<float>();
            switch (value)
            {
                case 0:
                    m_holdTime = 0;
                    if (m_isPeeking)
                    {
                        GameplaySystem.cinema.ApplyCameraPeekMode(CameraPeekMode.None);
                        m_isPeeking = false;
                    }
                    break;
                case 1:
                    if (m_isPeeking == false)
                    {
                        GameplaySystem.cinema.ApplyCameraPeekMode(CameraPeekMode.Up);
                        m_isPeeking = true;
                    }
                    break;
                case -1:
                    if (m_isPeeking == false)
                    {
                        GameplaySystem.cinema.ApplyCameraPeekMode(CameraPeekMode.Down);
                        m_isPeeking = true;
                    }
                    break;
            }
        }

        private void Start()
        {
            m_verticalInput.action.performed += OnActionPerformed;
        }


    }
}