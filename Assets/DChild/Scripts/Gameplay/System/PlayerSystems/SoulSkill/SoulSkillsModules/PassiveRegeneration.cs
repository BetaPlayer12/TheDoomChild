using System.Collections;
using DChild.Gameplay.Combat;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    [System.Serializable]
    public class PassiveRegeneration : HandledSoulSkillModule
    {
        private enum Stat
        {
            Health,
            Shadow
        }

        [System.Serializable]
        private struct Info
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

        private class Handle : BaseHandle
        {
            private Info m_info;

            private Coroutine m_coroutine;

            public Handle(IPlayer m_reference, Info info) : base(m_reference)
            {
                m_info = info;
            }

            private IEnumerator HPRegenRoutine(IHealable module)
            {
                var timer = m_info.interval;
                do
                {
                    if (GameplaySystem.isGamePaused == false)
                    {
                        timer -= GameplaySystem.time.deltaTime;
                        if (timer <= 0)
                        {
                            GameplaySystem.combatManager.Heal(module, m_info.amount);
                            timer = m_info.interval;
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
            }

            public override void Initialize()
            {
                switch (m_info.regenStat)
                {
                    case Stat.Health:
                        m_coroutine = m_player.character.StartCoroutine(HPRegenRoutine(m_player.healableModule));
                        break;
                    case Stat.Shadow:
                        m_coroutine = m_player.character.StartCoroutine(ShadowRegenRoutine(m_player.magic));
                        break;
                }

            }
        }

        protected override BaseHandle CreateHandle(IPlayer player) => new Handle(player, m_info);
    }
}