using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.UI
{
    public class PlayerHUDReactionFX : MonoBehaviour
    {
        [SerializeField]
        private Image m_shadowFX;
        [SerializeField]
        private Image m_fireFX;

        public void ShowShadowFX(bool show)
        {
            m_shadowFX.enabled = show;
        }

        public void ShowFireFX(bool show)
        {
            m_fireFX.enabled = show;
        }


        public void HideAll()
        {
            ShowShadowFX(false);
            ShowFireFX(false);
        }
    }
}