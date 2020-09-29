using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Cinematics.Cameras
{
    public class ReactivePlayerCamera : MonoBehaviour, IGameplayInitializable
    {
        [System.Serializable]
        private struct ShakeInfo
        {
            [SerializeField, MinValue(0f)]
            private float m_amplitude;
            [SerializeField, MinValue(0f)]
            private float m_frequency;
            [SerializeField, MinValue(0f)]
            private float m_duration;

            public float amplitude => m_amplitude;
            public float frequency => m_frequency;
            public float duration => m_duration;
        }

        [SerializeField]
        private bool m_shakeOnDamage;
        [SerializeField, ShowIf("m_shakeOnDamage")]
        private ShakeInfo m_onDamageShake;
        [SerializeField]
        private bool m_shakeOnAttackHit;
        [SerializeField, ShowIf("m_shakeOnAttackHit")]
        private ShakeInfo m_onAttackHitShake;
        [SerializeField, MinValue(0f)]
        private float m_shakePause;

        private ICinema m_cinema;
        private Coroutine m_shakeRoutine;

        public void Initialize()
        {
            m_cinema = GameplaySystem.cinema;
            var player = GameplaySystem.playerManager.player;
            player.attackModule.TargetDamaged += OnAttackHit;
            player.damageableModule.DamageTaken += OnDamageTaken;
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_shakeOnDamage)
            {
                StopAllCoroutines();
                m_shakeRoutine = StartCoroutine(CameraShakeRoutine(m_onDamageShake));
            }
        }

        private void OnAttackHit(object sender, CombatConclusionEventArgs eventArgs)
        {
            if (m_shakeOnAttackHit)
            {
                StopAllCoroutines();
                m_shakeRoutine = StartCoroutine(CameraShakeRoutine(m_onAttackHitShake));
            }
        }

        private IEnumerator CameraShakeRoutine(ShakeInfo shakeInfo)
        {
            if (m_shakeRoutine != null)
            {
                m_cinema.EnableCameraShake(false);
                yield return new WaitForSeconds(m_shakePause);
            }

            m_cinema.SetCameraShake(shakeInfo.amplitude, shakeInfo.frequency);
            m_cinema.EnableCameraShake(true);
            yield return new WaitForSeconds(shakeInfo.duration);
            m_cinema.EnableCameraShake(false);
            m_shakeRoutine = null;
        }
    }

}