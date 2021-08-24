using DChild.Gameplay.Characters.Enemies;
using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    public class RayAggroSensor : AggroSensor
    {
        [SerializeField]
        private Transform m_model;

        private IEnemyTarget m_spottedTarget;
        private bool m_targetRegistered;

        private bool RegisterSpottedPlayer(int hitCount)
        {
            var success = hitCount > 0;
            if (success)
            {
                m_brain.SetTarget(m_spottedTarget);
                m_targetRegistered = true;
            }
            return success;
        }

        private void FixedUpdate()
        {       
            if (m_spottedTarget != null && m_targetRegistered == false)
            {
                var rayOrigin = (Vector2)m_model.position;
                var toRayTarget = m_spottedTarget.position - rayOrigin;
                int hitCount = 0;
                Raycaster.SetLayerCollisionMask(DChildUtility.GetEnvironmentMask());
                Raycaster.Cast(rayOrigin, toRayTarget.normalized, toRayTarget.magnitude, true, out hitCount);
                if(RegisterSpottedPlayer(hitCount) == false)
                {
                    var offsetTarget = toRayTarget;
                    offsetTarget.y += 3f;
                    Raycaster.Cast(rayOrigin, offsetTarget.normalized, offsetTarget.magnitude, true, out hitCount);
                    if (RegisterSpottedPlayer(hitCount) == false)
                    {
                        offsetTarget = toRayTarget;
                        offsetTarget.y -= 3f;
                        Raycaster.Cast(rayOrigin, offsetTarget.normalized, offsetTarget.magnitude, true, out hitCount);
                        RegisterSpottedPlayer(hitCount);
                    }
                }
            }
            //else
            //{
            //    m_brain.SetTarget(m_spottedTarget);
            //}
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_spottedTarget == null)
            {             
                var target = collision.GetComponentInParent<IEnemyTarget>();
                if (target != null)
                {
                    m_spottedTarget = target;
                    m_targetRegistered = false;                  
                }
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (m_spottedTarget != null)
            {
                var target = collision.GetComponentInParent<IEnemyTarget>();
                if (m_spottedTarget == target)
                {
                    m_spottedTarget = null;
                    m_targetRegistered = false;
                }
            }
        }
    }
}