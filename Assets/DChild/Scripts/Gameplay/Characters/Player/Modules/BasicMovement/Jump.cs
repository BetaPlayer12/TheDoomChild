using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public abstract class Jump : MonoBehaviour
    {
        [SerializeField]
        [MinValue(0f)]
        protected float m_power = 1f;
        protected CharacterPhysics2D m_character;
        protected float m_movingAnimationVelocityTreshold;

        protected IJumpModifier m_modifier;
        protected IFacingConfigurator m_facing;

        public event EventAction<EventActionArgs> JumpStart;
        public event EventAction<EventActionArgs> JumpEnd;

        public virtual void Initialize(IPlayerModules player)
        {
            m_facing = player;
            m_character = player.physics;
            m_modifier = player.modifiers;
        }

        protected void CallJumpStart() => JumpStart?.Invoke(this, EventActionArgs.Empty);
        protected void CallJumpEnd() => JumpEnd?.Invoke(this, EventActionArgs.Empty);

        public virtual void HandleJump()
        {
            m_character.SetVelocity(y: 0);
        }

#if UNITY_EDITOR
        public void Initialize(float jumpPower)
        {
            m_power = jumpPower;
        }
#endif
    }
}