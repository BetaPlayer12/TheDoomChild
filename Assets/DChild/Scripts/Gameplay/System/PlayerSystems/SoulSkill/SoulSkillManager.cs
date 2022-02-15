using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.SoulSkills.UI;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DChild.Gameplay.SoulSkills
{
    public class SoulSkillManager : MonoBehaviour
    {
        [SerializeField]
        private SoulSkillList m_completeSoulSkillList;
        [SerializeField]
        private PlayerSoulSkillHandle m_playerHandle;

        /*Things I Need:
        -Something to Show Available SoulSkills
        -Something to Activate/Deactivate SoulSkills
        */
        [SerializeField, BoxGroup("UI")]
        private SoulSkillSelection m_skillSelection;
        [SerializeField, BoxGroup("UI")]
        private ActivatedSoulSkillListUI m_activatorUI;
        [SerializeField, BoxGroup("UI")]
        private SoulSkillListUI m_listUI;
        [SerializeField, BoxGroup("UI")]
        private SoulSkillInfoUI m_infoUI;

        public void ActivateSoulSkill(int soulSkillID)
        {
            ActivateSoulSkill(m_completeSoulSkillList.GetInfo(soulSkillID));
        }

        public void DeactivateSoulSkill(int soulSkillID)
        {
            DeactivateSoulSkill(m_completeSoulSkillList.GetInfo(soulSkillID));
        }

        public void ActivateSoulSkill(SoulSkill soulSkill)
        {
            if (m_playerHandle.CanBeActivated(soulSkill))
            {
                m_playerHandle.AddAsActivated(soulSkill);
                m_activatorUI.ActivateSoulSkill(soulSkill);
                m_listUI.MakeUnavailable(soulSkill.id);

                m_activatorUI.DisplayCapacity(m_playerHandle.currentSoulCapacity);
            }
            else
            {
                //Do Something to warn player why it cant be activated
            }
        }

        public void DeactivateSoulSkill(SoulSkill soulSkill)
        {
            bool willLastButtonBeDeleted = m_listUI.GetButton(m_playerHandle.activatedSkills.Count - 1).soulSkillID == soulSkill.id;

            m_playerHandle.RemoveAsActivated(soulSkill);
            m_activatorUI.DeactivateSoulSkill(soulSkill);
            m_listUI.MakeAvailable(soulSkill.id);

            m_activatorUI.DisplayCapacity(m_playerHandle.currentSoulCapacity);

            if (willLastButtonBeDeleted)
            {
                var activatedSkillCount = m_playerHandle.activatedSkills.Count;
                if (activatedSkillCount == 0)
                {
                    EventSystem.current.SetSelectedGameObject(m_listUI.GetButton(0).gameObject);
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(m_listUI.GetButton(activatedSkillCount - 1).gameObject);
                }
            }
        }

        public void SetAsActivatedSoulSkills(IReadOnlyCollection<int> list)
        {
            m_listUI.InitializeListActivatedState(list);
            List<SoulSkill> activatedSoulSkills = new List<SoulSkill>();
            for (int i = 0; i < list.Count; i++)
            {
                activatedSoulSkills.Add(m_completeSoulSkillList.GetInfo(list.ElementAt(i)));
            }
            m_activatorUI.SetAsActivedSoulSkills(activatedSoulSkills);

            m_activatorUI.DisplayCapacity(m_playerHandle.currentSoulCapacity);
        }

        public void SetAvailableSoulSkills(IReadOnlyCollection<int> list)
        {
            m_listUI.InitializeListAvailability(list);
        }

        private void OnSoulSkillSelected(object sender, SoulSkillSelected eventArgs)
        {
            var soulSkill = m_completeSoulSkillList.GetInfo(eventArgs.soulskillUI.soulSkillID);
            m_infoUI.DisplayInfoOf(soulSkill);
        }
        private void OnSoulSkillActionRequired(object sender, SoulSkillSelected eventArgs)
        {
            var soulSkillUI = eventArgs.soulskillUI;
            if (soulSkillUI.isAnActivatedSoulSkill)
            {
                DeactivateSoulSkill(soulSkillUI.soulSkillID);
            }
            else
            {
                ActivateSoulSkill(soulSkillUI.soulSkillID);
            }
        }

        private void OnSoulSkillSaveDataLoaded(object sender, EventActionArgs eventArgs)
        {
            m_activatorUI.DisplayCapacity(m_playerHandle.currentSoulCapacity);
            var skillIDs = m_completeSoulSkillList.GetIDs();

            m_listUI.InitializeListAvailability(m_playerHandle.acquiredSkills);
            var activatedSkillIDs = m_playerHandle.activatedSkills;
            m_listUI.InitializeListActivatedState(activatedSkillIDs);

            var activatedSoulSkillList = new List<SoulSkill>();
            for (int i = 0; i < activatedSkillIDs.Count; i++)
            {
                var soulSkill = m_completeSoulSkillList.GetInfo(activatedSkillIDs.ElementAt(i));
                activatedSoulSkillList.Add(soulSkill);
                m_playerHandle.AddAsActivated(soulSkill);
            }
            m_activatorUI.SetAsActivedSoulSkills(activatedSoulSkillList);
        }

        private void OnMaxCapacityChanged(object sender, EventActionArgs eventArgs)
        {
            m_activatorUI.DisplayCapacity(m_playerHandle.currentSoulCapacity);
        }

        private void OnAvailableSkillsChanged(object sender, EventActionArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void Awake()
        {
            m_playerHandle.SaveDataLoaded += OnSoulSkillSaveDataLoaded;
            m_playerHandle.AvailableSoulSkillChanged += OnAvailableSkillsChanged;
            m_playerHandle.MaxCapacityChanged += OnMaxCapacityChanged;
            m_skillSelection.OnSelected += OnSoulSkillSelected;
            m_skillSelection.OnActionRequired += OnSoulSkillActionRequired;
            m_listUI.InitializeList(m_completeSoulSkillList);
            m_activatorUI.Reset();
        }
    }
}
