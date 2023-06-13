using TMPro;
using UnityEngine;

namespace DChild.Gameplay.Leveling
{
    public class PlayerLevelUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerLevel m_reference;
        [SerializeField]
        private TextMeshProUGUI m_levelLabel;

        public void SyncWithReference()
        {
            m_levelLabel.text = m_reference.currentLevel.ToString();
        }
    }
}