using TMPro;
using UnityEngine;

namespace DChild.Gameplay.Narrative
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_text;

        public void SetMessage(string message)
        {
            m_text.text = message;
        }
    }
}
