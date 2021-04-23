using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Characters.NPC
{
    public class NPCProfileUI : MonoBehaviour
    {
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_name;
        [SerializeField]
        private TextMeshProUGUI m_title;

        public void Set(NPCProfile profile)
        {
            m_icon.sprite = profile.baseIcon;
            m_name.text = profile.characterName;
            if (m_title)
            {
                m_title.text = profile.title;
            }
        }
    }
}