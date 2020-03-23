using DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Essence
{

    public class SoulEssenceLoot : EssenceLoot
    {
        [SerializeField, Min(1)]
        private int m_value;

        protected override void OnApplyPickup(IPlayer player)
        {
            player.inventory.AddSoulEssence(m_value);
        }
    }
}