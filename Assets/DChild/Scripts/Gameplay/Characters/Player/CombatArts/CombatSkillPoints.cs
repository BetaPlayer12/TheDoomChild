
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Player.CombatArt.Leveling
{
    public class CombatSkillPoints : MonoBehaviour
    {
        [SerializeField]
        private CombatArtLevel m_level;
        [SerializeField]
        private int m_points;

        public int points => m_points;

        public event EventAction<EventActionArgs> OnPointsChange;

        public void AddPoint(int value)
        {
            SetPoints(m_points + value);
        }

        public void SetPoints(int value)
        {
            m_points = value;
            if (m_points < 0)
            {
                m_points = 0;
            }
            OnPointsChange?.Invoke(this, EventActionArgs.Empty);
        }

        private void OnLevelUp(object sender, EventActionArgs eventArgs)
        {
            AddPoint(1);
        }

        private void Awake()
        {
            m_level.OnLevelUp += OnLevelUp;
        }
    }
}