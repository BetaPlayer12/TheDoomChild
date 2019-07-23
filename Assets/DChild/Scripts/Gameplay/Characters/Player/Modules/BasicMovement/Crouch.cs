using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class Crouch : MonoBehaviour, IPlayerExternalModule, IEventModule
    {
        //[SerializeField]
        //private SpineRootMotion m_rootMotion;
        private RaySensor m_headSensor;
        private Animator m_animator;
        private ICrouchState m_state;

        public bool HandleCrouch(bool input)
        {
            if (input)
            {
                //EnableRootMotion(true, true, false);
                m_headSensor.enabled = true;
                m_animator.SetBool("Crouch", true);
                m_state.isCrouched = true;
            }
            else if (m_state.isCrouched)
            {
                m_headSensor.Cast();

                if (m_headSensor.isDetecting)
                {
                    //EnableRootMotion(true, true, false);
                    return m_state.isCrouched;
                }
                else
                {
                    StopCrouch();
                }
            }
            else
            {
                m_headSensor.enabled = false;
            }
            return m_state.isCrouched;
        }

        public void StopCrouch()
        {
            if (m_state.isCrouched)
            {
                m_animator.SetBool("Crouch", false);
                m_state.isCrouched = false;
                //EnableRootMotion(false, false, false);
            }
        }

        public void Initialize(IPlayerModules player)
        {
            m_headSensor = player.sensors.headSensor;
            m_headSensor.enabled = false;
            m_state = player.characterState;
            //m_animator = player.animation.GetComponentInChildren<Animator>();
            //m_rootMotion.enabled = false;
        }

        public void ConnectEvents()
        {
            GetComponentInParent<ICrouchController>().CrouchCall += OnCrouchCall;
        }

        private void OnCrouchCall(object sender, ControllerEventArgs eventArgs)
        {
            HandleCrouch(eventArgs.input.direction.isDownHeld);
        }

        //private void EnableRootMotion(bool enable, bool useX, bool useY)
        //{
        //    m_rootMotion.enabled = enable;
        //    if (enable)
        //    {
        //        m_rootMotion.useX = useX;
        //        m_rootMotion.useY = useY;
        //    }
        //}
    }

}