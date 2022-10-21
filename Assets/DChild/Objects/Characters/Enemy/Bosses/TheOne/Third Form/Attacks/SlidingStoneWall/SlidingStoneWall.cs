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

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_floorSensor;

        // Start is called before the first frame update
        void Start()
        {
            m_wallIsGrounded = false;
            m_slideWall = false;
        }

        // Update is called once per frame
        void Update()
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

        private IEnumerator WallSmashOnGround()
        {
            if (!m_floorSensor.isDetecting)
            {
                transform.Translate(Vector3.down);
            }

            yield return new WaitForSeconds(2f);

            m_wallIsGrounded = true;
            m_slideWall = true;
        }

        private IEnumerator SlideWall()
        {
            if (!m_wallSensor.isDetecting)
            {
                transform.Translate(Vector3.right);
            }
            else
            {
                //Destroy Wall animation here
                DestroyInstance();
            }

            yield return new WaitForSeconds(1f);
        }
    }
}

