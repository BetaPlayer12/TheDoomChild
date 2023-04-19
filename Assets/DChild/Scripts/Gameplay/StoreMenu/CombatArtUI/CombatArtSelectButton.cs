using DChild.Gameplay.Characters.Players;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.UI.CombatArts
{
    public class CombatArtSelectButton : MonoBehaviour
    {
        public enum State
        {
            Locked,
            Unlockable,
            Unlocked
        }

        [SerializeField, HideInPrefabAssets, OnValueChanged("OnConfigurationChanged")]
        private BattleAbility m_toUnlock;
        [SerializeField, HideInPrefabAssets, OnValueChanged("OnConfigurationChanged"), MinValue(1)]
        private int m_unlockLevel = 1;
        private State m_currentState = State.Unlocked;
        private UIToggle m_toggle;

        public BattleAbility skillUnlock => m_toUnlock;
        public int unlockLevel => m_unlockLevel;
        public State currentState => m_currentState;

        public void SetState(State state)
        {
            m_currentState = state;
            if (state == State.Unlocked)
            {
                m_toggle.SetIsOn(true);
            }
            else
            {
                m_toggle.SetIsOn(false);
            }
        }

#if UNITY_EDITOR
        private void OnConfigurationChanged()
        {
            gameObject.name = "CombatArtSelectable_" + m_toUnlock.ToString().Replace(" ", "") + m_unlockLevel.ToString();
        }
#endif
    }

}