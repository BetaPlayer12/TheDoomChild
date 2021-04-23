using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class FacingSynchronizer : MonoBehaviour, IComplexCharacterModule, IFacingComponent
    {
        [SerializeField]
        private bool m_maintainScaleValue;
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
            if (m_maintainScaleValue)
            {
                var shouldBeNegative = facing == HorizontalDirection.Left;
                foreach (var scaleFlip in m_scaleFlips)
                {
                    var scale = scaleFlip.localScale;
                    if (shouldBeNegative && scale.x > 0)
                    {
                        scale.x = scale.x * -1;
                    }
                    else if (scale.x < 0)
                    {
                        scale.x = Mathf.Abs(scale.x);
                    }
                    scaleFlip.localScale = scale;
                }
            }
            else
            {
                var currentScale = facing == HorizontalDirection.Left ? new Vector3(-1, 1, 1) : Vector3.one;
                for (int i = 0; i < m_scaleFlips.Length; i++)
                {
                    m_scaleFlips[i].localScale = currentScale;
                }
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

        private void Start()
        {

        }
    }

}