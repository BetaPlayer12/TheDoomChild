using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Narrative
{
    public class TutorialInfo : MonoBehaviour
    {
        [SerializeField,TextArea]
        private string m_instructions;

        public string instructions => m_instructions;

        public event EventAction<EventActionArgs> Selected;

        public void Select()
        {
            Selected?.Invoke(this, EventActionArgs.Empty);
        }
    }
}
