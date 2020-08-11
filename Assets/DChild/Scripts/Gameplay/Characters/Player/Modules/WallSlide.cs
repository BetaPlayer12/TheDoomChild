using DChild.Gameplay.Characters.Players.Behaviour;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class WallSlide : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        [SerializeField, MinValue(0f)]
        private float m_speed;
        [SerializeField]
        private RaySensor m_bodySensor;

        private Rigidbody2D m_rigibody;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_rigibody = info.rigidbody;
        }

        public bool IsThereAWall()
        {
            m_bodySensor.Cast();
            return m_bodySensor.allRaysDetecting;
        }

        public void Cancel()
        {
            m_rigibody.velocity = Vector2.zero;
        }

        public void Execute()
        {
            m_rigibody.velocity = new Vector2(m_rigibody.velocity.x, m_speed * -1);
        }
    }
}
