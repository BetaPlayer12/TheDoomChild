using UnityEngine;

namespace DChild.Gameplay.UI.CombatArts
{
    [System.Serializable]
    public class CombatArtUnlockProgressor
    {
        [SerializeField]
        private UnityEngine.UI.Image[] m_fillableImages;

        public void DisplayProgress(float progress)
        {
            for (int i = 0; i < m_fillableImages.Length; i++)
            {
                m_fillableImages[i].fillAmount = progress;
            }
        }
    }

}