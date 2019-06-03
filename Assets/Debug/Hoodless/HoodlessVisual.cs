#if UNITY_EDITOR
using Holysoft.Event;
using UnityEngine;

namespace DChildDebug.Gameplay
{
    public class HoodlessVisual : MonoBehaviour
    {
        [SerializeField]
        private Color DefaultColor = Color.red;
        [SerializeField]
        private Color StealthColor = Color.blue;
        private IHoodless m_hoodless;
        private SpriteRenderer m_visual;

        private void Update()
        {
            m_visual.color = (m_hoodless.isHoodless) ? StealthColor : DefaultColor;
        }

        private void Start()
        {
            m_hoodless = GetComponent<IHoodless>();
            m_visual = GetComponentInChildren<SpriteRenderer>();
        }
    }
}

#endif