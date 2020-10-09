using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [System.Serializable]
    public struct PlayerModificationItemEffect : IDurationItemEffect
    {

        [SerializeField]
        private PlayerModifier m_toModify;

        [SerializeField]
        private float m_value;

        public void StartEffect(IPlayer player)
        {
            player.modifiers.Add(m_toModify, m_value);
        }

        public void StopEffect(IPlayer player)
        {
            player.modifiers.Add(m_toModify, -m_value);
        }

        public override string ToString()
        {
            if (m_value > 0)
            {
                return $"Increase {m_toModify.ToString()} by {m_value * 100f}%";
            }
            else
            {
                return $"Decrease {m_toModify.ToString()} by {-m_value * 100f}%";
            }
        }
    }
}
