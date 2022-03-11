using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    public class GemCrawlerMatConfigurator : MonoBehaviour
    {
        [SerializeField]
        private float m_noise;
        [SerializeField, ColorUsage(true, true)]
        private Color m_color1;
        [SerializeField, ColorUsage(true, true)]
        private Color m_color2;
        [SerializeField, ColorUsage(true, true)]
        private Color m_color3;


        private const string REFERENCE_NOISE = "";
        private const string REFERENCE_COLOR1 = "";
        private const string REFERENCE_COLOR2 = "";
        private const string REFERENCE_COLOR3 = "";

        private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

        [Button]
        private void SetProperties()
        {
            var mesh = GetComponent<MeshRenderer>();
            mesh.GetPropertyBlock(propertyBlock);

            propertyBlock.SetFloat(REFERENCE_NOISE, m_noise);
            propertyBlock.SetColor(REFERENCE_COLOR1, m_color1);
            propertyBlock.SetColor(REFERENCE_COLOR2, m_color2);
            propertyBlock.SetColor(REFERENCE_COLOR3, m_color3);

            mesh.SetPropertyBlock(propertyBlock);
        }

        private void Start()
        {
            SetProperties();
        }

    }

}