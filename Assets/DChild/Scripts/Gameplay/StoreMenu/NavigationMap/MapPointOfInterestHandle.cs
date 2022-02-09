using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    [System.Serializable]
    public class MapPointOfInterestHandle
    {
        [SerializeField]
        private MapPointOfInterestTracker[] m_mapPointTracker;

       // public event EventAction<FogOfWarStateChangeEvent> TriggerValueChanged;

        public void Initialize()
        {
            for (int i = 0; i < m_mapPointTracker.Length; i++)
            {
                var pointOfInterest = m_mapPointTracker[i];
               // pointOfInterest.RevealValueChange += OnValueChanged;
            }
        }

        public void LoadStates()
        {
            for (int i = 0; i < m_mapPointTracker.Length; i++)
            {
                var pointOfInterest = m_mapPointTracker[i];
                var isRevealed = DialogueLua.GetVariable(pointOfInterest.varName).asBool;
                pointOfInterest.SetStateAs(isRevealed);
            }
        }

        private void OnValueChanged(object sender, FogOfWarStateChangeEvent eventArgs)
        {
            DialogueLua.SetVariable(eventArgs.varName, eventArgs.isRevealed);
            //TriggerValueChanged?.Invoke(this, eventArgs);
        }
    }
}
