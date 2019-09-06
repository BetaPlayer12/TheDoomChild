using System;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using Spine;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class BasicAttack : MonoBehaviour, IComplexCharacterModule, IControllableModule
    {
        private ICombatState m_state;
        private Animator m_animator;
        private IsolatedPhysics2D m_physics;
        private string m_attackTriggerParameter;
        private string m_attackDirectionParameter;
        [SerializeField]
        Transform circleRadius;
        [SerializeField]
        private float circle_rad;
        [SerializeField]
        LayerMask grabLayer;


        public void Initialize(ComplexCharacterInfo info)
        {
            m_state = info.state;
            m_animator = info.animator;
            m_physics = info.physics;
            m_attackTriggerParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Attack);
            m_attackDirectionParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.AttackYDirection);
        }

        public void ConnectTo(IMainController controller)
        {
            controller.GetSubController<IBasicAttackController>().BasicAttackCall += OnBasicAttackCall;
            controller.GetSubController<IBasicAttackController>().WhipAttackCall += OnWhipAttackCall;
            controller.ControllerDisabled += OnControllerDisabled;
        }

        private void OnBasicAttackCall(object sender, CombatEventArgs eventArgs)
        {
            SetAttackDirection(eventArgs);
            m_animator.SetTrigger(m_attackTriggerParameter);
            m_state.canAttack = false;
            m_state.waitForBehaviour = true;
            if (m_state.isGrounded)
            {
                m_physics.SetVelocity(0, 0);
            }
            
            
        }
        private void OnWhipAttackCall(object sender, CombatEventArgs eventArgs)
        {
            SetAttackDirection(eventArgs);
            m_animator.SetTrigger(m_attackTriggerParameter);
            m_state.canAttack = false;
            m_state.waitForBehaviour = true;
            if (m_state.isGrounded)
            {
                m_physics.SetVelocity(0, 0);
            }
            
            Collider2D[] circleCollision = Physics2D.OverlapCircleAll(circleRadius.position, circle_rad, grabLayer);
            for(int x = 0; x < circleCollision.Length; x++)
            {
                
                GameObject itemGrab = circleCollision[x].gameObject;
                Debug.Log("Mouse position: " + Input.mousePosition);
                transform.root.position = Vector3.Lerp(this.transform.position, itemGrab.transform.position, 10.0f);
               
                //Debug.Log("distance:" + distanceV);
                //Debug.Log("x postion: " + itemGrab.transform.position);

            }
           // Debug.Log("Whip Attack "+grabLayer +":" + circleCollision.Length);

        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(circleRadius.position, circle_rad);

        }

        private void SetAttackDirection(CombatEventArgs eventArgs)
        {
           
            if (eventArgs.directionalInput.isDownHeld)
            {
                m_animator.SetInteger(m_attackDirectionParameter, -1);
            }
            else if (eventArgs.directionalInput.isUpHeld)
            {
                m_animator.SetInteger(m_attackDirectionParameter, 1);
            }
            else
            {
                m_animator.SetInteger(m_attackDirectionParameter, 0);
            }
        }

        private void OnControllerDisabled(object sender, EventActionArgs eventArgs)
        {
            m_state.canAttack = true;
            m_state.waitForBehaviour = false;
        }
    }
}