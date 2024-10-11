using System.Collections;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems;
using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    [System.Serializable]
    public class PassiveRegeneration : HandledSoulSkillModule
    {
        public enum Stat
        {
            Health,
            Shadow
        }

        [System.Serializable]
        public struct Info
        {
            [SerializeField]
            private Stat m_regenStat;
            [SerializeField]
            private int m_amount;
            [SerializeField]
            private float m_interval;

            public Stat regenStat => m_regenStat;
            public int amount => m_amount;
            public float interval => m_interval;
        }

        [SerializeField, HideLabel]
        private Info m_info;

        public class Handle : BaseHandle
        {
            private Info m_info;

            private Coroutine m_coroutine;
            private bool m_isRegenerating;

            public bool isRegenerating => m_isRegenerating;

            public event EventAction<EventActionArgs> RegenStart;
            public event EventAction<EventActionArgs> RegenEnd;

            public Handle(IPlayer m_reference, Info info) : base(m_reference)
            {
                m_info = info;
                m_isRegenerating = false;
            }

            private IEnumerator HPRegenRoutine(IHealable module, ICappedStatInfo health)
            {
                var timer = m_info.interval;
                do
                {
                    if (GameplaySystem.isGamePaused == false)
                    {
                        if (health.currentValue == health.maxValue)
                        {
                            if (m_isRegenerating)
                            {
                                m_isRegenerating = false;
                                RegenEnd?.Invoke(this, EventActionArgs.Empty);
                            }
                        }
                        else
                        {
                            if (health.currentValue == 0)
                            {
                                if (m_isRegenerating)
                                {
                                    m_isRegenerating = false;
                                    RegenEnd?.Invoke(this, EventActionArgs.Empty);
                                }
                            }
                            else
                            {
                                if (m_isRegenerating == false)
                                {
                                    m_isRegenerating = true;
                                    RegenStart?.Invoke(this, EventActionArgs.Empty);
                                }

                                timer -= GameplaySystem.time.deltaTime;
                                if (timer <= 0)
                                {
                                    GameplaySystem.combatManager.Heal(module, m_info.amount);
                                    timer = m_info.interval;
                                }
                            }
                        }
                    }
                    yield return null;
                } while (true);

            }

            private IEnumerator ShadowRegenRoutine(ICappedStat module)
            {
                var timer = m_info.interval;
                do
                {
                    if (GameplaySystem.isGamePaused == false)
                    {
                        timer -= GameplaySystem.time.deltaTime;
                        if (timer <= 0)
                        {
                            module.AddCurrentValue(m_info.amount);
                            timer = m_info.interval;
                        }
                    }
                    yield return null;
                } while (true);
            }

            public override void Dispose()
            {
                m_player.character.StopCoroutine(m_coroutine);
                switch (m_info.regenStat)
                {
                    case Stat.Health:
                        GameplaySystem.gamplayUIHandle.DeactivateHealthRegenEffect();
                        break;
                    case Stat.Shadow:
                        GameplaySystem.gamplayUIHandle.DeactivateShadowRegenEffect();
                        break;
                }
            }

            public override void Initialize()
            {
                switch (m_info.regenStat)
                {
                    case Stat.Health:
                        GameplaySystem.gamplayUIHandle.ActivateHealthRegenEffect(this);
                        m_coroutine = m_player.character.StartCoroutine(HPRegenRoutine(m_player.healableModule, m_player.damageableModule.health));
                        break;
                    case Stat.Shadow:
                        GameplaySystem.gamplayUIHandle.ActivateShadowRegenEffect();
                        m_coroutine = m_player.character.StartCoroutine(ShadowRegenRoutine(m_player.magic));
                        break;
                }

            }
        }

        protected override BaseHandle CreateHandle(IPlayer player) => new Handle(player, m_info);
    }
}