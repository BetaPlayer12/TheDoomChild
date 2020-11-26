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
        private class ShakeInfo
        {
            [SerializeField]
            private AnimationCurve m_amplitude;
            [SerializeField]
            private AnimationCurve m_frequency;
            [SerializeField, MinValue(0f)]
            private float m_duration;

            public AnimationCurve amplitude => m_amplitude;
            public AnimationCurve frequency => m_frequency;
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
                GameplaySystem.cinema.SetCameraShakeProfile(Cinema.ShakeType.AllDirection);
                m_shakeRoutine = StartCoroutine(CameraShakeRoutine(m_onDamageShake));
            }
        }

        private void OnAttackHit(object sender, CombatConclusionEventArgs eventArgs)
        {
            if (m_shakeOnAttackHit)
            {
                StopAllCoroutines();
                if (eventArgs.target.isBreakableObject)
                {
                    switch (eventArgs.target.breakableObject.type)
                    {
                        case Environment.BreakableObject.Type.Others:
                            GameplaySystem.cinema.SetCameraShakeProfile(Cinema.ShakeType.AllDirection);
                            break;
                        case Environment.BreakableObject.Type.Floor:
                            GameplaySystem.cinema.SetCameraShakeProfile(Cinema.ShakeType.VerticalOnly);
                            break;
                        case Environment.BreakableObject.Type.Wall:
                            GameplaySystem.cinema.SetCameraShakeProfile(Cinema.ShakeType.HorizontalOnly);
                            break;
                    }
                }
                else
                {
                    GameplaySystem.cinema.SetCameraShakeProfile(Cinema.ShakeType.AllDirection);
                }
                m_shakeRoutine = StartCoroutine(CameraShakeRoutine(m_onAttackHitShake));
            }
        }

        private IEnumerator CameraShakeRoutine(ShakeInfo shakeInfo)
        {
            var timer = 0f;

            if (m_shakeRoutine != null)
            {
                m_cinema.EnableCameraShake(false);
                yield return new WaitForSeconds(m_shakePause);
            }

            m_cinema.EnableCameraShake(true);
            do
            {
                m_cinema.SetCameraShake(shakeInfo.amplitude.Evaluate(timer), shakeInfo.frequency.Evaluate(timer));
                timer += GameplaySystem.time.deltaTime;
                yield return null;
            } while (timer <= shakeInfo.duration);

            m_cinema.EnableCameraShake(false);
            m_shakeRoutine = null;
        }
    }

}