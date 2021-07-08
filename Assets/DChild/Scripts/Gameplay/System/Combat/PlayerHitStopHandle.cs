using DChild.Gameplay.Systems;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class PlayerHitStopHandle : MonoBehaviour
    {
        [SerializeField]
        private bool m_executeOnDamage;
        [SerializeField, MinValue(0.1f), ShowIf("m_executeOnDamage")]
        private float m_onDamageHitStopDuration;
        [SerializeField, MinValue(0), ShowIf("m_executeOnDamage")]
        private float m_onDamageHitStopTimeScale;

        [SerializeField]
        private bool m_executeOnAttackHit;
        [SerializeField, MinValue(0.1f), ShowIf("m_executeOnAttackHit")]
        private float m_onAttackHitStopDuration;
        [SerializeField, MinValue(0), ShowIf("m_executeOnAttackHit")]
        private float m_onAttackHitStopTimeScale;

        private CountdownTimer m_duration;

        public void Execute(bool useOnAttackHit)
        {
            if ((useOnAttackHit && m_executeOnAttackHit) || (useOnAttackHit == false && m_executeOnDamage))
            {
                var stopTimeScale = useOnAttackHit ? m_onAttackHitStopTimeScale : m_onDamageHitStopTimeScale;
                GameTime.RegisterValueChange(this, stopTimeScale, GameTime.Factor.Multiplication);
                var countdownDuration = useOnAttackHit ? m_onAttackHitStopDuration : m_onDamageHitStopDuration;
                m_duration.SetStartTime(countdownDuration);
                m_duration.Reset();
                enabled = true;
            }
        }

        private void ResumeTime(object sender, EventActionArgs eventArgs)
        {
            GameTime.UnregisterValueChange(this, GameTime.Factor.Multiplication);
            enabled = false;
        }

        private void Awake()
        {
            m_duration = new CountdownTimer(0);
            m_duration.CountdownEnd += ResumeTime;
            enabled = false;
        }

        private void Update()
        {
            if (GameplaySystem.isGamePaused == false)
            {
                m_duration.Tick(Time.unscaledDeltaTime);
            }
        }

        private void OnDestroy()
        {
            GameTime.UnregisterValueChange(this, GameTime.Factor.Multiplication);
        }
    }
}