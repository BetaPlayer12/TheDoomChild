using DChild.Gameplay.Characters.Players;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using Sirenix.OdinInspector;
using static DChild.Gameplay.UI.CombatArts.CombatArtSelectButton;
using System;

namespace DChild.Gameplay.UI.CombatArts
{
    public class CombatArtSelectButton : MonoBehaviour
    {
        [SerializeField, HideInPrefabAssets, OnValueChanged("OnConfigurationChanged")]
        private CombatArt m_toUnlock;
        [SerializeField, HideInPrefabAssets, OnValueChanged("OnConfigurationChanged"), MinValue(1)]
        private int m_unlockLevel = 1;
        [ShowInInspector, ReadOnly]
        private CombatArtUnlockState m_currentState = CombatArtUnlockState.Unlockable;
        [SerializeField]
        private CombatArtSelectButtonVisual m_visuals;

        public event Action<CombatArtSelectButton> Selected;

        public CombatArt skillUnlock => m_toUnlock;
        public int unlockLevel => m_unlockLevel;
        public CombatArtUnlockState currentState => m_currentState;

        public void SetState(CombatArtUnlockState state)
        {
            m_currentState = state;
            m_visuals.SetState(state);
        }

        public void ForceVisualSync()
        {
            m_visuals.SetState(m_currentState);
        }

        public void Select()
        {
            Selected?.Invoke(this);
        }

        public void DisplayAs(CombatArtLevelData artLevelData) => m_visuals.DisplayAs(artLevelData);

        private void Awake()
        {
            m_visuals.Initialize(GetComponent<UIButton>());
        }

#if UNITY_EDITOR
        private void OnConfigurationChanged()
        {
            gameObject.name = "CombatArtSelectable_" + m_toUnlock.ToString().Replace(" ", "") + m_unlockLevel.ToString();
        }
#endif
    }

}