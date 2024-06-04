using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MouthBlastIAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private Transform m_mouthBlastOneLeftSide;
        public Transform mouthBlastOneLeftSide => m_mouthBlastOneLeftSide;
        [SerializeField]
        private Transform m_mouthBlastOneRightSide;
        public Transform mouthBlastOneRightSide => m_mouthBlastOneRightSide;
        [SerializeField]
        private float m_mouthBlastOneMoveSpeed;
        public float mouthBlastOneMoveSpeed => m_mouthBlastOneMoveSpeed;
        [SerializeField]
        private Vector2 m_mouthBlastOneOriginalPosition;
        public Vector2 mouthBlastOneOriginalPosition => m_mouthBlastOneOriginalPosition;
        [SerializeField]
        private BlackBloodFlood m_blackBloodFlood;
        [SerializeField, TabGroup("References")]
        private GameObject MouthBlastHolder;
        [SerializeField, BoxGroup("Laser")]
        private LaserLauncher m_launcher;

        public event EventAction<EventActionArgs> AttackStart;
        public event EventAction<EventActionArgs> AttackDone;

        public IEnumerator ExecuteAttack()
        {
            AttackStart?.Invoke(this, EventActionArgs.Empty);

            yield return ChargeBeam();

            AttackDone?.Invoke(this, EventActionArgs.Empty);
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(AITargetInfo Target)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerator ChargeBeam()
        {
            m_launcher.SetBeam(true);
            m_launcher.SetAim(false);
            yield return ShootBlast();
        }

        private IEnumerator ShootBlast()
        {
            MouthBlastHolder.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            m_launcher.TurnLazer(true);
            StartCoroutine(m_launcher.LazerBeamRoutine());
            m_launcher.PlayAnimation("CielingBlastAnticipation");
            yield return new WaitForSeconds(0.5f);
            m_blackBloodFlood.StartFlooding();
            yield return new WaitForSeconds(10f);
            yield return EndMouthBlast();
        }
        public void ShootLazer()
        {
            m_launcher.PlayAnimation("CielingBlast", "CielingBlastAnticipation");
        }
        public IEnumerator EndMouthBlast()
        {
            m_launcher.SetBeam(false);
            m_launcher.PlayAnimation("TentacleBlastDissipation", "CielingBlast");
            m_launcher.TurnLazer(false);
            MouthBlastHolder.SetActive(false);
            //m_launcher.TurnOffDamageCollider();
            yield return null;
        }

        [Button]
        private void TestLaser()
        {
            StartCoroutine(ChargeBeam());
        }
    }

}
