using UnityEngine;

namespace DChild.Gameplay.UI.PrimarySkills
{
    public class PrimarySkillUIManager : MonoBehaviour
    {
        [SerializeField]
        private PrimarySkillSelectableList m_skillList;

        public void UpdateSelectables()
        {
            m_skillList.UpdateListAvailability();
        }

        public void Select(PrimarySkillSelectable selectable)
        {

        }

        private void Start()
        {
            m_skillList.InitializeList();
        }
    }
}