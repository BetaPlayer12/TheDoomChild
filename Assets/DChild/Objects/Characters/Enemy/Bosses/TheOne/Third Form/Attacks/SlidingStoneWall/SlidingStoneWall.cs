using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SlidingStoneWall : PoolableObject
    {
        private bool m_wallIsGrounded;
        private bool m_slideWall;

        public bool executeAttack;

        [SerializeField]
        private Collider2D m_wallCollider;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_floorSensor;

        [SerializeField, TabGroup("Damagers")]
        private GameObject m_floorSlamCollider;
        [SerializeField, TabGroup("Damagers")]
        private GameObject m_wallSlamCollider;

        // Start is called before the first frame update
        void Start()
        {
            m_wallIsGrounded = false;
            m_slideWall = false;
            m_floorSlamCollider.SetActive(false);
            m_wallSlamCollider.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (executeAttack)
            {
                if (!m_wallIsGrounded)
                {
                    StartCoroutine(WallSmashOnGround());
                }

                if (m_slideWall)
                {
                    StartCoroutine(SlideWall());
                }
            }         
        }

        private IEnumerator WallSmashOnGround()
        {
            if (!m_floorSensor.isDetecting)
            {
                transform.Translate(Vector3.down);
            }
            else
            {
                m_floorSlamCollider.SetActive(true);

            yield return new WaitForSeconds(2f);

            m_floorSlamCollider.SetActive(false);

            m_wallIsGrounded = true;
            m_slideWall = true;
            }

            
        }

        private IEnumerator SlideWall()
        {
            if (!m_wallSensor.isDetecting)
            {
                transform.Translate(Vector3.right);
            }
            else
            {
                m_wallCollider.enabled = false;
                m_wallSlamCollider.SetActive(true);

                yield return new WaitForSeconds(0.5f);

                m_wallSlamCollider.SetActive(false);
                //Destroy Wall animation here
                DestroyInstance();
            }

            yield return new WaitForSeconds(1f);
        }
    }
}

