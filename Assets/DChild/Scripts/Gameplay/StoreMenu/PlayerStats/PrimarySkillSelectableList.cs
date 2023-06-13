using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.UI.PrimarySkills
{
    public class PrimarySkillSelectableList : MonoBehaviour
    {
        [SerializeField]
        private PrimarySkillList m_data;

        private PrimarySkillSelectable[] m_selectables;

        public void UpdateListAvailability()
        {
            var skills = GameplaySystem.playerManager.player.skills;
            for (int i = 0; i < m_selectables.Length; i++)
            {
                var selectable = m_selectables[i];
                var isUnlocked = skills.IsSkillUnlocked(selectable.reference.skill);
                selectable.SetAsUnlocked(isUnlocked);
            }
        }

        public void InitializeList()
        {
            m_selectables = GetComponentsInChildren<PrimarySkillSelectable>();
            for (int i = 0; i < m_selectables.Length; i++)
            {
                var selectable = m_selectables[i];
                selectable.SetSelectableFor(m_data.GetData(i));
            }
        }
    }
}