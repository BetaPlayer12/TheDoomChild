using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerCharacterController : MonoBehaviour, IMainController
    {
        [SerializeField]
        private InputTranslator m_input;
        [SerializeField]
        private CharacterState m_state;
        [SerializeField]
        private GameObject m_character;

        #region Behaviours
        private Movement m_movement;
        private Crouch m_crouch;
        private Dash m_dash;
        private WallStick m_wallStick;
        #endregion

        public event EventAction<EventActionArgs> ControllerDisabled;

        public void Disable()
        {
            enabled = false;
            m_movement.Cancel();
            m_crouch.Cancel();
            m_dash.Cancel();
            m_wallStick.Cancel();
        }

        public void Enable()
        {
            enabled = true;
        }

        private void Awake()
        {
            m_movement = m_character.GetComponentInChildren<Movement>();
            m_crouch = m_character.GetComponentInChildren<Crouch>();
            m_dash = m_character.GetComponentInChildren<Dash>();
            m_wallStick = m_character.GetComponentInChildren<WallStick>();
        }


        private void Update()
        {
            if (m_state.waitForBehaviour)
                return;

            if (m_state.isGrounded)
            {
                HandleGroundBehaviour();
            }
            else
            {
                if (m_state.isWallSticked)
                {

                }
                else
                {
                    m_movement.Move(m_input.horizontalInput);
                    if (m_input.horizontalInput != 0)
                    {
                        if (m_wallStick.IsThereAWall())
                        {
                            m_wallStick.Execute();
                        }
                    }
                }
            }
        }

        private void HandleGroundBehaviour()
        {
            if (m_state.isCrouched)
            {
                m_movement.Move(m_input.horizontalInput);
                if (m_input.crouchHeld == false)
                {
                    if (m_crouch.IsThereNoCeiling())
                    {
                        m_crouch.Cancel();
                        m_movement.SwitchConfigTo(Movement.Type.Jog);
                    }
                }
            }
            else if (m_state.isDashing)
            {

            }
            else
            {
                //From Standing
                if (m_input.crouchHeld)
                {
                    m_crouch.Execute();
                    m_movement.SwitchConfigTo(Movement.Type.Crouch);
                }
                else if (m_input.dashPressed)
                {
                    m_movement.Cancel();
                    m_dash.Execute();
                }
                else
                {
                    m_movement.Move(m_input.horizontalInput);
                }
            }
        }
    }
}
