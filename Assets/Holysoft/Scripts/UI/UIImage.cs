using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.UI
{
    [RequireComponent(typeof(Image))]
    public sealed class UIImage : UIElement
    {
        [SerializeField]
        [HideInInspector]
        private Image m_image;

        public override void Hide()
        {
            m_image.enabled = false;
        }

        public override void Show()
        {
            m_image.enabled = true;
        }

        private void OnValidate()
        {
            m_image = GetComponent<Image>();
        }
    }
}