using Doozy.Runtime.UIManager.Components;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Menu.Codex
{
    public abstract class CodexIndexInitializer<DatabaseAssetType> : MonoBehaviour where DatabaseAssetType:DatabaseAsset
    {
        [SerializeReference]
        private CodexHandle<DatabaseAssetType> m_handle;
        [SerializeField]
        private UIToggleGroup m_itemGroup;

        private void AddToggleOnListener(UIToggle toggle)
        {
            var button = toggle.GetComponent<CodexIndexButton<DatabaseAssetType>>();
            UnityAction action = delegate { OnItemSelected(button); };
            toggle.OnToggleOnCallback.Event.AddListener(action);
            toggle.OnInstantToggleOnCallback.Event.AddListener(action);
        }

        private void OnItemSelected(CodexIndexButton<DatabaseAssetType> button)
        {
            m_handle.Select(button);
        }

        private IEnumerator Start()
        {
            while (m_itemGroup.numberOfToggles == 0)
                yield return null;

            var toggles = m_itemGroup.toggles;
            AddToggleOnListener(m_itemGroup.FirstToggle);
            for (int i = 0; i < toggles.Count; i++)
            {
                var toggle = toggles[i];
                AddToggleOnListener(toggle);
            }

            Debug.Log("Bestiary Index Initialized: " + m_itemGroup.numberOfToggles);
        }
    }
}