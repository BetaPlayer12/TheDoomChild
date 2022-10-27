using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MouthBlastIIAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private GameObject m_mouthBlastEntity;
        [SerializeField]
        private GameObject m_mouthBlastLaser;
        [SerializeField]
        private Transform m_wallPosition;
        [SerializeField]
        private Vector2 m_originalEntityPosition;

        //Temporary - to be replaced with animation and waitforanimationEnd
        [SerializeField]
        private float m_mouthEntityEmergeTime;
        [SerializeField]
        private float m_anticipationTime;
        [SerializeField]
        private float m_blastDurationTime;

        [SerializeField]
        private float m_emergeSpeed;
        
        public IEnumerator ExecuteAttack()
        {
            //Emerge
            m_mouthBlastEntity.transform.position = m_wallPosition.transform.position;
            yield return new WaitForSeconds(m_mouthEntityEmergeTime);
            //Anticipation
            yield return new WaitForSeconds(m_anticipationTime);
            //Attack
            m_mouthBlastLaser.SetActive(true);
            yield return new WaitForSeconds(m_blastDurationTime);
            m_mouthBlastLaser.SetActive(false);
            m_mouthBlastEntity.transform.position = m_originalEntityPosition;
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        private void Start()
        {
            m_mouthBlastLaser.SetActive(false);
            m_originalEntityPosition = m_mouthBlastEntity.transform.position;
        }
    }
}

