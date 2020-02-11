using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.Gameplay.UI
{

    public class ImageMaterialStatUI : CappedStatUI
    {
        [SerializeField]
        private Image m_image;
        [SerializeField]
        private string m_fillString;
        [ShowInInspector, InlineEditor, HideInEditorMode]
        private Material m_material;
        private float m_maxValue;

        public override float maxValue { set => m_maxValue = value; }
        public override float currentValue
        {
            set
            {
                var uiValue = value / m_maxValue;
                m_material.SetFloat(m_fillString, float.IsNaN(uiValue) ? 0 : uiValue);
            }
        }

        protected override void Awake()
        {
            m_material = Instantiate(m_image.material);
            m_image.material = m_material;
            base.Awake();
        }
    }
}