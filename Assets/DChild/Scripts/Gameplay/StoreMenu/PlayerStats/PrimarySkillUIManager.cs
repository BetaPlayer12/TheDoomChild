using TMPro;
using UnityEngine;

namespace DChild.Gameplay.UI.PrimarySkills
{
    public class PrimarySkillUIManager : MonoBehaviour
    {
        [SerializeField]
        private PrimarySkillSelectableList m_skillList;
        [SerializeField]
        private TextMeshProUGUI m_descriptionLabel;
        [SerializeField]
        private TextMeshProUGUI m_controlsLabel;

        public void UpdateSelectables()
        {
            m_skillList.UpdateListAvailability();
        }

        public void Select(PrimarySkillSelectable selectable)
        {
            m_descriptionLabel.text = selectable.reference.description;
            m_controlsLabel.text = selectable.reference.instruction;
        }

        private void Start()
        {
            m_skillList.InitializeList();
        }

    }
}