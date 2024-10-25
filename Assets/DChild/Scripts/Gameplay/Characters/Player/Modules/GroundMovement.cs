/*using DChild.Gameplay.Characters.Players.Behaviour;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{

    public class GroundMovement : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        public enum Type
        {
            Crouch,
            Jog,
            Pull,
            Push
        }

        [SerializeField, HideLabel]
        private MovementStatsInfo m_configuration;

        private float m_currentSpeed;
        private IPlayerModifer m_modifier;
        private Rigidbody2D m_rigidbody;
        private Character m_character;
        private Animator m_animator;
        private int m_speedAnimationParameter;
        private int m_xInputAnimationParameter;
        private bool m_isTurning;

        [HideInEditorMode, ShowInInspector, ReadOnly]
        protected float speed => m_currentSpeed * m_modifier?.Get(PlayerModifier.MoveSpeed) ?? 1;

        public bool isTurning => m_isTurning;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_modifier = info.modifier;
            m_character = info.character;
            m_rigidbody = info.rigidbody;
            SwitchConfigTo(Type.Jog);
            m_animator = info.animator;
            m_speedAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX);
            m_xInputAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.XInput);
        }

        public void SetConfiguration(MovementStatsInfo info)
        {
            m_configuration.CopyInfo(info, true);
        }

        public void Cancel()
        {
            m_animator.SetFloat(m_speedAnimationParameter, 0);
            m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
        }

        public void SwitchConfigTo(Type type)
        {
            switch (type)
            {
                case Type.Crouch:
                    m_currentSpeed = m_configuration.crouchSpeed;
                    break;
                case Type.Jog:
                    m_currentSpeed = m_configuration.jogSpeed;
                    break;
                case Type.Pull:
                    m_currentSpeed = m_configuration.pullSpeed;
                    break;
                case Type.Push:
                    m_currentSpeed = m_configuration.pushSpeed;
                    break;
            }
        }

        public void UpdateFaceDirection(float direction)
        {
            m_isTurning = true;
            if (Mathf.Sign(direction) != (int)m_character.facing)
            {
                var otherFacing = m_character.facing == HorizontalDirection.Right ? HorizontalDirection.Left : HorizontalDirection.Right;
                m_character.SetFacing(otherFacing);
            }
            m_isTurning = false;
        }
        [SerializeField]
        private LayerMask groundLayer;
        public void Move(float direction, bool faceDirection)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);
            var slopeAngle = 0f;
            if (direction == 0)
            {
                m_animator.SetFloat(m_speedAnimationParameter, 0);
                m_animator.SetInteger(m_xInputAnimationParameter, 0);
            }
            else
            {
                if (faceDirection == true)
                {
                    UpdateFaceDirection(direction);
                }

                m_animator.SetFloat(m_speedAnimationParameter, 1);
                m_animator.SetInteger(m_xInputAnimationParameter, direction > 0 ? 1 : -1);
            }

            var xVelocity = speed * direction;
            if (hit.collider != null)
            {
                Vector2 surfaceNormal = hit.normal; 
                float dotProduct = Vector2.Dot(surfaceNormal, Vector2.right);
                slopeAngle = Vector2.Angle(surfaceNormal, Vector2.up);
                Debug.DrawRay(hit.point, surfaceNormal * 2, Color.red);
                Debug.Log("Slope angle: " + slopeAngle);
                *//*var xVelocity = speed * direction;
                m_rigidbody.velocity = new Vector2(xVelocity, slopeAngle);*//*
                if (dotProduct > 0)
                {
                    if (m_character.facing == HorizontalDirection.Left)
                    {
                        m_rigidbody.velocity = new Vector2(xVelocity, slopeAngle);
                        if (m_rigidbody.velocity.x == 0)
                        {
                            m_rigidbody.velocity = new Vector2(xVelocity, 0);
                        }
                    }
                    else
                    {
                        m_rigidbody.velocity = new Vector2(xVelocity, -slopeAngle);
                    }
                    Debug.Log("Slope on the left side");
                    Debug.Log(m_character.facing);
                }
                else if (dotProduct < 0)
                {
                    if(m_character.facing == HorizontalDirection.Left)
                    {
                        m_rigidbody.velocity = new Vector2(xVelocity, -slopeAngle);
                    }
                    else
                    {
                        m_rigidbody.velocity = new Vector2(xVelocity, slopeAngle);
                        if(m_rigidbody.velocity.x == 0)
                        {
                            m_rigidbody.velocity = new Vector2(xVelocity, 0);
                        }
                    }
                    Debug.Log("Slope on the right side");
                    Debug.Log(m_character.facing);
                }
                else
                {
                    m_rigidbody.velocity = new Vector2(xVelocity, m_rigidbody.velocity.y);
                }
            }
            *//*else
            {*//*
            //}
        }
    }
}*/