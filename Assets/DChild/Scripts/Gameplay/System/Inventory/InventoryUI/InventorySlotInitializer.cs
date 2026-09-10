using Doozy.Runtime.UIManager.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Inventories.UI
{
    public enum ItemSprite
    {
        KeystoneFragment,
        ShadowShard,
        HealthShard,
        Default
    }

    public class InventorySlotInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerInventoryUIHandle m_handle;
        [SerializeField] private InventoryUISwapHandle m_swapHandle;
        [SerializeField] private InventoryConditionalSpritesUI m_spriteCheckerUI;
        [SerializeField] private UIToggleGroup m_itemGroup;

        private readonly Dictionary<UIToggle, UnityAction<bool>> m_toggleListeners =
            new Dictionary<UIToggle, UnityAction<bool>>();
        private Coroutine m_bindTogglesRoutine;

        private ItemSprite SetIconSprite(string itemName)
        {
            if (itemName.Contains("Health Shard"))
                return ItemSprite.HealthShard;

            if (itemName.Contains("Shadow Shard"))
                return ItemSprite.ShadowShard;

            if (itemName.Contains("Keystone"))
                return ItemSprite.KeystoneFragment;

            return ItemSprite.Default;
        }

        private void AddToggleListener(UIToggle toggle)
        {
            if (toggle == null || m_toggleListeners.ContainsKey(toggle))
                return;

            var item = toggle.GetComponent<InventoryItemUI>();
            if (item == null)
                return;

            UnityAction<bool> listener = isOn => m_swapHandle.OnSlotToggleChanged(item, isOn);
            m_toggleListeners.Add(toggle, listener);
            toggle.OnValueChangedCallback.AddListener(listener);
        }

        private void OnEnable()
        {
            BindCurrentToggles();
            m_bindTogglesRoutine = StartCoroutine(BindTogglesNextFrame());
        }

        private void OnDisable()
        {
            if (m_bindTogglesRoutine != null)
            {
                StopCoroutine(m_bindTogglesRoutine);
                m_bindTogglesRoutine = null;
            }

            foreach (var pair in m_toggleListeners)
            {
                if (pair.Key != null)
                    pair.Key.OnValueChangedCallback.RemoveListener(pair.Value);
            }

            m_toggleListeners.Clear();
        }

        private IEnumerator BindTogglesNextFrame()
        {
            yield return null;
            m_bindTogglesRoutine = null;
            BindCurrentToggles();
        }

        private void BindCurrentToggles()
        {
            foreach (var toggle in m_itemGroup.toggles)
                AddToggleListener(toggle);
        }
    }
}
