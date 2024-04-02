using System;
using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class CinderBoltHeatLightsReaction : CinderBoltHeatReaction
    {
        [System.Serializable]
        private class Configuration
        {
            [SerializeField]
            private int m_heatThreshold;
            [SerializeField, SpineSlot]
            private string m_slot;
            [SerializeField, SpineAttachment]
            private string m_attachment;

            public int heatThreshold => m_heatThreshold;
            public string slot => m_slot;
            public string attachment => m_attachment;
        }

        [SerializeField]
        private Configuration[] m_configurations;

        public override void HandleReaction(SpineAnimation spineAnimation, int heatValue)
        {
            for (int i = 0; i < m_configurations.Length; i++)
            {
                var config = m_configurations[i];
                if (heatValue >= config.heatThreshold)
                {
                    spineAnimation.skeletonAnimation.skeleton.SetAttachment(config.slot, config.attachment);
                }
                else
                {
                    spineAnimation.skeletonAnimation.skeleton.SetAttachment(config.slot, null);
                }
            }
        }
    }
}