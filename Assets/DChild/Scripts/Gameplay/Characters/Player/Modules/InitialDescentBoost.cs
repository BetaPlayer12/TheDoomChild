using DChild.Gameplay.Characters.Players.Behaviour;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class InitialDescentBoost : MonoBehaviour, IComplexCharacterModule, IResettableBehaviour
    {
        [SerializeField, MinValue(0f)]
        private float m_boost;

        private Rigidbody2D m_rigidbody;
        private bool m_prevYVelocityIsPositive;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_rigidbody = info.rigidbody;
            m_prevYVelocityIsPositive = true;
        }

        public void Handle()
        {
            if (m_rigidbody.velocity.y <= 0)
            {
                if (m_prevYVelocityIsPositive)
                {
                    var velocity = m_rigidbody.velocity;
                    velocity.y = -m_boost;
                    m_rigidbody.velocity = velocity;
                }
                m_prevYVelocityIsPositive = false;
            }
            else
            {
                m_prevYVelocityIsPositive = true;
            }
        }

        public void Reset()
        {
            m_prevYVelocityIsPositive = true;
        }
    }
}
