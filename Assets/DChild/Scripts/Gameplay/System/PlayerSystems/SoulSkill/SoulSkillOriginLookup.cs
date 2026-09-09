using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.EquipmentSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.SoulSkills.UI
{
    public sealed class SoulSkillOriginLookup : MonoBehaviour
    {
        [SerializeField]
        private SoulEquipmentList m_equipmentList;

        private readonly Dictionary<int, List<SoulEquipmentItem>> m_origins = new();

        public void Initialize()
        {
            m_origins.Clear();

            foreach (int equipmentID in m_equipmentList.GetIDs())
            {
                var item = m_equipmentList.GetInfo(equipmentID);

                if (item == null || item.soulEquipment == null || item.soulEquipment.soulSkillList == null)
                    continue;

                foreach (SoulSkill skill in item.soulEquipment.soulSkillList)
                {
                    if (skill == null)
                        continue;

                    if (!m_origins.TryGetValue(skill.id, out var equipment))
                    {
                        equipment = new List<SoulEquipmentItem>();
                        m_origins.Add(skill.id, equipment);
                    }

                    if (!equipment.Contains(item))
                        equipment.Add(item);
                }
            }
        }

        public IReadOnlyList<SoulEquipmentItem> GetOrigins(int soulSkillID)
        {
            return m_origins.TryGetValue(soulSkillID, out var equipment)
                ? equipment
                : Array.Empty<SoulEquipmentItem>();
        }
    }
}
