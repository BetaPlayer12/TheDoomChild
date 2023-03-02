using DChild.Gameplay.Characters.Players.Behaviour;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class WallSlide : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        [SerializeField, HideLabel]
        private WallSlideStatsInfo m_configuration;
        [SerializeField]
        private RaySensor m_bodySensor;

        private Rigidbody2D m_rigibody;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_rigibody = info.rigidbody;
        }

        public void SetConfiguration(WallSlideStatsInfo info)
        {
            m_configuration.CopyInfo(info);
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
            m_rigibody.velocity = new Vector2(m_rigibody.velocity.x, m_configuration.speed * -1);
        }
    }
}
