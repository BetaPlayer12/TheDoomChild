using DChild.Gameplay.Characters.Enemies;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.Combat.UI
{
    public class BossNameUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_bossName;
        [SerializeField]
        private TextMeshProUGUI m_bossTitle;
        [SerializeField]
        private TextMeshProUGUI m_bossNameOnly;

        public void SetName(Boss boss)
        {
            var hasTitle = boss.creatureTitle != string.Empty || boss.creatureTitle != "";
            if (hasTitle)
            {
                m_bossName.text = boss.creatureName;
                m_bossTitle.text = boss.creatureTitle;
            }
            else
            {
                m_bossNameOnly.text = boss.creatureName;
            }
            m_bossName.enabled = hasTitle;
            m_bossTitle.enabled = hasTitle;
            m_bossNameOnly.enabled = !hasTitle;
        }
    }
}