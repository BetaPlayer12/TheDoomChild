using System;
using DChild.Gameplay.Characters.Players.Behaviour;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    //public interface IWhipGrappleController
    //{
    //    event EventAction<ControllerEventArgs> UpdateCall;
    //    event EventAction<ControllerEventArgs> FixedUpdateCall;
    //    event EventAction<EventActionArgs> WhipGrappleCall;
    //}

    //public class WhipGrapple : MonoBehaviour, IPlayerExternalModule
    //{
    //    [SerializeField]
    //    private GrappleHook m_hook;

    //    private bool m_launched;
    //    private bool m_hooked;
    //    private bool m_isDeciding;
    //    private bool m_pull;
    //    private bool m_updatePhysics;

    //    [SerializeField, HideLabel]
    //    private GrappleDashHandler m_dashHandler;
    //    public float m_pullSpeed;

    //    private Vector2 m_startPosition;
    //    private Vector2 m_destination;
    //    private Vector2 m_direction;
    //    private CharacterPhysics2D m_physics;
    //    private bool m_isDashing;

    //    public void Initialize(IPlayerModules player)
    //    {
    //        m_launched = false;
    //        m_hooked = false;
    //        m_isDeciding = false;
    //        m_pull = false;
    //        m_physics = player.physics;

    //        //m_hook.ObjectCatched += OnHookCatchedObject;
    //        //m_hook.HookReturned += OnHookReturned;

    //        var controller = GetComponentInParent<IAutoGrappleController>();
    //        controller.AutoGrappleCall += OnAutoGrappleCall;
    //        controller.UpdateCall += OnUpdateCall;
    //        controller.FixedUpdateCall += OnFixedUpdateCall;
    //    }



    //    private void OnFixedUpdateCall(object sender, ControllerEventArgs eventArgs)
    //    {
    //        if (m_updatePhysics)
    //        {
    //            if (m_pull)
    //            {
    //                //Pull Object using Physics
    //                //if (Vector2.Distance(m_hook.catchedObject.physics.position, m_destination) <= 0.5f)
    //                //{
    //                //    var velocity = m_hook.catchedObject.physics.velocity;
    //                //    velocity.y = 0;
    //                //    m_hook.catchedObject.physics.SetVelocity(velocity);
    //                //    ResetState();
    //                //}
    //                //else
    //                //{
    //                //    m_hook.catchedObject.physics.SetVelocity(m_direction * m_pullSpeed);
    //                //}
    //            }
    //            else
    //            {
    //                m_dashHandler.DashToDestination(m_physics, transform.position, ref m_isDashing);
    //                if (m_isDashing == false)
    //                {
    //                    ResetState();
    //                }
    //            }
    //        }
    //    }

    //    private void OnUpdateCall(object sender, ControllerEventArgs eventArgs)
    //    {
    //        if (m_launched)
    //        {
    //            if (m_hooked)
    //            {
    //                if (m_isDeciding)
    //                {
    //                    if (eventArgs.input.direction.isLeftPressed)
    //                    {
    //                        m_isDeciding = false;
    //                        m_updatePhysics = true;
    //                        m_pull = true;
    //                        m_hook.ResetState();

    //                        //m_startPosition = m_hook.catchedObject.position;
    //                        //m_direction = ((Vector2)transform.position - m_startPosition).normalized;
    //                        //m_destination = m_startPosition + (m_direction * m_hook.catchedObject.pullOffset);
    //                    }
    //                    else if (eventArgs.input.direction.isRightPressed)
    //                    {
    //                        m_isDeciding = false;
    //                        m_updatePhysics = true;
    //                        m_pull = false;

    //                        //var objectPos = m_hook.catchedObject.position;
    //                        //var toObject = objectPos - (Vector2)transform.position;
    //                        //m_dashHandler.Set(transform.position, objectPos + (toObject.normalized * m_hook.catchedObject.dashOffset));
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    private void OnAutoGrappleCall(object sender, EventActionArgs eventArgs)
    //    {
    //        m_hook.Launch(transform.right);
    //        m_launched = true;
    //    }

    //    private void OnHookReturned(object sender, EventActionArgs eventArgs)
    //    {
    //        ResetState();
    //    }

    //    //private void OnHookCatchedObject(object sender, GrappleHook.ObjectCatchedEventArgs eventArgs)
    //    //{
    //    //    if (eventArgs.canBePulled)
    //    //    {
    //    //        m_isDeciding = true;
    //    //    }
    //    //    else
    //    //    {
    //    //        m_isDeciding = false;
    //    //        m_pull = false;
    //    //        m_isDashing = true;
    //    //    }
    //    //    m_hooked = true;
    //    //}

    //    private void ResetState()
    //    {
    //        m_launched = false;
    //        m_hooked = false;
    //        m_isDeciding = false;
    //        m_pull = false;
    //        m_isDashing = false;
    //        m_updatePhysics = false;
    //    }
    //}
}