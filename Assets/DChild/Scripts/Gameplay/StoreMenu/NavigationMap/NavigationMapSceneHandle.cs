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
            private List<bool> m_fogOfWarIsRevealed;

            public int fogOfWarChanges => m_fogOfWarNames.Count;

            public ChangesInfo()
            {
                m_fogOfWarNames = new List<string>();
                m_fogOfWarIsRevealed = new List<bool>();
            }

            public void AddFogOfWarChange(string varName, bool isRevealed)
            {
                m_fogOfWarNames.Add(varName);
                m_fogOfWarIsRevealed.Add(isRevealed);
            }

            public string GetFogOfWarName(int index) => m_fogOfWarNames[index];
            public bool GetFogOfWarState(int index) => m_fogOfWarIsRevealed[index];

            public void Clear()
            {
                m_fogOfWarNames.Clear();
                m_fogOfWarIsRevealed.Clear();
            }
        }

        public static ChangesInfo changes { get; } = new ChangesInfo();

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

        private void OnFogOfWarChange(object sender, FogOfWarStateChangeEvent eventArgs)
        {
            changes.AddFogOfWarChange(eventArgs.varName, eventArgs.isRevealed);
        }

        private void Start()
        {
            m_fogOfWarHandle.Initialize();
            m_fogOfWarHandle.TriggerValueChanged += OnFogOfWarChange;
            m_fogOfWarHandle.LoadStates();

            m_pointOfInterest.Initialize();
             
            m_pointOfInterest.LoadStates();

            GameplaySystem.gamplayUIHandle.UpdateNavMapConfiguration(m_sceneLocation, m_configurator.inGameReferencePoint, m_configurator.mapReferencePoint, m_configurator.offset);
        }
    }
}
