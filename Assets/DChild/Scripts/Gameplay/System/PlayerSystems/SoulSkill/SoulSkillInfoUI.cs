using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.EquipmentSystem;
using DChild.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.SoulSkills.UI
{
    public class SoulSkillInfoUI : MonoBehaviour, ISoulSkillLocalizer
    {
        [SerializeField] private CanvasGroup m_parentCanvas;
        //[SerializeField] private Image m_soulIcon;
        [SerializeField] private TextMeshProUGUI m_name;
        [SerializeField] private TextMeshProUGUI m_capcity;
        [SerializeField] private TextMeshProUGUI m_description;
        [SerializeField] private TextMeshProUGUI m_originEquipmentName;

        private TextMeshProUGUI[] m_detailsText;

        public event System.Action<TextMeshProUGUI, TextMeshProUGUI, SoulSkill> soulSkillLocalize;

        private void Awake()
        {
            ClearInfo();
        }

        public void ClearInfo()
        {
            SetTextVisible(false);
        }

        private void SetTextVisible(bool visible)
        {
            m_detailsText ??= m_parentCanvas.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var text in m_detailsText)
            {
                text.enabled = visible;
            }
        }

        public void DisplayInfo(SoulSkill soulSkill, SoulEquipmentItem originEquipment)
        {
            if (soulSkill == null)
            {
                ClearInfo();
                return;
            }

            SetTextVisible(true);
            m_capcity.text = soulSkill.capacity.ToString();

            //m_soulIcon.sprite = soulSkill.icon;

            m_originEquipmentName.text =
    originEquipment != null
        ? $"Origin: {originEquipment.itemName}"
        : "";


            if (soulSkillLocalize == null)
            {
                m_name.text = soulSkill.name;
                m_description.text = soulSkill.description;
                return;
            }
            soulSkillLocalize?.Invoke(m_name, m_description, soulSkill);
        }
    }
}
