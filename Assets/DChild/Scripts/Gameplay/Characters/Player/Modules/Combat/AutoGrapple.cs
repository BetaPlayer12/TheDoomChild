using System;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Environment;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IAutoGrappleController
    {
        event EventAction<ControllerEventArgs> UpdateCall;
        event EventAction<ControllerEventArgs> FixedUpdateCall;
        event EventAction<EventActionArgs> AutoGrappleCall;
    }

    public class AutoGrapple : MonoBehaviour, IPlayerExternalModule
    {
        [SerializeField]
        private GrappleSensor m_sensor;
        [SerializeField,HideLabel]
        private GrappleDashHandler m_dashHandler;

        private bool m_isDashing;
        private bool m_isChoosingTarget;
        private CharacterPhysics2D m_physics;
        private IGrappleObject m_grappleTarget;

        public void Initialize(IPlayerModules player)
        {
            m_isDashing = false;
            m_isChoosingTarget = false;
            m_physics = player.physics;
            var controller = GetComponentInParent<IAutoGrappleController>();
            controller.AutoGrappleCall += OnAutoGrappleCall;
            controller.UpdateCall += OnUpdateCall;
            controller.FixedUpdateCall += OnFixedUpdateCall;
        }

        public void ChangeGrappleTarget(Inputs.DirectionalInput input)
        {
            if (input.isUpPressed)
            {
                SelectAsTarget(m_sensor.GetNearestTopObjectFrom(m_grappleTarget));
            }
            else if (input.isDownPressed)
            {
                SelectAsTarget(m_sensor.GetNearestBottomObjectFrom(m_grappleTarget));
            }
            else if (input.isRightPressed)
            {
                SelectAsTarget(m_sensor.GetNearestRightObjectFrom(m_grappleTarget));
            }
            else if (input.isLeftPressed)
            {
                SelectAsTarget(m_sensor.GetNearestLeftObjectFrom(m_grappleTarget));
            }
        }

        public void SelectGrappleTarget()
        {
            m_grappleTarget = m_sensor.GetClosestObject();
            m_isChoosingTarget = true;
            GameplaySystem.playerManager.autoReflex.AutoReflexEnd += OnAutoReflexEnd;
            GameplaySystem.playerManager.autoReflex.StartAutoReflex();
        }

        private void OnAutoReflexEnd(object sender, EventActionArgs eventArgs)
        {
            DashToGrappleTarget();       
        }

        public void DashToGrappleTarget()
        {
            m_dashHandler.Set(transform.position, m_grappleTarget.position);
            m_isChoosingTarget = false;
            m_isDashing = true;
            GameplaySystem.playerManager.autoReflex.AutoReflexEnd -= OnAutoReflexEnd;
            GameplaySystem.playerManager.autoReflex.StopAutoReflex();
        }

        private void SelectAsTarget(IGrappleObject grappleObject)
        {
            m_grappleTarget = grappleObject;
        }

        private void OnUpdateCall(object sender, ControllerEventArgs eventArgs)
        {
            if (m_isChoosingTarget)
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    DashToGrappleTarget();
                }
                else
                {
                    ChangeGrappleTarget(eventArgs.input.direction);
                }
            }
        }

        private void OnFixedUpdateCall(object sender, ControllerEventArgs eventArgs)
        {
            if (m_isDashing)
            {
                m_dashHandler.DashToDestination(m_physics, transform.position, ref m_isDashing);
            }
        }

        private void OnAutoGrappleCall(object sender, EventActionArgs eventArgs)
        {
            SelectGrappleTarget();
        }
    }
}