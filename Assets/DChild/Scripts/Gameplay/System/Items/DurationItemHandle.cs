using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [System.Serializable, ReadOnly]
    public class DurationItemHandle
    {
        private IDurationItemEffect[] m_durationItemEffects;
        private IUpdatableItemEffect[] m_updatableItemEffects;
        private bool m_hasDurationEffects;
        private bool m_hasUpdateableItemEffects;
        private float m_duration;
        [SerializeField, HorizontalGroup,LabelWidth(50)]
        private ItemData m_source;
        [SerializeField, HorizontalGroup, SuffixLabel("@m_duration", overlay: true)]
        private float m_timer;
        private bool m_isActive;

        public IPlayer player { get; }
        public ItemData source => m_source;

        public bool isDone => m_isActive == true && m_timer <= 0;
        public float durationPercent => m_timer / m_duration;

        public DurationItemHandle(IPlayer player, ItemData itemData, float duration, IDurationItemEffect[] durationItemEffects, IUpdatableItemEffect[] updatableItemEffects)
        {
            this.player = player;
            m_source = itemData;
            m_duration = duration;
            m_durationItemEffects = durationItemEffects;
            m_hasDurationEffects = durationItemEffects != null && durationItemEffects.Length > 0;
            if (updatableItemEffects != null && updatableItemEffects.Length > 0)
            {
                m_updatableItemEffects = new IUpdatableItemEffect[updatableItemEffects.Length];
                for (int i = 0; i < updatableItemEffects.Length; i++)
                {
                    m_updatableItemEffects[i] = updatableItemEffects[i].CreateCopy();
                }
                m_hasUpdateableItemEffects = true;
            }
            m_hasUpdateableItemEffects = false;
        }

        public void StartEffect()
        {
            if (m_isActive == false)
            {
                m_timer = m_duration;
                if (m_hasDurationEffects)
                {
                    for (int i = 0; i < m_durationItemEffects.Length; i++)
                    {
                        m_durationItemEffects[i].StartEffect(player);
                    }
                }
                m_isActive = true;
            }
        }


        public void ResetTimer()
        {
            m_timer = m_duration;
        }

        public void StopEffect()
        {
            if (m_isActive)
            {
                if (m_hasDurationEffects)
                {
                    for (int i = 0; i < m_durationItemEffects.Length; i++)
                    {
                        m_durationItemEffects[i].StopEffect(player);
                    }
                }

                m_isActive = false;
            }
        }

        public void UpdateEffect(float deltaTime)
        {
            m_timer -= deltaTime;
            if (m_hasUpdateableItemEffects && m_timer > 0)
            {
                for (int i = 0; i < m_updatableItemEffects.Length; i++)
                {
                    m_updatableItemEffects[i].Execute(player, deltaTime);
                }
            }
        }
    }

}