﻿using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Refactor.DChild.Gameplay.Characters.Players;
using Spine.Unity.Modules;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class CrouchMovement : MonoBehaviour, IComplexCharacterModule, IControllableModule
    {
        [SerializeField]
        private GroundMoveHandler m_moveHandler;
        private Character m_character;
        private CharacterPhysics2D m_characterPhysics2D;
        private Animator m_animator;
<<<<<<< HEAD

=======
        private string m_speedParameter;
>>>>>>> 8a39fd8cd7f0cc51ad8649a6c0042de0166c2f82
        private ICrouchState m_state;

        public void Move(float direction)
        {
            if (m_state.isCrouched)
            {
                if (direction == 0)
                {
                    m_animator.SetInteger(m_speedParameter, 0);
                    m_moveHandler.Deccelerate();
                    m_state.isMoving = false;
                }
                else
                {
                    var moveDirection = direction > 0 ? Vector2.right : Vector2.left;
                    m_moveHandler.SetDirection(moveDirection);
                    m_moveHandler.Accelerate();
                    m_state.isMoving = true;
                    m_animator.SetInteger(m_speedParameter, 1);
                    m_character.SetFacing(direction > 0 ? HorizontalDirection.Right : HorizontalDirection.Left);
                }
            }
        }

        public void ConnectEvents()
        {
            GetComponentInParent<ICrouchController>().CrouchMoveCall += OnCrouchMoveCall;
        }

        private void OnCrouchMoveCall(object sender, ControllerEventArgs eventArgs)
        {

            //m_moveHandler.Deccelerate();
            Move(eventArgs.input.direction.horizontalInput);
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_characterPhysics2D = info.physics;
            m_moveHandler.SetPhysics(m_characterPhysics2D);
            m_state = info.state;
            m_animator = info.animator;
            m_speedParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX);
        }

        public void ConnectTo(IMainController controller)
        {
            controller.GetSubController<ICrouchController>().CrouchMoveCall += OnCrouchMoveCall;
        }

    }

}