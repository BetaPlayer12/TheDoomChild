using TMPro;
using UnityEngine;

namespace DChild.Gameplay.Characters.Player.CombatArt.Leveling
{
    public class CombatArtLevelUI : MonoBehaviour
    {
        [SerializeField]
        private CombatArtLevel m_reference;
        [SerializeField]
        private TextMeshProUGUI m_levelLabel;

        public void SyncWithReference()
        {
            m_levelLabel.text = m_reference.currentLevel.ToString();
        }
    }
}