using DChild.Gameplay.Characters.Players;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.UI.CombatArts
{
    public class CombatArtUIDetail : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_artNameLabel;

        public void Display(CombatArtData data, int level)
        {
            if (data != null)
            {
                m_artNameLabel.text = data.combatArtName;
                if (level > 1)
                {
                    m_artNameLabel.text += $" {level}";
                }
            }
        }
    }

}