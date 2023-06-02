using DChild.Gameplay.Leveling;
using Holysoft.Event;
using UnityEngine;

namespace DChildDebug.Window
{
    public class PlayerLevelAdjuster : MonoBehaviour, ITrackableValue
    {
        [SerializeField]
        private PlayerLevel m_level;

        public float value => m_level.currentLevel;

        public event EventAction<EventActionArgs> ValueChange;

        public void ForceLevelUp()
        {
            m_level.exp.AddCurrentValue(m_level.exp.maxValue);
        }
    }
}
