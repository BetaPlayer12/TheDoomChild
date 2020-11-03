using System;
using Holysoft.Gameplay.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.UI
{

    public class PlayerHealthUI : CappedStatUI
    {
        [System.Serializable]
        private class MaterialInfo
        {
            [SerializeField]
            private Material m_material;
            [SerializeField]
            private string m_colorField;

            public void ChangeColor(Color color)
            {
                m_material.SetColor(m_colorField, color);
            }
        }
        [SerializeField, ColorUsage(true, true), BoxGroup("Color Changer")]
        private Color m_fromColor;
        [SerializeField, ColorUsage(true, true), BoxGroup("Color Changer")]
        private Color m_toColor;
        [SerializeField, Range(0f, 1f), BoxGroup("Color Changer")]
        private float m_colorChangeThreshold;
        [SerializeField, BoxGroup("Color Changer")]
        private MaterialInfo[] m_materials;
        private bool m_colorChanged;

        [SerializeField]
        private Material m_material;
        [SerializeField]
        private string m_fillString;

        private float m_maxValue;

        public override float maxValue { set => m_maxValue = value; }
        public override float currentValue
        {
            set
            {
                var uiValue = value / m_maxValue;
                var percentValue = float.IsNaN(uiValue) ? 0 : uiValue;
                m_material.SetFloat(m_fillString, percentValue);
                if (percentValue <= m_colorChangeThreshold)
                {
                    if (m_colorChanged == false)
                    {
                        for (int i = 0; i < m_materials.Length; i++)
                        {
                            m_materials[i].ChangeColor(m_toColor);
                        }
                        m_colorChanged = true;
                    }
                }
                else
                {
                    if (m_colorChanged)
                    {
                        for (int i = 0; i < m_materials.Length; i++)
                        {
                            m_materials[i].ChangeColor(m_fromColor);
                        }
                        m_colorChanged = false;
                    }
                }
            }
        }

        protected override void Initialize(float maxValue, float currentValue)
        {
            base.Initialize(maxValue, currentValue);
            var uiValue = currentValue / maxValue;
            var percentValue = float.IsNaN(uiValue) ? 0 : uiValue;
            if (percentValue <= m_colorChangeThreshold)
            {
                for (int i = 0; i < m_materials.Length; i++)
                {
                    m_materials[i].ChangeColor(m_toColor);
                }
                m_colorChanged = true;

            }
            else
            {
                for (int i = 0; i < m_materials.Length; i++)
                {
                    m_materials[i].ChangeColor(m_fromColor);
                }
                m_colorChanged = false;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < m_materials.Length; i++)
            {
                m_materials[i].ChangeColor(m_fromColor);
            }
            m_colorChanged = false;
        }
    }
}