using Holysoft.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    public class NavigationMapSceneHandle : MonoBehaviour
    {
        public class ChangesInfo
        {
            private Environment.Location m_location;
            private List<string> m_fogOfWarNames;
            private List<Flag> m_fogOfWarStates;

            public int fogOfWarChanges => m_fogOfWarNames.Count;

            public ChangesInfo()
            {
                m_fogOfWarNames = new List<string>();
                m_fogOfWarStates = new List<Flag>();
            }

            public void AddFogOfWarChange(string varName, Flag state)
            {
                if (m_fogOfWarNames.Contains(varName))
                {
                    var index = m_fogOfWarNames.IndexOf(varName);
                    m_fogOfWarStates[index] = state;
                }
                else
                {
                    m_fogOfWarNames.Add(varName);
                    m_fogOfWarStates.Add(state);
                }
            }

            public string GetFogOfWarName(int index) => m_fogOfWarNames[index];
            public Flag GetFogOfWarState(int index) => m_fogOfWarStates[index];

            public void Clear()
            {
                m_fogOfWarNames.Clear();
                m_fogOfWarStates.Clear();
            }
        }

        public static ChangesInfo changes { get; private set; }

        [SerializeField, BoxGroup("Scene Details")]
        private Environment.Location m_sceneLocation;
        [SerializeField, MinValue(1), BoxGroup("Scene Details")]
        public int m_sceneIndex = 1;

        [SerializeField, TabGroup("Configurator")]
        private NavMapConfigurator m_configurator;
        [SerializeField, TabGroup("Fog Of War")]
        private FogofWarTriggerHandle m_fogOfWarHandle;
        [SerializeField, TabGroup("Point Of Interest")]
        private MapPointOfInterestHandle m_pointOfInterest;

        private void OnFogOfWarChange(object sender, FogOfWarSegmentChangeEvent eventArgs)
        {
            changes.AddFogOfWarChange(eventArgs.varName, eventArgs.revealState);
        }

        private void Start()
        {
            if (changes == null)
            {
                changes = new ChangesInfo();
            }

            m_fogOfWarHandle.Initialize();
            m_fogOfWarHandle.TriggerValueChanged += OnFogOfWarChange;
            m_fogOfWarHandle.LoadStates();

            m_pointOfInterest.Initialize();

            m_pointOfInterest.LoadStates();

            GameplaySystem.gamplayUIHandle.UpdateNavMapConfiguration(m_sceneLocation, m_configurator.inGameReferencePoint, m_configurator.mapReferencePoint, m_configurator.offset);
        }
    }
}
