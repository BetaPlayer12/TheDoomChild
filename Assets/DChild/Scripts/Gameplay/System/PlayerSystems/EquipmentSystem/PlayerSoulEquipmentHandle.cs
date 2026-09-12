using DChild.Gameplay.Characters.Player.CombatArt.Leveling;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.SoulSkills;
using Holysoft.Collections;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.EquipmentSystem
{
    public class PlayerSoulEquipmentHandle : SerializedMonoBehaviour, ISerializable<PlayerSoulEquipmentData>
    {
        [System.Serializable]
        public class LeveledEquipment
        {
            [SerializeField]
            private SoulEquipmentItem m_item;
            [SerializeField]
            private int m_exp;

            public LeveledEquipment(SoulEquipmentItem item, int exp)
            {
                m_item = item;
                m_exp = exp;
            }

            public SoulEquipmentItem item => m_item;
            public int exp => m_exp;

            public void GainEXP(int exp)
            {
                m_exp += exp;
            }
        }

        [SerializeField]
        private SoulEquipmentList m_data;

        [SerializeField]
        private IPlayer m_player;

        [SerializeField]
        private PlayerSoulSkillHandle m_soulSkillHandle;

        [SerializeField]
        private Attacker m_attacker;

        [SerializeField]
        private Dictionary<SoulSlot, SoulEquipmentItem> m_equippedSoulSlotEquipmentPair = new Dictionary<SoulSlot, SoulEquipmentItem>();

        [SerializeField]
        private List<LeveledEquipment> m_acquiredSoulEquipment = new List<LeveledEquipment>();

        private List<LeveledEquipment> m_eqiuppedItems = new List<LeveledEquipment>();

        private void OnEnable()
        {
            m_player.inventory.SoulEquipmentAcquired += OnSoulEquipmentAcquired;
            m_player.attackModule.TargetDamaged += OnTargetDamaged;
            m_player.damageableModule.DamageTaken += OnDamageTaken;
        }

        private void OnDisable()
        {
            m_player.inventory.SoulEquipmentAcquired -= OnSoulEquipmentAcquired;
            m_player.attackModule.TargetDamaged -= OnTargetDamaged;
            m_player.damageableModule.DamageTaken -= OnDamageTaken;
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if(m_player.damageableModule.isAlive == false)
            {
                //Unequip all equipment on death
                for(int i = m_eqiuppedItems.Count - 1; i >= 0; i--)
                {
                    UnequipSoulEquipment(m_eqiuppedItems[i].item);
                }
            }
        }

        private void OnSoulEquipmentAcquired(object sender, SoulEquipmentAcquiredEventArgs eventArgs)
        {
            AddAcquiredSoulEquipmentItem(eventArgs.Item);
        }


        private void OnTargetDamaged(object sender, CombatConclusionEventArgs eventArgs)
        {
            if (eventArgs.target.instance.isAlive)
                return;

            if (m_eqiuppedItems.Count <= 0)
                return;

            var expPoints = eventArgs.target.instance.transform.GetComponent<CombatArtExperienceDropper>().Data.exp;

            for (int i = 0; i < m_eqiuppedItems.Count; i++)
            {
                if (m_acquiredSoulEquipment.Contains(m_eqiuppedItems[i]))
                {
                    for (int j = 0; j < m_acquiredSoulEquipment.Count; j++)
                    {
                        if (m_acquiredSoulEquipment[j] == m_eqiuppedItems[i])
                        {
                            m_acquiredSoulEquipment[j].GainEXP(expPoints);

                            if (m_acquiredSoulEquipment[j].exp >= m_acquiredSoulEquipment[j].item.soulEquipment.ExpRequired)
                            {
                                foreach (SoulSkill skill in m_acquiredSoulEquipment[j].item.soulEquipment.soulSkillList)
                                {
                                    skill.SetFullyLearned(true);
                                    m_soulSkillHandle.AddAsAcquired(skill.id);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void LoadData(PlayerSoulEquipmentData data)
        {
            if(m_eqiuppedItems.Count > 0)
            {
                for(int i = m_eqiuppedItems.Count - 1; i >= 0; i--)
                {
                    if (m_eqiuppedItems[i] != null)
                    {
                        UnequipSoulEquipment(m_eqiuppedItems[i].item);
                    }
                }
            }

            if (data != null)
            {
                m_equippedSoulSlotEquipmentPair.Clear();
                m_acquiredSoulEquipment.Clear();
                m_eqiuppedItems.Clear();

                Dictionary<int, LeveledEquipment> equipmentIDPair = new Dictionary<int, LeveledEquipment>();

                for (int i = 0; i < data.acquiredEquipmentID.Length; i++)
                {
                    SoulEquipmentItem item = m_data.GetInfo(data.acquiredEquipmentID[i]);
                    var currentExp = Mathf.FloorToInt(item.soulEquipment.ExpRequired * data.equipmentExpPercent[i]);
                    var leveledEquipment = new LeveledEquipment(item, currentExp);

                    AddAcquiredSoulEquipmentItem(leveledEquipment.item);
                    equipmentIDPair.Add(item.id, leveledEquipment);
                }

                for (int i = 0; i < (int)SoulSlot._COUNT; i++)
                {
                    var id = data.equippedEquipmentID[i];
                    if (id != -1)
                    {
                        var levelEquipment = equipmentIDPair[id];
                        EquipSoulEquipment(levelEquipment.item);
                    }
                }
            }
        }

        public PlayerSoulEquipmentData SaveData()
        {
            return new PlayerSoulEquipmentData(m_acquiredSoulEquipment, m_equippedSoulSlotEquipmentPair);
        }

        [Button]
        public void EquipSoulEquipment(SoulEquipmentItem soulEquipment)
        {
            TryEquipSoulEquipment(soulEquipment);
        }

        public bool TryEquipSoulEquipment(SoulEquipmentItem soulEquipment)
        {
            if (soulEquipment == null)
                return false;

            var equipment = soulEquipment.soulEquipment;

            //Check if soul equipment is acquired already 
            var isAcquired = false;
            for(int i = 0; i < m_acquiredSoulEquipment.Count; i++)
            {
                if (m_acquiredSoulEquipment[i].item == soulEquipment)
                {
                    isAcquired = true;
                }
            }

            if (isAcquired == false)
                return false;

            if (m_equippedSoulSlotEquipmentPair.TryGetValue(equipment.Slot, out SoulEquipmentItem equippedItem))
            {
                if (equippedItem == soulEquipment)
                    return true;

                if (!TryUnequipSoulEquipment(equippedItem))
                    return false;
            }

            m_equippedSoulSlotEquipmentPair.Add(equipment.Slot, soulEquipment);

            //find item to equip in acquired items
            for (int i = 0; i < m_acquiredSoulEquipment.Count; i++)
            {
                if (m_acquiredSoulEquipment[i].item == soulEquipment)
                {
                    m_eqiuppedItems.Add(m_acquiredSoulEquipment[i]);
                }
            }

            //Logic for setting soul skill as activated when equipped
            foreach (SoulSkill soulSkill in equipment.soulSkillList)
            {
                m_soulSkillHandle.AddAsActivated(soulSkill, false);
            }

            foreach(IEquipmentStatBoostModule statBoost in equipment.statBoostList)
            {
                statBoost.AttachTo(m_player);
            }

            return true;
        }

        [Button]
        public void UnequipSoulEquipment(SoulEquipmentItem soulEquipment)
        {
            TryUnequipSoulEquipment(soulEquipment);
        }

        public bool TryUnequipSoulEquipment(SoulEquipmentItem soulEquipment)
        {
            if (soulEquipment == null)
                return false;

            var equipment = soulEquipment.soulEquipment;

            //Check if soul equipment is acquired already 
            var isAcquired = false;
            for (int i = 0; i < m_acquiredSoulEquipment.Count; i++)
            {
                if (m_acquiredSoulEquipment[i].item == soulEquipment)
                {
                    isAcquired = true;
                }
            }

            if (isAcquired == false)
                return false;

            if (!m_equippedSoulSlotEquipmentPair.TryGetValue(equipment.Slot, out SoulEquipmentItem equippedItem) ||
                equippedItem != soulEquipment)
                return false;

            m_equippedSoulSlotEquipmentPair.Remove(equipment.Slot);
            m_eqiuppedItems.RemoveAll(item => item.item == soulEquipment);

            foreach (SoulSkill soulSkill in equipment.soulSkillList)
            {
                m_soulSkillHandle.RemoveAsActivated(soulSkill, false);
            }


            foreach (IEquipmentStatBoostModule statBoost in equipment.statBoostList)
            {
                statBoost.DetachFrom(m_player);
            }

            return true;
        }

        [Button]
        public void AddAcquiredSoulEquipmentItem(SoulEquipmentItem soulEquipment)
        {
            var leveledItem = new LeveledEquipment(soulEquipment, 0);

            var equipment = soulEquipment.soulEquipment;
            m_acquiredSoulEquipment.Add(leveledItem);
            //Logic to set soul skills in acquired equipment as activated
            foreach (SoulSkill soulSkill in equipment.soulSkillList)
            {
                m_soulSkillHandle.AddAsAcquired(soulSkill.id);
                m_soulSkillHandle.SetActivationRestriction(soulSkill.id, false);
            }
        }

        public List<LeveledEquipment> GetAcquiredSoulEquipment()
        {
            return m_acquiredSoulEquipment;
        }

        public SoulEquipmentList GetFullSoulEquipmentList()
        {
            return m_data;
        }

        public bool TryGetEquippedSoulEquipment(SoulSlot slot, out SoulEquipmentItem equipmentItem)
        {
            return m_equippedSoulSlotEquipmentPair.TryGetValue(slot, out equipmentItem);
        }
    }
}

