using Refactor.DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class AnimationFacingSynchronizer : MonoBehaviour, IComplexCharacterModule
    {
        private Animator m_animator;
        private string m_facingLeftParamater;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_animator = info.animator;
            m_facingLeftParamater = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsFacingLeft);
            info.character.CharacterTurn += OnCharacterTurn;
            m_animator.SetBool(m_facingLeftParamater, info.character.facing == HorizontalDirection.Left);
        }

        private void OnCharacterTurn(object sender, FacingEventArgs eventArgs)
        {
            m_animator.SetBool(m_facingLeftParamater, eventArgs.currentFacingDirection == HorizontalDirection.Left);
        }
    }

}