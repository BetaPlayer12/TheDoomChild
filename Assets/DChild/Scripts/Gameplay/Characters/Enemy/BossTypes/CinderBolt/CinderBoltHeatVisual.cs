using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class CinderBoltHeatVisual : MonoBehaviour
    {
        [SerializeField]
        private CinderBoltHeatGauge m_heatGauge;
        [SerializeField]
        private SpineAnimation m_animation;
        [SerializeField]
        private CinderBoltHeatReaction[] m_reactions;
        private void OnHeatChange(object sender, EventActionArgs eventArgs)
        {
            for (int i = 0; i < m_reactions.Length; i++)
            {
                m_reactions[i].HandleReaction(m_heatGauge.currentValue);
            }
        }

        private void Awake()
        {
            m_heatGauge.HeatChanged += OnHeatChange;
            m_reactions = GetComponentsInChildren<CinderBoltHeatReaction>();
        }

    }
}