using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Inputs
{
    [AddComponentMenu("DChild/Gameplay/Player/Controller/Player Input Old")]
    public class PlayerInputOld : MonoBehaviour
    {
        private const string INPUT_JUMP = "Jump";
        private const string INPUT_INTERACT = "Interact";

        private DirectionalInput m_directionalInput;
        private MouseInput m_mouseInput;
        private bool m_isJumpPressed;
        private bool m_isJumpHeld;
        private SkillInput m_skillInput;
        private bool m_isInteractPressed;
        private CombatInput m_combatInput;

        public DirectionalInput direction => m_directionalInput;
        public MouseInput mouseInput => m_mouseInput;
        public bool isJumpPressed => m_isJumpPressed;
        public bool isJumpHeld => m_isJumpHeld;
        public SkillInput skillInput => m_skillInput;
        public bool isInteractPressed => m_isInteractPressed;
        public CombatInput combat => m_combatInput;

        private void GetJumpInputs()
        {
            m_isJumpPressed = Input.GetButtonDown(INPUT_JUMP);
            m_isJumpHeld = Input.GetButton(INPUT_JUMP);
        }

        public void Disable()
        {
            enabled = false;
            m_isJumpPressed = false;
            m_isJumpHeld = false;
            m_isInteractPressed = false;
            m_directionalInput.Disable();
            m_mouseInput.Disable();
            m_skillInput.Disable();
            m_combatInput.Disable();
        }

        public void Enable()
        {
            enabled = true;
        }

        private void Awake()
        {
            m_directionalInput = new DirectionalInput();
            m_mouseInput = new MouseInput();
            m_combatInput = new CombatInput();
        }

        private void Update()
        {
            m_directionalInput.Update();
            m_mouseInput.Update();
            GetJumpInputs();
            m_skillInput.Update();
            //m_isInteractPressed = Input.GetButtonDown(INPUT_INTERACT);
            m_combatInput.Update();
        }
    }
}
