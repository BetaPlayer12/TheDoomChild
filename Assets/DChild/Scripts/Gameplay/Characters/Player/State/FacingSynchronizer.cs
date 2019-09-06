using Refactor.DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class FacingSynchronizer : MonoBehaviour, IComplexCharacterModule, IFacingComponent
    {
        [SerializeField]
        private Transform[] m_scaleFlips;

        private Animator m_animator;
        private string m_facingLeftParamater;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_animator = info.animator;
            m_facingLeftParamater = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsFacingLeft);
            info.character.CharacterTurn += OnCharacterTurn;
            var facing = info.character.facing;
            CallUpdate(facing);
        }

        private void AlignTransformScales(HorizontalDirection facing)
        {
            var currentScale = facing == HorizontalDirection.Left ? new Vector3(-1, 1, 1) : Vector3.one;
            for (int i = 0; i < m_scaleFlips.Length; i++)
            {
                m_scaleFlips[i].localScale = currentScale;
            }
        }

        public void CallUpdate(HorizontalDirection facing)
        {
            if (m_animator)
            {
                m_animator?.SetBool(m_facingLeftParamater, facing == HorizontalDirection.Left);
            }
            AlignTransformScales(facing);
        }

        private void OnCharacterTurn(object sender, FacingEventArgs eventArgs)
        {
            CallUpdate(eventArgs.currentFacingDirection);
        }
    }

}