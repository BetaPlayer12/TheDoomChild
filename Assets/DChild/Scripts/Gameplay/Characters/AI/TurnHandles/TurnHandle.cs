using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Holysoft;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public abstract class TurnHandle : MonoBehaviour
    {
        [SerializeField]
        protected Character m_character;

        public event EventAction<FacingEventArgs> TurnDone;

        protected void TurnCharacter() => m_character.SetFacing(m_character.facing == HorizontalDirection.Left ? HorizontalDirection.Right : HorizontalDirection.Left);
        protected void CallTurnDone(FacingEventArgs eventArgs) => TurnDone?.Invoke(this, eventArgs);

        private void OnValidate()
        {
            ComponentUtility.AssignNullComponent(this,ref m_character, ComponentUtility.ComponentSearchMethod.Parent);
        }
    }
}