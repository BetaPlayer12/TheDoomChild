using Sirenix.OdinInspector;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;
using Holysoft.Event;
using Holysoft.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.NavigationMap
{
    public struct FogOfWarSegmentChangeEvent : IEventActionArgs
    {
        public FogOfWarSegmentChangeEvent(string varName, Flag revealState)
        {
            this.varName = varName;
            this.revealState = revealState;
        }

        public string varName { get; }
        public Flag revealState { get; }
    }

    public class FogOfWarSegment : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, OnValueChanged("UpdateList")]
        private string m_name;
        [InfoBox("List Should Only Have a Maximum Of 32 Elements", InfoMessageType = InfoMessageType.Warning, VisibleIf = "isListExceedLimit")]

        private bool isListExceedLimit => m_list.Length > 32;
#endif

        [SerializeField, VariablePopup(true), HideInPrefabAssets]
        private string m_varName;
        [SerializeField, ListDrawerSettings(ShowIndexLabels = true, HideRemoveButton = true, HideAddButton = true), TabGroup("Triggers")]
        private FogofWarTrigger[] m_list;

        [SerializeField, DisableInPlayMode, HideInEditorMode, EnumToggleButtons(), TabGroup("State")]
        private Flag m_currentState;

        public string varName => m_varName;

        public event EventAction<FogOfWarSegmentChangeEvent> SegmentUpdate;

        public void SetStateAs(Flag state)
        {
            m_currentState = state;
        }

        private void OnStateChange(object sender, FogOfWarStateChangeEvent eventArgs)
        {
            var flag = eventArgs.index;
            if (eventArgs.isRevealed)
            {
                m_currentState |= flag;
            }
            else
            {
                m_currentState &= flag;
            }

            SegmentUpdate?.Invoke(this, new FogOfWarSegmentChangeEvent(m_varName, m_currentState));
        }

#if UNITY_EDITOR
        [ContextMenu("Update List")]
        private void UpdateList()
        {
            m_list = GetComponentsInChildren<FogofWarTrigger>(true);
            if (PrefabUtility.IsPartOfPrefabAsset(gameObject) == false)
            {
                UpdateUINames();
            }
        }

        [ContextMenu("Update UI Names")]
        private void UpdateUINames()
        {
            var name = m_name == string.Empty ? "NoNameFOG" : (m_name + "FOG");
            gameObject.name = name;
            for (int i = 0; i < m_list.Length; i++)
            {
                m_list[i].gameObject.name = name + i;
            }
        }
#endif

        private void Awake()
        {
            for (int i = 0; i < m_list.Length; i++)
            {
                var trigger = m_list[i];
                trigger.SetIndex(i);
                trigger.RevealValueChange += OnStateChange;
            }
        }


    }
}
