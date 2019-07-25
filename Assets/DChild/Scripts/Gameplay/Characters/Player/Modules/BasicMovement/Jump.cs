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
        protected float m_movingAnimationVelocityTreshold;

        protected IJumpModifier m_modifier;
        protected Character m_character;

        public event EventAction<EventActionArgs> JumpStart;
        public event EventAction<EventActionArgs> JumpEnd;

        public virtual void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_physics = info.physics;
        }

        protected void CallJumpStart() => JumpStart?.Invoke(this, EventActionArgs.Empty);
        protected void CallJumpEnd() => JumpEnd?.Invoke(this, EventActionArgs.Empty);

        public virtual void HandleJump()
        {
            m_physics.SetVelocity(y: 0);
        }

#if UNITY_EDITOR
        public void Initialize(float jumpPower)
        {
            m_power = jumpPower;
        }

       
#endif
    }
}