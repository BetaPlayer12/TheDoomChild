using DChild.Gameplay.Characters;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters
{
    public class TransformTurnHandle : SimpleTurnHandle
    {
        public override void Execute()
        {
            TurnCharacter();
            Debug.Log("turn trigger");
            var currentScale = m_character.facing == HorizontalDirection.Left ? new Vector3(-1, 1, 1) : Vector3.one;
            m_character.transform.localScale = currentScale;
            CallTurnDone(new FacingEventArgs(m_character.facing));
        }
    }
}