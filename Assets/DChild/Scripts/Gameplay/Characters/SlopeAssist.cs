using Holysoft.Collections;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public class SlopeAssist : MonoBehaviour
    {
        [SerializeField]
        private RaySensor m_slopeSensor;

        [SerializeField]
        private CharacterPhysics2D m_characterPhysics;
        //private CalculateSlope m_calculate;
        private float m_defaultGravity;

        private void Start()
        {
            //m_characterPhysics = player.physics;
           // m_calculate = m_characterPhysics.GetComponentInChildren<CalculateSlope>();
            //m_defaultGravity = m_characterPhysics.gravity.gravityScale;
        }

        public void FixedUpdate()
        {
            /*
            if (m_characterPhysics.onWalkableGround)
            {
                if (m_slopeSensor.isDetecting)
                {
                    var walkableAngle = m_characterPhysics.acceptableAngle.max;
                   var groundAngle = m_characterPhysics.groundAngle;
                   m_characterPhysics.gravity.gravityScale = m_calculate.CalculateGravity(walkableAngle, groundAngle, m_defaultGravity);
                }
                else
                {
                    m_characterPhysics.gravity.gravityScale = m_defaultGravity;
                }
            }
            */
        }
        

#if UNITY_EDITOR
        public void Initialize(RaySensor slopeSensor)
        {
            m_slopeSensor = slopeSensor;
        }
#endif
    } 
}
 