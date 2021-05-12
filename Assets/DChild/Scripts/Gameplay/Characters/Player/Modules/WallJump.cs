using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{

    public class WallJump : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private Vector2 m_power;
        [SerializeField, MinValue(0f)]
        private float m_disableInputDuration;

        private Character m_character;
        private Rigidbody2D m_rigidbody;
        private IWallJumpState m_state;
        private Animator m_animator;
        private int m_animationParameter;

        public event EventAction<EventActionArgs> ExecuteModule;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_rigidbody = info.rigidbody;
            m_state = info.state;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Jump);
        }

        public void JumpAway()
        {
            var velocity = m_power;
            velocity.x *= (float)m_character.facing;
            m_rigidbody.velocity = velocity;
            ExecuteModule?.Invoke(this, EventActionArgs.Empty);

            StartCoroutine(DisableInputRoutine());
        }

        private IEnumerator DisableInputRoutine()
        {
            m_state.waitForBehaviour = true;
            yield return new WaitForSeconds(m_disableInputDuration);
            m_state.waitForBehaviour = false;
        }
    }
}
