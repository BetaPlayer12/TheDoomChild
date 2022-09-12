using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    public abstract class ConsumableItemData : ItemData
    {
        [SerializeField, MinValue(1), ToggleGroup("m_enableEdit")]
        private bool m_hasInfiniteUses;

        public override bool hasInfiniteUses => m_hasInfiniteUses;

        public abstract bool CanBeUse(IPlayer player);
        public abstract void Use(IPlayer player);
    }

    public interface IItemDurationEffectInfo
    {
        DurationItemHandle GenerateEffectHandle(IPlayer reference);
    }
}
