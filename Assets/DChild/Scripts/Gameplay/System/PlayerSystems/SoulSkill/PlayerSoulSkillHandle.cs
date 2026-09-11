using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.EquipmentSystem;
using DChild.Gameplay.SoulSkills.UI;
using DChild.Serialization;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.SoulSkills
{
    public class PlayerSoulSkillHandle : SerializedMonoBehaviour, ISerializable<PlayerSoulSkillData>
    {
        [SerializeField]
        private PlayerSoulSkillsConfiguration m_playerSoulSkillsConfiguration;
        [SerializeField]
        private IPlayer m_player;
        [SerializeField, MinValue(1)]
        private int m_maxActivatedSoulSkill = 9;
        [SerializeField, MinValue(1)]
        private int m_maxSoulCapacity = 1;

        [SerializeField]
        private bool m_soulskillHandleOnload = false;

        [ShowInInspector, HideInEditorMode]
        private int m_currentSoulCapacity;
        private HashSet<int> m_acquiredSkills;
        private HashSet<int> m_activatedSkillsID;
        [ShowInInspector, ReadOnly]
        private HashSet<SoulSkill> m_activatedSkills;
        private HashSet<int> m_temporaryAcquiredSkills;

        private Dictionary<int, bool> m_canBeActivatedAsPermanent;

        public int maxSoulCapacity => m_maxSoulCapacity;
        public int currentSoulCapacity => m_currentSoulCapacity;
        public IReadOnlyCollection<int> acquiredSkills => m_acquiredSkills; 
        public IReadOnlyCollection<int> temporaryAcquiredSkills => m_temporaryAcquiredSkills;
        public IReadOnlyCollection<int> activatedSkills => m_activatedSkillsID;

        public event EventAction<EventActionArgs> SaveDataLoaded;
        public event EventAction<EventActionArgs> AvailableSoulSkillChanged;
        public event EventAction<EventActionArgs> MaxCapacityChanged;


        public PlayerSoulSkillData SaveData()
        {
           var acquiredAsPermanent = m_canBeActivatedAsPermanent.Where(x => x.Value == true).Select(x => x.Key);
            return new PlayerSoulSkillData(m_currentSoulCapacity, acquiredAsPermanent.ToArray(), m_activatedSkillsID.ToArray());
        }

        public void LoadData(PlayerSoulSkillData data)
        {
            m_acquiredSkills.Clear();
            m_canBeActivatedAsPermanent.Clear();
            RemoveAllActiveSoulSkills();
            
            if (data != null)
            {
                //temporary bandaid fix
                m_currentSoulCapacity = m_maxSoulCapacity;

                for (int i = 0; i < data.acquiredSoulSkills.Length; i++)
                {
                    var skillId = data.acquiredSoulSkills[i];
                    m_acquiredSkills.Add(skillId);
                    m_canBeActivatedAsPermanent.Add(skillId, true);
                }

                for (int i = 0; i < data.activatedSoulSkills.Length; i++)
                {
                    m_activatedSkillsID.Add(data.activatedSoulSkills[i]);
                }
                m_soulskillHandleOnload = true;
                m_currentSoulCapacity = Mathf.Clamp(data.currentSoulCapacity, 0, m_maxSoulCapacity);
            }

            SaveDataLoaded?.Invoke(this, EventActionArgs.Empty);
            m_soulskillHandleOnload = false;
        }

        public void AddSoulSkillEnergyPoint(int increment)
        {
            m_currentSoulCapacity += increment;
        }

        public void AddAsAcquired(int soulSkillID)
        {
            m_acquiredSkills.Add(soulSkillID);

            if (m_canBeActivatedAsPermanent.ContainsKey(soulSkillID) == false)
            {
                m_canBeActivatedAsPermanent.Add(soulSkillID, true);
            }

            AvailableSoulSkillChanged?.Invoke(this, EventActionArgs.Empty);
        }

        public bool CanBeActivatedAsPermanent(int soulSkillID)
        {
            if(m_canBeActivatedAsPermanent.TryGetValue(soulSkillID, out var result)) { return result; }
            return false;
        }

        public void SetActivationRestriction(int soulSkillID, bool canBeActivatedAsPermanent)
        {
            m_canBeActivatedAsPermanent[soulSkillID] = canBeActivatedAsPermanent;
        }

        public void RemoveAsAcquired(int soulSkillID)
        {
            m_acquiredSkills.Remove(soulSkillID);
            AvailableSoulSkillChanged?.Invoke(this, EventActionArgs.Empty);
        }

        public bool CanBeActivated(SoulSkill soulSkill)
        {
            if (m_activatedSkillsID == null)
            {
                return false;
            }
            return m_maxActivatedSoulSkill >= m_activatedSkillsID.Count + 1 && (m_currentSoulCapacity - soulSkill.capacity) >= 0;
        }

        public bool HasAcquired(SoulSkill soulSkill)
        {
            if (m_acquiredSkills == null)
            {
                //Temporary For Testing
                return true;
            }

            return m_acquiredSkills.Contains(soulSkill.id);
        }

        public void AddAsActivated(SoulSkill soulSkill, bool asPermanent = true)
        {
            if(HasActivatedSkill(soulSkill))
            {
                return;
            }

            if (m_acquiredSkills.Contains(soulSkill.id))
            {
                m_activatedSkillsID.Add(soulSkill.id);
                m_activatedSkills.Add(soulSkill);
                soulSkill.AttachTo(m_player);
                SetActivationRestriction(soulSkill.id, asPermanent);
                //Equipment Skill check to only deduct soul capacity if skill is not equipment skill
                if (asPermanent == true && m_soulskillHandleOnload == false)
                    m_currentSoulCapacity -= soulSkill.capacity;
            }
        }

        public void RemoveAsActivated(SoulSkill soulSkill, bool asPermanent = true)
        {
            if (m_activatedSkillsID.Contains(soulSkill.id))
            {
                m_activatedSkillsID.Remove(soulSkill.id);
                m_activatedSkills.Remove(soulSkill);
                soulSkill.DetachFrom(m_player);
                //Need to add boolean check if soul skill was activated as temporary, if so do not add soul capacity
                if (asPermanent == true)
                    m_currentSoulCapacity += soulSkill.capacity;
            }
        }

        public void AddMaxCapacity(int additionCapacity)
        {
            m_maxSoulCapacity += additionCapacity;
            m_currentSoulCapacity += additionCapacity;
            MaxCapacityChanged?.Invoke(this, EventActionArgs.Empty);
        }

        public void SetMaxCapacity(int maxCapacity)
        {
            m_maxSoulCapacity = maxCapacity;
            MaxCapacityChanged?.Invoke(this, EventActionArgs.Empty);
            m_currentSoulCapacity = Mathf.Min(m_currentSoulCapacity, m_maxSoulCapacity);
        }

        public void Initialize()
        {
            m_maxActivatedSoulSkill = m_playerSoulSkillsConfiguration.maxActivatedSoulSkill;
            m_maxSoulCapacity = m_playerSoulSkillsConfiguration.maxSoulCapacity;

            m_acquiredSkills = new HashSet<int>();
            m_activatedSkillsID = new HashSet<int>();
            m_activatedSkills = new HashSet<SoulSkill>();

            m_canBeActivatedAsPermanent = new Dictionary<int, bool>();

            m_player.inventory.SoulSkillItemAcquired += OnSoulSkillItemAcquired;
        }

        public bool HasActivatedSkill(SoulSkill skill)
        {
            return m_activatedSkills.Contains(skill);
        }

        private void OnSoulSkillItemAcquired(object sender, SoulSkillAcquiredEventArgs eventArgs)
        {
            AddAsAcquired(eventArgs.ID);
        }

        private void RemoveAllActiveSoulSkills()
        {
            for (int i = 0; i < m_activatedSkills.Count; i++)
            {
                RemoveAsActivated(m_activatedSkills.ElementAt(i));
            }
            m_activatedSkills.Clear();
            m_activatedSkillsID.Clear();
        }
    }
}
