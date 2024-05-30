using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Cinematics.Cameras
{
    public class ReactivePlayerCamera : SerializedMonoBehaviour, IGameplayInitializable
    {
        [SerializeField]
        private bool m_shakeOnDamage;
        [SerializeField, ShowIf("m_shakeOnDamage")]
        private CameraShakeData m_onDamageShakeData;

        [SerializeField]
        private bool m_shakeOnAttackHit;
        [SerializeField, ShowIf("m_shakeOnAttackHit")]
        private CameraShakeData m_onAttackHitShakeData;
        [SerializeField, ShowIf("m_shakeOnAttackHit")]
        private CameraShakeData m_onAttackKillShakeData;

        //[SerializeField, ShowIf("m_shakeOnAttackHit")]
        //private CameraShakeData m_onAttackHitShakeStartData;
        //[SerializeField, ShowIf("m_shakeOnAttackHit")]
        //private CameraShakeData m_onAttackHitShakeLoopData;
        //[SerializeField, ShowIf("m_shakeOnAttackHit")]
        //private float m_onAttackHitShakeLoopResetDuration;
        //[SerializeField, ShowIf("m_shakeOnAttackHit")]
        //private CameraShakeData m_onEnemyKilledShakeLoopData;
        //[SerializeField, MinValue(0f)]
        //private float m_shakePause;

        [SerializeField]
        private ICameraShakeHandle m_cameraShake;

        private ICinema m_cinema;
        private Coroutine m_shakeRoutine;
        private Coroutine m_onAttackShakeResetRoutine;
        private bool m_useOnAttackLoop;

        public void Initialize()
        {
            m_cinema = GameplaySystem.cinema;
        }

        public void HandleOnDamageRecieveShake()
        {
            m_cinema.ExecuteCameraShake(m_onDamageShakeData);
            //if (m_shakeOnDamage)
            //{
            //    StopAllCoroutines();
            //    GameplaySystem.cinema.SetCameraShakeProfile(CameraShakeType.AllDirection);
            //    m_shakeRoutine = StartCoroutine(CameraShakeRoutine(m_onDamageShakeData.cameraShakeInfo));
            //    m_useOnAttackLoop = false;
            //}
        }

        public void HandleOnAttackHit(CombatConclusionEventArgs eventArgs)
        {
            if (m_shakeOnAttackHit)
            {
                if (eventArgs.target.instance.isAlive)
                {
                    m_cinema.ExecuteCameraShake(m_onAttackHitShakeData);
                }
                else
                {
                    m_cinema.ExecuteCameraShake(m_onAttackKillShakeData);
                }
            }

            //if (m_shakeOnAttackHit)
            //{
            //    //StopAllCoroutines();
            //    if (eventArgs.target.instance.isAlive)
            //    {

            //        if (eventArgs.target.isBreakableObject)
            //        {
            //            switch (eventArgs.target.breakableObject.type)
            //            {
            //                case Environment.BreakableObject.Type.Others:
            //                    GameplaySystem.cinema.SetCameraShakeProfile(CameraShakeType.AllDirection);
            //                    break;
            //                case Environment.BreakableObject.Type.Floor:
            //                    GameplaySystem.cinema.SetCameraShakeProfile(CameraShakeType.VerticalOnly);
            //                    break;
            //                case Environment.BreakableObject.Type.Wall:
            //                    GameplaySystem.cinema.SetCameraShakeProfile(CameraShakeType.HorizontalOnly);
            //                    break;
            //            }
            //        }
            //        else
            //        {
            //            GameplaySystem.cinema.SetCameraShakeProfile(CameraShakeType.AllDirection);
            //        }

            //        if (m_useOnAttackLoop)
            //        {
            //            m_shakeRoutine = StartCoroutine(CameraShakeRoutine(m_onAttackHitShakeLoopData.cameraShakeInfo));
            //            StopCoroutine(m_onAttackShakeResetRoutine);
            //            m_onAttackShakeResetRoutine = StartCoroutine(OnAttackCameraShakeRoutine());
            //        }
            //        else
            //        {
            //            m_shakeRoutine = StartCoroutine(CameraShakeRoutine(m_onAttackHitShakeStartData.cameraShakeInfo));
            //            m_onAttackShakeResetRoutine = StartCoroutine(OnAttackCameraShakeRoutine());
            //            m_useOnAttackLoop = true;
            //        }
            //    }
            //    else if(eventArgs.target.isCharacter)
            //    {
            //        StopAllCoroutines();
            //        m_shakeRoutine = StartCoroutine(CameraShakeRoutine(m_onEnemyKilledShakeLoopData.cameraShakeInfo));
            //    }
            //}
        }

        private IEnumerator CameraShakeRoutine(CameraShakeInfo shakeInfo)
        {
            //var timer = 0f;

            //m_cameraShake.SetShakeTo(shakeInfo);
            //m_cinema.EnableCameraShake(true);
            //do
            //{
            //    var deltaTime = GameplaySystem.time.deltaTime;
            //    m_cameraShake.UpdateShake(m_cinema, deltaTime);
            //    timer += deltaTime;
            yield return null;
            //} while (timer <= shakeInfo.duration);

            //m_cinema.EnableCameraShake(false);
            //m_shakeRoutine = null;
        }

        private IEnumerator OnAttackCameraShakeRoutine()
        {
            yield return null;
            //yield return new WaitForSeconds(m_onAttackHitShakeLoopResetDuration);
            //m_useOnAttackLoop = false;
            //m_onAttackShakeResetRoutine = null;
        }
    }
}