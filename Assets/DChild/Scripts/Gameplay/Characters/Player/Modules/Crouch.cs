using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class Crouch : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        private Animator m_animator;
        private int m_animationParameter;

        private ICrouchState m_state;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsCrouched);
            m_state = info.state;
        }

        public void Cancel()
        {
            m_animator.SetBool(m_animationParameter, false);
            m_state.isCrouched = false;
        }

        public bool IsThereNoCeiling()
        {
            return true;
        }

        public void Execute()
        {
            m_animator.SetBool(m_animationParameter, true);
            m_state.isCrouched = true;
        }
    }
}
