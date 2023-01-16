using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
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
        [SerializeField]
        private GameObject m_mouthOneBlastLaser;

        [SerializeField, BoxGroup("Laser")]
        private LaserLauncher m_launcher;

        public IEnumerator ExecuteAttack()
        {
            throw new System.NotImplementedException();
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
            yield return new WaitForSeconds(3f);
            yield return ShootBlast();
        }

        private IEnumerator ShootBlast()
        {
            StartCoroutine(m_launcher.LazerBeamRoutine());
            m_blackBloodFlood.isFlooding = true;
            yield return null;
        }

        private IEnumerator EndMouthBlast()
        {
            m_blackBloodFlood.isFlooding = false;
            m_launcher.SetBeam(false);
            yield return null;
        }

        private IEnumerator SetMouthBlastPosition()
        {
            int side = Random.Range(0, 2);
            if(side == 0)
            {
                m_mouthOneBlastLaser.transform.position = m_mouthBlastOneLeftSide.position;
            }
            else if(side == 1)
            {
                m_mouthOneBlastLaser.transform.position = m_mouthBlastOneRightSide.position;
            }
            yield return MoveMouthBlastBeam(side);
        }

        private IEnumerator MoveMouthBlastBeam(int side)
        {
            StartCoroutine(ShootBlast());
            bool attackDone = false;
            if (side == 0)
            {
                while (!attackDone)
                {
                    m_mouthOneBlastLaser.transform.position = Vector2.MoveTowards(transform.position, m_mouthBlastOneRightSide.position, m_mouthBlastOneMoveSpeed);

                    if(m_mouthOneBlastLaser.transform.position.x > m_mouthBlastOneRightSide.position.x)
                    {
                        attackDone = true;
                        yield return EndMouthBlast();
                    }
                }
            }
            else if (side == 1)
            {
                while (!attackDone)
                {
                    m_mouthOneBlastLaser.transform.position = Vector2.MoveTowards(transform.position, m_mouthBlastOneLeftSide.position, m_mouthBlastOneMoveSpeed);

                    if (m_mouthOneBlastLaser.transform.position.x < m_mouthBlastOneLeftSide.position.x)
                    {
                        attackDone = true;
                        yield return EndMouthBlast();
                    }
                }
            }
        }

        private IEnumerator FullSequence()
        {
            yield return ChargeBeam();
            yield return SetMouthBlastPosition();
        }

        [Button]
        private void TestMouthBlastOneAttack()
        {
            StartCoroutine(FullSequence());

        }
    }

}
