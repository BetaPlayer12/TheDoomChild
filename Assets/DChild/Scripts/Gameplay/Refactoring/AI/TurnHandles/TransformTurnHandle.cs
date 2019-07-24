using DChild.Gameplay.Characters;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters
{
    public class TransformTurnHandle : SimpleTurnHandle, IFacingComponent
    {
        public override void Execute()
        {
            TurnCharacter();
            CallUpdate(m_character.facing);
            CallTurnDone(new FacingEventArgs(m_character.facing));
        }

        public void CallUpdate(HorizontalDirection facing)
        {
            var currentScale = facing == HorizontalDirection.Left ? new Vector3(-1, 1, 1) : Vector3.one;
            m_character.transform.localScale = currentScale;
        }
    }
}