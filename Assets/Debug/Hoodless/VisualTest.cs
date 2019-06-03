#if UNITY_EDITOR
using Holysoft.Event;
using UnityEngine;

namespace DChildDebug.Gameplay
{
    public class VisualTest : MonoBehaviour
    {
        [SerializeField]
        private Color DefaultColor = Color.red;
        [SerializeField]
        private Color HoodlessColor = Color.blue;
        [SerializeField]
        private Color StealthColor = Color.green;
        private IHoodless m_hoodless;
        private IStealth m_stealth;
        private SpriteRenderer m_visual;

        private void Update()
        {
            if (m_hoodless.isHoodless)
            {
                m_visual.color = HoodlessColor;
            }
            else if (m_stealth.isStealth)
            {
                m_visual.color = StealthColor;
            }
            else
            {
                m_visual.color = DefaultColor;
            }
        }

        private void Start()
        {
            m_hoodless = GetComponent<IHoodless>();
            m_stealth = GetComponent<IStealth>();
            m_visual = GetComponentInChildren<SpriteRenderer>();
        }
    }
}
#endif