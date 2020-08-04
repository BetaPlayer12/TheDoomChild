using DChild.Gameplay.Characters.Players.Behaviour;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class Movement : MonoBehaviour, ICancellableBehaviour
    {
        public enum Type
        {
            Crouch,
            Jog,
        }

        [SerializeField]
        private float m_jogSpeed;
        [SerializeField]
        private float m_crouchSpeed;

        private float m_currentSpeed;
        private Rigidbody2D m_rigidbody;
        private Character m_character;


        public void Cancel()
        {

        }

        public void SwitchConfigTo(Type type)
        {
            switch (type)
            {
                case Type.Crouch:
                    m_currentSpeed = m_jogSpeed;
                    break;
                case Type.Jog:
                    m_currentSpeed = m_crouchSpeed;
                    break;
            }
        }

        public void Move(float direction)
        {
            if (direction == 0)
            {

            }
            else
            {
                if (Mathf.Sign(direction) != (int)m_character.facing)
                {
                    var otherFacing = m_character.facing == HorizontalDirection.Right ? HorizontalDirection.Left : HorizontalDirection.Right;
                }

            }
            var xVelocity = m_currentSpeed * direction;
            m_rigidbody.velocity = new Vector2(xVelocity, m_rigidbody.velocity.y);
        }
    }
}
