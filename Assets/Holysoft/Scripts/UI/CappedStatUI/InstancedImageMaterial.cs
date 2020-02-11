using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.Gameplay.UI
{
    [System.Serializable]
    public class InstancedImageMaterial
    {
        [SerializeField]
        private Image m_image;
        [ShowInInspector, InlineEditor, HideInEditorMode]
        private Material m_material;

        public Material material => m_material;

        public void Initialize()
        {
            m_material = Object.Instantiate(m_image.material);
            m_image.material = m_material;
        }
    }
}