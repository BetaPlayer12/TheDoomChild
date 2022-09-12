using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    public struct StateChangeItemEffect : IDurationItemEffect
    {
        private enum State
        {
            Rage
        }

        [SerializeField]
        private State m_stateToChange;

        public void StartEffect(IPlayer player)
        {
            switch (m_stateToChange)
            {
                case State.Rage:
                    player.state.isEnraged = true;
                    break;
            }
        }

        public void StopEffect(IPlayer player)
        {
            switch (m_stateToChange)
            {
                case State.Rage:
                    player.state.isEnraged = false;
                    break;
            }
        }
    }
}
