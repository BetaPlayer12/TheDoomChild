using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.EquipmentSystem;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DChild.Gameplay.EquipmentSystem.PlayerSoulEquipmentHandle;

namespace DChild.Menu.Equipment.UI
{
    public class EquipmentDetailsUI : MonoBehaviour
    {
        [BoxGroup("MAIN UI"), SerializeField] private EquipmentUI m_equipmentUI;

        [BoxGroup("INFO"), SerializeField] private Image m_equipmentIcon;
        [BoxGroup("INFO"), SerializeField] private TextMeshProUGUI m_itemNameLabel;

        [BoxGroup("STATS"), SerializeField] private GameObject m_parentTransform;
        [BoxGroup("STATS"), SerializeField] private GameObject m_statRow;

        [BoxGroup("SKILL BONUS"), SerializeField] private TextMeshProUGUI m_bonusLabel;

        private List<GameObject> m_instantiatedRows = new();
        private SoulEquipmentItem m_highlightedEquipment;

        [BoxGroup("TEST DATA"), HideInPlayMode, SerializeField] private SoulEquipmentItem m_sampleItem;

        public void ConnectGridItem(EquipmentGridItemUI gridItem) => gridItem.OnGridItemSelected += OnGridItemSelected;
        public void DisconnectGridItem(EquipmentGridItemUI gridItem) => gridItem.OnGridItemSelected -= OnGridItemSelected;
        public void OnGridItemSelected(object sender, EventActionArgs eventArgs) => UpdateUI();
        public void SetHighlightedEquipment(SoulEquipmentItem value) => m_highlightedEquipment = value;

        [Button]
        public void UpdateUI()
        {
            if (m_instantiatedRows.Count > 0)
                Reset();

            if (m_highlightedEquipment == null)
            {
                Clear();
                return;
            }

            m_equipmentIcon.gameObject.SetActive(true);
            m_equipmentIcon.sprite = m_highlightedEquipment.icon;
            m_itemNameLabel.text = m_highlightedEquipment.itemName;

            var boostList = m_highlightedEquipment.soulEquipment.statBoostList;
            if (boostList != null)
                ShowStatBuffs(boostList);
        }

        public void Clear()
        {
            Reset();
            m_highlightedEquipment = null;
            m_equipmentIcon.sprite = null;
            m_equipmentIcon.gameObject.SetActive(false);
            m_itemNameLabel.text = string.Empty;
            m_bonusLabel.text = string.Empty;
        }

        private void ShowStatBuffs(List<IEquipmentStatBoostModule> statBuffs)
        {

            //Transform parentTransform = m_statRow.transform.parent;
            for (int i = 0; i < statBuffs.Count; i++)
            {
                var gameobject = Instantiate(m_statRow, m_parentTransform.transform);
                gameobject.gameObject.SetActive(true);
                gameobject.name = $"Row - StatBuff ({i + 1})";
                m_instantiatedRows.Add(gameobject);

                gameobject.GetComponent<EquipmentStatBuffUI>().Display(statBuffs[i]);
            }

        }
        
        private void SetSkillBonusLabel(SoulSkill soulSkill)
        {
            m_bonusLabel.text = soulSkill.description;
        }

        private void Reset()
        {
            foreach (GameObject row in m_instantiatedRows)
                Destroy(row);

            m_instantiatedRows.Clear();
        }
    }
}
