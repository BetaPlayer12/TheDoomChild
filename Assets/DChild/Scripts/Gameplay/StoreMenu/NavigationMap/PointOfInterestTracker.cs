using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{

    public enum PointsOfInterests
    {
        savePoint,
        obstacle,
        realmGate,

    }   
    public struct PointOfInterestStateChangeEvent : IEventActionArgs
    {
        public PointOfInterestStateChangeEvent(string varName, bool isRevealed)
        {
            this.varName = varName;
            this.isRevealed = isRevealed;
        }

        public string varName { get; }
        public bool isRevealed { get; }
    }
    public class PointOfInterestTracker : MonoBehaviour
    {

        [SerializeField, VariablePopup(true), HideInPrefabAssets]
        private string m_varName;
        [ShowInInspector, HideInPrefabAssets]
        private bool m_isRevealed = true;
        [SerializeField]
        private PointsOfInterests m_pointOfInterests;
        

        public event EventAction<PointOfInterestStateChangeEvent>RevealValueChange;

        public string varName => m_varName;
        public bool isRevealed => m_isRevealed;

        public void SetState(bool isRevealed)
        {
            SetStateAs(isRevealed);
            RevealValueChange?.Invoke(this, new PointOfInterestStateChangeEvent(m_varName, m_isRevealed));
        }

        public void SetStateAs(bool isRevealed)
        {
            m_isRevealed = isRevealed;
     
        }


    }
}

