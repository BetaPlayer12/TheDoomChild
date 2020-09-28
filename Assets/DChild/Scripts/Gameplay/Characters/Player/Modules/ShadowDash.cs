using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Combat;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ShadowDash : MonoBehaviour, IDash, IComplexCharacterModule
    {
        [SerializeField]
        private Dash m_dash;
        [SerializeField, MinValue(0)]
        private int m_sourceRequiredAmount;
        [SerializeField]
        private ParticleSystem m_tempFX;

        private ICappedStat m_source;
        private Damageable m_damageable;
        private bool m_wasUsed;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_source = info.magic;
            m_damageable = info.damageable;
        }

        public void Cancel()
        {
            m_dash.Cancel();
            GameplaySystem.world.SetShadowColliders(false);
            m_damageable.SetInvulnerability(Invulnerability.MAX);
            m_wasUsed = false;
            m_tempFX?.Stop(true);
        }

        public bool HaveEnoughSourceForExecution() => m_sourceRequiredAmount <= m_source.currentValue;

        public void ConsumeSource() => m_source.ReduceCurrentValue(m_sourceRequiredAmount);

        public void HandleCooldown() => m_dash.HandleCooldown();

        public void ResetCooldownTimer() => m_dash.ResetCooldownTimer();

        public void HandleDurationTimer() => m_dash.HandleDurationTimer();

        public bool IsDashDurationOver() => m_dash.IsDashDurationOver();

        public void ResetDurationTimer() => m_dash.ResetDurationTimer();

        public void Execute()
        {
            if (m_wasUsed == false)
            {
                GameplaySystem.world.SetShadowColliders(true);
                m_damageable.SetInvulnerability(Invulnerability.MAX);
                m_wasUsed = true;
                m_tempFX?.Play(true);
            }
            m_dash.Execute();
        }

        public void Reset()
        {
            m_dash.Reset();
        }
    }
}
