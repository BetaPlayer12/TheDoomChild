using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Modules
{
    public class AcceleratedGroundMovement : MonoBehaviour, IMoveHandle
    {
#if UNITY_EDITOR
        [SerializeField]
#endif
        private CharacterPhysics2D m_physics;

        [SerializeField, MinValue(0)]
        private float m_topSpeed;

        public void Move(HorizontalDirection direction)
        {
            var directionVector = new Vector2((int)direction, 0);
            m_physics.SetVelocity(m_topSpeed * directionVector);
        }

        public void Stop()
        {
            if (m_physics.inContactWithGround)
            {
                m_physics.SetVelocity(Vector2.zero);
            }
            else
            {
                m_physics.SetVelocity(0);
            }
        }
    }
}