using DChild.Gameplay.Characters.Players.State;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using DChild.Gameplay.Characters.Players.Modules;
using Spine.Unity.Examples;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public abstract class Dash : MonoBehaviour, IComplexCharacterModule, IControllableModule
    {
        [SerializeField]
        protected SkeletonGhost m_ghosting;

        [SerializeField]
        [MinValue(0.1)]
        protected float m_power;
        [SerializeField]
        protected CountdownTimer m_duration;
        protected CharacterPhysics2D m_physics;
        protected Character m_character;
        private Animator m_animator;
        private string m_isDashingParameter;
        protected Vector2 m_direction;

        protected IDashState m_state;
        public virtual void Initialize(ComplexCharacterInfo info)
        {
            m_physics = info.physics;
            m_character = info.character;
            m_animator = info.animator;
            m_isDashingParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsDashing);
            m_state = info.state;
            info.skillResetRequester.SkillResetRequest += OnSkillReset;
        }

        private void OnSkillReset(object sender, ResetSkillRequestEventArgs eventArgs)
        {
            if (eventArgs.IsRequestedToReset(PrimarySkill.Dash))
            {
                m_state.canDash = true;
            }
        }

        public virtual void ConnectTo(IMainController controller)
        {
            controller.ControllerDisabled += OnControllerDisabled;
        }

        protected void TurnOnAnimation(bool value) => m_animator.SetBool(m_isDashingParameter, value);

        protected abstract void StopDash();

        protected abstract void OnDashDurationEnd(object sender, EventActionArgs eventArgs);

        protected virtual void OnControllerDisabled(object sender, EventActionArgs eventArgs)
        {
            StopDash();
        }
        protected virtual void Awake()
        {
            m_duration.CountdownEnd += OnDashDurationEnd;
        }

        protected virtual void Update()
        {
            m_duration.Tick(m_character.isolatedObject.deltaTime);
        }
    }
}