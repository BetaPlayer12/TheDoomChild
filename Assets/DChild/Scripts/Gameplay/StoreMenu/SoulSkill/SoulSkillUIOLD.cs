using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.UI;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Menu.SoulSkills
{
    public class SoulSkillUIOLD : MonoBehaviour
    {
        [ShowInInspector]
        private SoulSkill m_data;
        [SerializeField]
        private Transform m_parent;
        private Vector2 m_originalPosition;

        private DraggableUI m_draggableUI;

        public SoulSkill data => m_data;

        public event EventAction<EventActionArgs> ReplacementAttempt;

        public void ReturnToOriginalPosition()
        {
            transform.SetParent(m_parent);
            transform.position = m_originalPosition;
        }

        public void UsePositionAsSnapOrigin() => m_draggableUI.SetOrigin(transform.localScale);

        public void SendReplacementAttemptEvent()
        {
            ReplacementAttempt?.Invoke(this, EventActionArgs.Empty);
        }

        private void Awake()
        {
            m_originalPosition = transform.position;
        }
    }
}