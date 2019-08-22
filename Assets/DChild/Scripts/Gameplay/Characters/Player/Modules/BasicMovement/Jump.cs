using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public abstract class Jump : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        [MinValue(0f)]
        protected float m_power = 1f;
        protected CharacterPhysics2D m_physics;
        protected Character m_character;

        public virtual void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_physics = info.physics;
        }

        public virtual void HandleJump()
        {
            m_physics.SetVelocity(y: 0);
        }
    }
}