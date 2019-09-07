using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Systems;
using UnityEngine;

namespace DChild.Gameplay.SoulEssence
{
    public class SoulEssenceLoot : Loot
    {
        [SerializeField, Min(1)]
        private int m_value;

        protected override void ApplyPickUp()
        {
            base.ApplyPickUp();
            m_pickedBy.inventory.AddSoulEssence(m_value);
            //CallPoolRequest();
        }
    }
}