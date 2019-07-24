using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Refactor.DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class Crouch : MonoBehaviour, IComplexCharacterModule, IControllableModule
    {
        //[SerializeField]
        //private SpineRootMotion m_rootMotion;
        private RaySensor m_headSensor;
        private Animator m_animator;
        private string m_crouchParameter;
        private ICrouchState m_state;

        public bool HandleCrouch(bool input)
        {
            if (input)
            {
                //EnableRootMotion(true, true, false);
                m_headSensor.enabled = true;
                m_animator.SetBool(m_crouchParameter, true);
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
                m_animator.SetBool(m_crouchParameter, false);
                m_state.isCrouched = false;
                //EnableRootMotion(false, false, false);
            }
        }

        public void ConnectTo(IMainController controller)
        {
            var crouchController = controller.GetSubController<ICrouchController>();
            crouchController.CrouchCall += OnCrouchCall;
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_headSensor = info.GetSensor(PlayerSensorList.SensorType.Head);
            m_headSensor.enabled = false;
            m_animator = info.animator;
            m_crouchParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsCrouching);
            m_state = info.state;
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