using Holysoft.Event;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    [System.Serializable]
    public class FogofWarTriggerHandle
    {
        [SerializeField]
        private FogofWarTrigger[] m_fogOfWarList;

        private Environment.Location m_sceneLocation;
        private int m_sceneIndex = 1;

        public event EventAction<FogOfWarStateChangeEvent> TriggerValueChanged;

        public void Initialize(Environment.Location location, int sceneIndex)
        {
            m_sceneLocation = location;
            m_sceneIndex = sceneIndex;

            for (int i = 0; i < m_fogOfWarList.Length; i++)
            {
                var fogOfWar = m_fogOfWarList[i];
                fogOfWar.RevealValueChange += OnValueChanged;
            }
        }

        public void LoadStates()
        {
            for (int i = 0; i < m_fogOfWarList.Length; i++)
            {
                var fogOfWar = m_fogOfWarList[i];
                var isRevealed = DialogueLua.GetVariable(fogOfWar.varName).asBool;
                fogOfWar.SetStateAs(isRevealed);
            }
        }

        private void OnValueChanged(object sender, FogOfWarStateChangeEvent eventArgs)
        {
            DialogueLua.SetVariable(eventArgs.varName, eventArgs.isRevealed);
            TriggerValueChanged?.Invoke(this, eventArgs);
        }
    }
}
