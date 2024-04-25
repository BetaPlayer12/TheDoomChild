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
            [SerializeField]
            private SkeletonRendererCustomMaterials m_renderCustomMaterials;

            public int heatThreshold => m_heatThreshold;
            public SkeletonRendererCustomMaterials skeletonRendererCustomMaterials => m_renderCustomMaterials;
        }

        [SerializeField]
        private Configuration[] m_configurations;

        public override void HandleReaction(int heatValue)
        {
            for (int i = 0; i < m_configurations.Length; i++)
            {
                var config = m_configurations[i];
                if (heatValue >= config.heatThreshold)
                {
                    config.skeletonRendererCustomMaterials.enabled = true;
                }
                else
                {
                    config.skeletonRendererCustomMaterials.enabled = false;
                }
            }
        }

        private void Start()
        {
            for (int i = 0; i < m_configurations.Length; i++)
            {
                m_configurations[i].skeletonRendererCustomMaterials.enabled = false;
            }
        }
    }
}