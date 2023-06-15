using DChild.Gameplay.Characters.Player.CombatArt.Leveling;
using Holysoft.Event;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class CombatSkillPointsUI : MonoBehaviour
    {
        [SerializeField]
        private CombatSkillPoints m_reference;
        [SerializeField]
        private TextMeshProUGUI m_pointsLabel;

        public void SyncWithReference()
        {
            m_pointsLabel.text = m_reference.points.ToString();
        }
        private void OnPointsChange(object sender, EventActionArgs eventArgs)
        {
            SyncWithReference();
        }

        private void Awake()
        {
            m_reference.OnPointsChange += OnPointsChange;
            SyncWithReference();
        }
    }
}