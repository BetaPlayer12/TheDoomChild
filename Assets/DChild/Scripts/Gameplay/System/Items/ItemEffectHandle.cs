using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    public class ItemEffectHandle : MonoBehaviour
    {
        private IPlayer m_player;

        private Dictionary<IItemDurationEffectInfo, DurationItemHandle> m_itemEffectsPair = new Dictionary<IItemDurationEffectInfo, DurationItemHandle>();
        [SerializeField, ReadOnly]
        private List<DurationItemHandle> m_activeEffects = new List<DurationItemHandle>();

        public void ActivateEffect(IItemDurationEffectInfo itemDurationEffectInfo)
        {
            if (m_itemEffectsPair.TryGetValue(itemDurationEffectInfo, out DurationItemHandle handle))
            {
                handle.ResetTimer();
            }
            else
            {
                handle = itemDurationEffectInfo.GenerateEffectHandle(m_player);
                m_itemEffectsPair.Add(itemDurationEffectInfo, handle);
                m_activeEffects.Add(handle);
                handle.StartEffect();
            }
        }

        public void DeactivateEffect(IItemDurationEffectInfo itemDurationEffectInfo)
        {
            if (m_itemEffectsPair.TryGetValue(itemDurationEffectInfo, out DurationItemHandle handle))
            {
                handle.StopEffect();
                m_activeEffects.Remove(handle);
                m_itemEffectsPair.Remove(itemDurationEffectInfo);
            }
        }

        public void DeactivateAllEffects()
        {
            for (int i = 0; i < m_activeEffects.Count; i++)
            {
                m_activeEffects[i].StopEffect();
            }
            m_activeEffects.Clear();
            m_itemEffectsPair.Clear();
        }

        private void Start()
        {
            m_player = GameplaySystem.playerManager.player;
        }

        public void Update()
        {
            for (int i = m_activeEffects.Count - 1; i >= 0; i--)
            {
                var currentEffect = m_activeEffects[i];
                currentEffect.UpdateEffect(GameplaySystem.time.deltaTime);
                if (currentEffect.isDone)
                {
                    DeactivateEffect((IItemDurationEffectInfo)currentEffect.source);
                }
            }
        }
    }

}