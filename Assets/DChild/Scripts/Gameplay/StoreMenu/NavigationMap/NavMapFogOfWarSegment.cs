using Sirenix.OdinInspector;
using UnityEngine;
using Holysoft.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.NavigationMap
{
    public class NavMapFogOfWarSegment : SerializedMonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, OnValueChanged("UpdateList")]
        private string m_name;
        [InfoBox("List Should Only Have a Maximum Of 32 Elements", InfoMessageType = InfoMessageType.Warning, VisibleIf = "isListExceedLimit")]

        private bool isListExceedLimit => m_list.Length > 32;
#endif
        [SerializeField, ListDrawerSettings(ShowIndexLabels = true, HideRemoveButton = true, HideAddButton = true)]
        private NavMapFogOfWarGroup[] m_list;

        public void SetUIState(Flag state)
        {
            for (int i = 0; i < m_list.Length; i++)
            {
                var currentFlag = (Flag)(1 << i);
                var isRevealed = state.HasFlag(currentFlag);
                m_list[i].RevealArea(isRevealed);
            }
        }

        [ContextMenu("Update List")]
        private void UpdateList()
        {
            m_list = GetComponentsInChildren<NavMapFogOfWarGroup>(true);
#if UNITY_EDITOR
            if (PrefabUtility.IsPartOfPrefabAsset(gameObject) == false)
            {
                UpdateUINames();
            }
#endif
        }

        [ContextMenu("Update UI Names")]
        private void UpdateUINames()
        {
            var name = m_name == string.Empty ? "NoNameFOG" : (m_name + "FOG");
            gameObject.name = name;
            for (int i = 0; i < m_list.Length; i++)
            {
                m_list[i].RenameGameObject(name, i);
            }
        }
    }
}