using DChild.Gameplay.Characters.Players.Behaviour;
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
        private MovementHandle m_movementhandle;
        [SerializeField]
        private MoveSpeedTransistor m_speedTransistor;

        public float moveDirectionInput { set { m_moveDirectionInput = Mathf.Clamp(value, -1f, 1f); } }

        private void Awake()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            m_speedTransistor.SwitchToJogSpeed();
        }

        private void FixedUpdate()
        {
            m_movementhandle.SetMovingSpeedParameter(1);
            m_movementhandle.Move(m_moveDirectionInput);
        }
    } 
}
