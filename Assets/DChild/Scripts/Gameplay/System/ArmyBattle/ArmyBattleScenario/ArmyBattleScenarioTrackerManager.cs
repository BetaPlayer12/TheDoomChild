using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleScenarioTrackerManager
    {
        [SerializeField, ValueDropdown("GetTrackerTypes", IsUniqueList = true), ListDrawerSettings(DraggableItems = false), HideReferenceObjectPicker]
        private ArmyBattleTrackerType[] m_trackersTypesToCreate = new ArmyBattleTrackerType[0];
        [SerializeField, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, DraggableItems = false), HideReferenceObjectPicker]
        private IArmyBattleScenarioTracker[] m_trackers = new IArmyBattleScenarioTracker[0];

        public void UpdateTrackerValues()
        {
            for (int i = 0; i < m_trackers.Length; i++)
            {
                try
                {
                    m_trackers[i].UpdateValue();
                }
                catch(Exception e)
                {
                    Debug.LogError($"ARMY BATTLE SCENARIO TRACKER {m_trackers[i].type} has a Error, Read notes Below");
                    Debug.LogException(e);
                }
            }
        }

        [Button]
        private void GenerateTrackers()
        {
            List<IArmyBattleScenarioTracker> trackers = new List<IArmyBattleScenarioTracker>();
            for (int i = 0; i < m_trackersTypesToCreate.Length; i++)
            {
                var trackerType = m_trackersTypesToCreate[i];
                var tracker = GetTrackerOfType(trackerType);
                if (tracker == null)
                {
                    tracker = ArmyBattleScenarioTrackerCreator.CreateTracker(trackerType);
                }
                trackers.Add(tracker);
            }

            m_trackers = trackers.ToArray();
        }

        private IArmyBattleScenarioTracker GetTrackerOfType(ArmyBattleTrackerType trackerType)
        {
            for (int i = 0; i < m_trackers.Length; i++)
            {
                var tracker = m_trackers[i];
                if (tracker.type == trackerType)
                    return tracker;
            }

            return null;
        }

        private IEnumerable GetTrackerTypes()
        {
            ValueDropdownList<ArmyBattleTrackerType> dropdownList = new ValueDropdownList<ArmyBattleTrackerType>();
            for (int i = 0; i < (int)ArmyBattleTrackerType._COUNT; i++)
            {
                dropdownList.Add((ArmyBattleTrackerType)i);
            }

            return dropdownList;
        }
    }
}
