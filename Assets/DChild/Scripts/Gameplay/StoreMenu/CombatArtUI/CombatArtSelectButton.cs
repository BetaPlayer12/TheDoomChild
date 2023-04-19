using DChild.Gameplay.Characters.Players;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;

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

        [SerializeField]
        private BattleAbility m_toUnlock;
        private State m_currentState = State.Unlocked;
        private UIToggle m_toggle;

        public BattleAbility skillUnlock => m_toUnlock;
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
    }

}