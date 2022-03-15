using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    public class GemCrawlerMatConfigurator : MonoBehaviour
    {
        [SerializeField]
        private Vector2 m_noiseOffset;
        [SerializeField]
        private float m_noiseScale;
        [SerializeField, ColorUsage(true, true)]
        private Color m_glowColor;
        [SerializeField, ColorUsage(true, true)]
        private Color m_glowOverlay;
        [SerializeField, ColorUsage(true, true)]
        private Color m_OverallTexColor;


        private const string REFERENCE_NOISEOFFSET = "Vector2_GradientNoiseOffset";
        private const string REFERENCE_NOISESCALE = "Vector1_GradientScale";
        private const string REFERENCE_GLOWCOLOR = "Color_GlowColor";
        private const string REFERENCE_GLOWOVERLAY = "Color_GlowOverlay";
        private const string REFERENCE_OVERALLTEXCOLOR = "Color_OverallTexColor";

        private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

        [Button]
        private void SetProperties()
        {
            var mesh = GetComponent<MeshRenderer>();
            mesh.GetPropertyBlock(propertyBlock);

            propertyBlock.SetVector(REFERENCE_NOISEOFFSET, m_noiseOffset);
            propertyBlock.SetFloat(REFERENCE_NOISESCALE, m_noiseScale);
            propertyBlock.SetColor(REFERENCE_GLOWCOLOR, m_glowColor);
            propertyBlock.SetColor(REFERENCE_GLOWOVERLAY, m_glowOverlay);
            propertyBlock.SetColor(REFERENCE_OVERALLTEXCOLOR, m_OverallTexColor);

            mesh.SetPropertyBlock(propertyBlock);
        }

        private void Start()
        {
            SetProperties();
        }

    }

}