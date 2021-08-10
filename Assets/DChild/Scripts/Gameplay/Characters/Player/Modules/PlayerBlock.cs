using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerBlock : MonoBehaviour, ICancellableBehaviour, IResettableBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private float m_blockDuration;

        private IBlockingState m_state;
        private Animator m_animator;
        private int m_animationParameter;
        private bool m_canParry;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_state = info.state;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsBlocking);
        }

        public void Execute()
        {
            Debug.Log("Blocking!");
            m_animator.SetBool(m_animationParameter, true);
            m_state.isBlocking = true;
        }

        public void Cancel()
        {
            m_animator.SetBool(m_animationParameter, false);
            m_state.isBlocking = false;
        }

        public void Reset()
        {
            
        }
    }
}