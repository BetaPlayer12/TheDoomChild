using DChild.Gameplay.Characters.Players.SoulSkills;
using NUnit.Framework.Internal.Filters;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace DChild.Gameplay.SoulSkills.UI
{
    public class SoulSkillListUI : MonoBehaviour
    {
        [SerializeField]
        private SoulSkillUI[] m_uiList;

        [SerializeField]
        private SoulSkillNavigationHandle m_navigationHandle;

        [SerializeField]
        private bool m_considerAllAsAvailable;
        private Dictionary<int, SoulSkillUI> m_uiPair;

        private SoulSkillList m_completeList;

        private IReadOnlyCollection<int> m_playerAcquiredSkills;
        private IReadOnlyCollection<int> m_activatedSkills;

        public event System.Action<bool> OnPageUpdated;

        #region Deprecated
        public SoulSkillUI GetButton(int index) => m_uiPair.Values.ElementAt(index);

        public void MakeAvailable(int soulSkillID)
        {
            //m_uiPair[soulSkillID].Show(false);
        }

        public SoulSkillUI MakeAvailableAndGetUI(int soulSkillID)
        {
            var button = m_uiPair[soulSkillID];
            return button;
        }

        public void MakeUnavailable(int soulSkillID)
        {
            //m_uiPair[soulSkillID].Hide(false);
        }

        public void MakeAllAvailable()
        {
            //foreach (var ui in m_uiPair.Values)
            //{
            //    ui.Show(false);
            //}
        }

        public void MakeAllUnavailable()
        {
            //foreach (var ui in m_uiPair.Values)
            //{
            //    ui.Hide(false);
            //}
        }

        public void SetActivatedUIState(int soulSkillID, bool isActivated)
        {
            //m_uiPair[soulSkillID].SetIsAnActivatedUIState(isActivated);
        }



        public void InitializeListActivatedState(IReadOnlyCollection<int> activatedoulSkillIDs)
        {
            //foreach (var ui in m_uiPair.Values)
            //{
            //    ui.SetIsAnActivatedUIState(activatedoulSkillIDs.Contains(ui.soulSkillID));
            //}
        }
        #endregion

        #region Initialization
        public void InitializeList(SoulSkillList completeSoulSkillList)
        {
            m_navigationHandle.SetupScroll(completeSoulSkillList, m_uiList.Length);

            m_completeList = completeSoulSkillList;

            m_uiPair ??= new Dictionary<int, SoulSkillUI>();
            UpdateToggleData(0);
        }
        #endregion

        #region Soul Skill Filter Setters
        public void SetAvailableSkills(IReadOnlyCollection<int> availableSoulSkillIDs) => m_playerAcquiredSkills = availableSoulSkillIDs;
        public void SetActivatedSkills(IReadOnlyCollection<int> activatedIDs) => m_activatedSkills = activatedIDs;
        #endregion

        #region Update and Display
        public void UpdateToggleData(int pageNumber)
        {
            var allIDs = m_completeList.GetIDs();
            int itemsPerPage = m_uiList.Length;
            int startOffset = pageNumber * itemsPerPage;
            bool hasAvailableSkills = false;

            m_uiPair.Clear();

            for (int i = 0; i < itemsPerPage; i++)
            {
                int dataIndex = startOffset + i;
                bool hasData = dataIndex < allIDs.Count();

                GameObject uiObj = m_uiList[i].gameObject;
                uiObj.SetActive(hasData);

                if (hasData)
                {
                    int id = allIDs[dataIndex];
                    m_uiPair.Add(id, m_uiList[i]);
                    DisplayData(id);
                    hasAvailableSkills |= m_playerAcquiredSkills.Contains(id);
                }
            }

            OnPageUpdated?.Invoke(hasAvailableSkills);
        }

        private void DisplayData(int id)
        {
            if (!m_playerAcquiredSkills.Contains(id))
            {
                m_uiPair[id].Display(null);
                return;
            }

            var data = m_completeList.GetInfo(id);
            var isActivated = m_activatedSkills.Contains(id);
            m_uiPair[id].Display(data, isActivated);
        }
        #endregion
    }
}
