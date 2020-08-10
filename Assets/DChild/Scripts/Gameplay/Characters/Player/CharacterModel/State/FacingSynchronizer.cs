using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class FacingSynchronizer : MonoBehaviour, IComplexCharacterModule, IFacingComponent
    {
        [SerializeField]
        private Transform[] m_scaleFlips;

        public void Initialize(ComplexCharacterInfo info)
        {
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
            AlignTransformScales(facing);
        }

        private void OnCharacterTurn(object sender, FacingEventArgs eventArgs)
        {
            CallUpdate(eventArgs.currentFacingDirection);
        }
    }

}