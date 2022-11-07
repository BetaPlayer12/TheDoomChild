using Doozy.Runtime.UIManager.Components;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
#endif

namespace DChild.Menu.Bestiary
{
    public class BestiaryIndexInitializer : MonoBehaviour
    {
        [SerializeField]
        private BestiaryHandle m_handle;
        [SerializeField]
        private UIToggleGroup m_itemGroup;

        private void OnItemSelected(BestiaryIndexButton tradeFilter)
        {
            m_handle.Select(tradeFilter);
        }

        private void AddToggleOnListener(UIToggle toggle)
        {
            var tradeFilter = toggle.GetComponent<BestiaryIndexButton>();
            UnityAction action = delegate { OnItemSelected(tradeFilter); };
            toggle.OnToggleOnCallback.Event.AddListener(action);
            toggle.OnInstantToggleOnCallback.Event.AddListener(action);
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