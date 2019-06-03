using DChild;
using DChild.Gameplay.Environment;

using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Systems.WorldComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Characters.Enemies.Collections
{
    public class GrassSensor : MonoBehaviour
    {
        [SerializeField]
        private RaySensor m_grassSensor;

        [SerializeField]
        private float m_detectorDistance;

        private GrassMonster m_minion;

        private Vector2 dir;

        private RaycastHit2D[] hit;

        public bool Walkable()
        {
            m_grassSensor.Cast();

            var dir = (m_minion.currentFacingDirection == HorizontalDirection.Left ? 180f : 0f);
            m_grassSensor.SetRotation(dir);
            
            if (m_grassSensor.isDetecting)
            {
                var foliageGrass = m_grassSensor.GetProminentHitCollider().transform.gameObject.GetComponentInChildren<IFoliage>();          
                return (foliageGrass != null) ? true : false;
            }
            else
            {
                return false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Debug.DrawRay(transform.position, -dir * m_detectorDistance);
        }

        private void Awake()
        {
            m_minion = GetComponentInParent<GrassMonster>();
            m_grassSensor = GetComponent<RaySensor>();
        }
    }  
}