using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TentacleBlast : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_tentacleEntity;
        [SerializeField]
        private GameObject m_tentacleBlastLaser;
        [SerializeField]
        private Transform m_tentacleShootPosition;
        [SerializeField]
        private float m_tentacleMoveSpeed;

        [SerializeField]
        private Quaternion m_startRotation;
        [SerializeField]
        private Quaternion m_endRotation;
        [SerializeField]
        private Quaternion m_addRotation;

        private Vector2 m_tentacleOriginalPosition;

        public bool m_emergeTentacle;
        public bool m_isRightTentacle;
        private bool m_rotateTentacle;
        private bool m_returnTentacle;

        private IEnumerator EmergeTentacle()
        {
            transform.position = Vector2.MoveTowards(transform.position, m_tentacleShootPosition.position, m_tentacleMoveSpeed);

            if (transform.position == m_tentacleShootPosition.position)
            {
                transform.rotation = m_startRotation;
                m_emergeTentacle = false;
                m_rotateTentacle = true;
            }

            yield return new WaitForSeconds(1f);
        }

        private IEnumerator RotateRightTentacle()
        {
            if (!m_tentacleBlastLaser.activeSelf)
                m_tentacleBlastLaser.SetActive(true);

            if (transform.rotation.eulerAngles.z > 1)
            {
                transform.Rotate(0f, 0f, -0.1f - m_tentacleMoveSpeed * GameplaySystem.time.deltaTime);
                //transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, transform.rotation.z - 1f * GameplaySystem.time.deltaTime));
                Debug.Log(transform.rotation.eulerAngles.z);
                yield return new WaitForSeconds(1f);
                if(transform.rotation.eulerAngles.z < 1)
                {
                    m_returnTentacle = true;
                }              
            }

            if (m_returnTentacle)
            {
                m_tentacleBlastLaser.SetActive(false);
                m_rotateTentacle = false;                
            }

            yield return new WaitForSeconds(1f);
        }

        private IEnumerator RotateTentacle()
        {
            if (!m_tentacleBlastLaser.activeSelf)
                m_tentacleBlastLaser.SetActive(true);

            if (transform.rotation.eulerAngles.z > 1)
            {
                if (m_isRightTentacle)
                {
                    transform.Rotate(0f, 0f, -0.1f - m_tentacleMoveSpeed * GameplaySystem.time.deltaTime);
                }
                else
                {
                    transform.Rotate(0f, 0f, 1f - m_tentacleMoveSpeed * GameplaySystem.time.deltaTime);
                }
                
                //transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, transform.rotation.z - 1f * GameplaySystem.time.deltaTime));
                Debug.Log(transform.rotation.eulerAngles.z);
                yield return new WaitForSeconds(1f);
                if (transform.rotation.eulerAngles.z < 1)
                {
                    m_returnTentacle = true;
                }
            }

            if (m_returnTentacle)
            {
                m_tentacleBlastLaser.SetActive(false);
                m_rotateTentacle = false;
            }

            yield return new WaitForSeconds(1f);
        }

        private IEnumerator ReturnRightTentacleToOriginalPosition()
        {
            transform.position = Vector2.MoveTowards(transform.position, m_tentacleOriginalPosition, m_tentacleMoveSpeed);

            yield return null;
        }


        // Start is called before the first frame update
        void Start()
        {
            m_tentacleOriginalPosition = m_tentacleEntity.transform.position;
            m_tentacleBlastLaser.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (m_emergeTentacle)
                StartCoroutine(EmergeTentacle());

            //if(m_isRightTentacle)
            //    if (m_rotateTentacle)
            //        StartCoroutine(RotateRightTentacle());
            //else
            //    if (m_rotateTentacle)
            //        StartCoroutine(RotateLeftTentacle());

            if (m_rotateTentacle)
                StartCoroutine(RotateTentacle());

            if (m_returnTentacle)
                StartCoroutine(ReturnRightTentacleToOriginalPosition());
        }
    }
}

