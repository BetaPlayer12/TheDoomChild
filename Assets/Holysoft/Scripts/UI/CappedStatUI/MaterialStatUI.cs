using UnityEngine;

namespace Holysoft.Gameplay.UI
{
    public class MaterialStatUI : CappedStatUI
    {
        [SerializeField]
        private Material m_material;
        [SerializeField]
        private string m_fillString;

        private float m_maxValue;

        protected override float maxValue { set => m_maxValue = value; }
        protected override float currentValue
        {
            set
            {
                var uiValue = value / m_maxValue;
                m_material.SetFloat(m_fillString, float.IsNaN(uiValue) ? 0 : uiValue);
            }
        }
    }
}