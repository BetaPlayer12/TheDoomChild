using DChild.Gameplay.Characters.Players.SoulSkills;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.SoulSkills.UI
{
    public class SoulSkillListUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_uiListParent;
        private Dictionary<int, SoulSkillUI> m_uiPair;

        public SoulSkillUI GetButton(int index) => m_uiPair.Values.ElementAt(index);

        public void MakeAvailable(int soulSkillID)
        {
            m_uiPair[soulSkillID].Show(false);
        }

        public SoulSkillUI MakeAvailableAndGetUI(int soulSkillID)
        {
            var button = m_uiPair[soulSkillID];
            button.Show(false);
            return button;
        }

        public void MakeUnavailable(int soulSkillID)
        {
            m_uiPair[soulSkillID].Hide(false);
        }

        public void MakeAllAvailable()
        {
            foreach (var ui in m_uiPair.Values)
            {
                ui.Show(false);
            }
        }

        public void MakeAllUnavailable()
        {
            foreach (var ui in m_uiPair.Values)
            {
                ui.Hide(false);
            }
        }

        public void SetActivatedUIState(int soulSkillID, bool isActivated)
        {
            m_uiPair[soulSkillID].SetIsAnActivatedUIState(isActivated);
        }

        public void InitializeListAvailability(IReadOnlyCollection<int> availableSoulSkillIDs)
        {
            foreach (var ui in m_uiPair.Values)
            {
                if (availableSoulSkillIDs.Contains(ui.soulSkillID))
                {
                    ui.Show(true);
                }
                else
                {
                    ui.Hide(true);
                }
            }
        }

        public void InitializeListActivatedState(IReadOnlyCollection<int> activatedoulSkillIDs)
        {
            foreach (var ui in m_uiPair.Values)
            {
                ui.SetIsAnActivatedUIState(activatedoulSkillIDs.Contains(ui.soulSkillID));
            }
        }

        public void InitializeList(SoulSkillList m_completeSoulSkillList)
        {
            if (m_uiPair == null)
            {
                m_uiPair = new Dictionary<int, SoulSkillUI>();
            }
            m_uiPair.Clear();
            var uiList = m_uiListParent.GetComponentsInChildren<SoulSkillUI>();
            var idList = m_completeSoulSkillList.GetIDs();

            for (int i = 0; i < m_completeSoulSkillList.Count; i++)
            {
                var id = idList[i];
                var ui = uiList[i];
                ui.DisplayAs(m_completeSoulSkillList.GetInfo(id));
                m_uiPair.Add(id, ui);
            }
        }
    }
}
