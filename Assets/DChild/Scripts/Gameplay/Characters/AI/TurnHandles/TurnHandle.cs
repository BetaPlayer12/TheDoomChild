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
        [SerializeField]
        private bool m_maintainScaleValue = true;
        public event EventAction<FacingEventArgs> TurnDone;

        public void ForceTurnImmidiately()
        {
            TurnCharacter();
            m_character.transform.localScale = GetFacingScale(m_character.facing);
        }

        protected void TurnCharacter()
        {
            m_character.SetFacing(m_character.facing == HorizontalDirection.Left ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        protected Vector3 GetFacingScale(HorizontalDirection horizontalDirection)
        {
            if (m_maintainScaleValue == true)
            {
                var shouldBeNegative = m_character.facing == HorizontalDirection.Left;
                var currentScale = m_character.transform.localScale;
                if (shouldBeNegative && currentScale.x > 0)
                {
                    currentScale.x = currentScale.x * -1;
                }
                else if (currentScale.x < 0)
                {
                    currentScale.x = Mathf.Abs(currentScale.x);
                }
                return currentScale;
            }
            else
            {
                var currentScale = m_character.facing == HorizontalDirection.Left ? new Vector3(-1, 1, 1) : Vector3.one;
                return currentScale;
            }



        }

        protected void CallTurnDone(FacingEventArgs eventArgs) => TurnDone?.Invoke(this, eventArgs);

        private void OnValidate()
        {

            ComponentUtility.AssignNullComponent(this, ref m_character, ComponentUtility.ComponentSearchMethod.Parent);
        }


#if UNITY_EDITOR
        public void InitializeField(Character character)
        {
            m_character = character;
        }
#endif
    }
}