﻿using DChild.Gameplay.SoulEssence;

namespace DChild.Gameplay.Systems
{
    public interface ILootHandler
    {
        void DropLoot(LootDropRequest request);
    }
}