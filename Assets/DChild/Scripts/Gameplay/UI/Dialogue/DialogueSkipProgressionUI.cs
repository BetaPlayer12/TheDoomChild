using UnityEngine;
using UnityEngine.UI;

namespace DChild.UI
{
    public class DialogueSkipProgressionUI : MonoBehaviour
    {
        [SerializeField]
        private Image m_progressionUI;

        public void SetProgression(float progress)
        {
            m_progressionUI.fillAmount = progress;
        }
    }
}