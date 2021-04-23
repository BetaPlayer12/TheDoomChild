using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    public class DurationItemHandle
    {
        private Character m_character;
        private IDurationItemEffect[] m_durationItemEffects;
        private IUpdatableItemEffect[] m_updatableItemEffects;
        private bool m_hasDurationEffects;
        private bool m_hasUpdateableItemEffects;
        private float m_duration;
        private float m_timer;
        private Coroutine m_activeRoutine;

        public IPlayer player { get; }

        public event EventAction<EventActionArgs> EffectEnd;

        public DurationItemHandle(IPlayer player, float duration, IDurationItemEffect[] durationItemEffects, IUpdatableItemEffect[] updatableItemEffects)
        {
            this.player = player;
            m_character = player.character;
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

        public void Start()
        {
            if (m_activeRoutine == null)
            {
                m_timer = m_duration;
                m_activeRoutine = m_character.StartCoroutine(DurationRoutine());
                player.damageableModule.Destroyed += OnDeath;
            }
        }


        public void ResetTimer()
        {
            m_timer = m_duration;
        }

        private void OnDeath(object sender, EventActionArgs eventArgs)
        {
            if(player.damageableModule.isAlive == false)
            {
                m_character.StopCoroutine(m_activeRoutine);
                if (m_hasDurationEffects)
                {
                    for (int i = 0; i < m_durationItemEffects.Length; i++)
                    {
                        m_durationItemEffects[i].StopEffect(player);
                    }
                }
                player.damageableModule.Destroyed -= OnDeath;
                EffectEnd?.Invoke(this, EventActionArgs.Empty);
            }
        }

        private IEnumerator DurationRoutine()
        {
            if (m_hasDurationEffects)
            {
                for (int i = 0; i < m_durationItemEffects.Length; i++)
                {
                    m_durationItemEffects[i].StartEffect(player);
                }
            }

            if (m_hasUpdateableItemEffects)
            {
                while (m_timer > 0)
                {
                    var deltaTime = GameplaySystem.time.deltaTime;
                    for (int i = 0; i < m_updatableItemEffects.Length; i++)
                    {
                        m_updatableItemEffects[i].Execute(player, deltaTime);
                    }
                    m_timer -= deltaTime;
                    yield return null;
                }
            }
            else
            {
                while (m_timer > 0)
                {
                    m_timer -= GameplaySystem.time.deltaTime;
                    yield return null;
                }
            }

            if (m_hasDurationEffects)
            {
                for (int i = 0; i < m_durationItemEffects.Length; i++)
                {
                    m_durationItemEffects[i].StopEffect(player);
                }
            }

            m_activeRoutine = null;
            EffectEnd?.Invoke(this, EventActionArgs.Empty);
        }
    }
}
