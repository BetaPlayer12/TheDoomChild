using DChild.Gameplay.Characters.Player.CombatArt.Leveling;
using Holysoft.Event;
using UnityEngine;

namespace DChildDebug.Window
{
    public class CombatArtLevelAdjuster : MonoBehaviour, ITrackableValue
    {
        [SerializeField]
        private CombatArtLevel m_level;

        public float value => m_level.currentLevel;

        public event EventAction<EventActionArgs> ValueChange;

        public void ForceLevelUp()
        {
            m_level.exp.AddCurrentValue(m_level.exp.maxValue);
        }
    }
}
