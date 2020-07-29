using DChild.Gameplay.Characters.Players.Behaviour;
using PlayerNew;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [AddComponentMenu("DChild/Gameplay/Player/Controller/Player Character Override")]
    public class PlayerCharacterOverride : MonoBehaviour
    {
        [SerializeField, Range(-1f, 1f)]
        private float m_moveDirectionInput;

        [Title("Modules")]
        [SerializeField]
        private InputState m_input;

        public float moveDirectionInput { set { m_moveDirectionInput = Mathf.Clamp(value, -1f, 1f); } }

        private void Awake()
        {
            enabled = false;
        }

        private void OnDisable()
        {
            m_moveDirectionInput = 0;
        }

        private void FixedUpdate()
        {
            m_input.horizontal = m_moveDirectionInput;

            //m_input.SetButtonValue(Buttons.Left, m_moveDirectionInput < 0);
            //m_input.SetButtonValue(Buttons.Right, m_moveDirectionInput > 0);
        }
    } 
}
