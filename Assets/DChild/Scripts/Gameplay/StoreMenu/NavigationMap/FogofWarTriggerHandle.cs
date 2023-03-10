using Holysoft.Collections;
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
        private FogOfWarSegment[] m_segmentList;

        public event EventAction<FogOfWarSegmentChangeEvent> TriggerValueChanged;

        public void Initialize()
        {
            for (int i = 0; i < m_segmentList.Length; i++)
            {
                var fogOfWar = m_segmentList[i];
                fogOfWar.SegmentUpdate += OnSegmentUpdate;
            }
        }


        public void LoadStates()
        {
            for (int i = 0; i < m_segmentList.Length; i++)
            {
                var fogOfWar = m_segmentList[i];
                var revealState = DialogueLua.GetVariable(fogOfWar.varName).asInt;
                fogOfWar.SetStateAs((Flag)revealState);
            }
        }
        private void OnSegmentUpdate(object sender, FogOfWarSegmentChangeEvent eventArgs)
        {
            DialogueLua.SetVariable(eventArgs.varName, (int)eventArgs.revealState);
            TriggerValueChanged?.Invoke(this, eventArgs);
        }
    }
}
