using DChild.Gameplay.SoulEssence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class LootHandler : MonoBehaviour, ILootHandler
    {
        [SerializeField]
        private SoulEssenceDropHandler m_soulEssenceDropHandler;

        public void Drop(SoulEssenceDropInfo info, Vector2 position)
        {
            m_soulEssenceDropHandler.Drop(info, position);
        }
    }
}