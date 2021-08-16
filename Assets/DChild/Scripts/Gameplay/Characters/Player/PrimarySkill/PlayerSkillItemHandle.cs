using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class PlayerSkillItemHandle : MonoBehaviour
    {
        [SerializeField]
        private PlayerInventory m_inventory;
        [SerializeField, HideReferenceObjectPicker]
        private Dictionary<PrimarySkill, ItemData> m_skills = new Dictionary<PrimarySkill, ItemData>();
        private void OnSkillUpdate(object sender, PrimarySkillUpdateEventArgs eventArgs)
        {
            if (m_skills.TryGetValue(eventArgs.skill, out ItemData itemData))
            {
                if (eventArgs.isEnabled)
                {
                    m_inventory.AddItem(itemData, 1);
                }
                else
                {
                    m_inventory.RemoveItem(itemData);
                }
            }
        }

        private void Awake()
        {
            GetComponent<PlayerSkills>().SkillUpdate += OnSkillUpdate;
        }
    }
}