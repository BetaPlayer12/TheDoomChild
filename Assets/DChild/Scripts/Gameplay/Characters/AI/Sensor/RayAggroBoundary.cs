using DChild.Gameplay;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    public class RayAggroBoundary : AggroBoundary
    {
        [SerializeField]
        private Transform m_model;

        private Collider2D m_spottedTarget;
        private ITarget m_targetComponent;
        private bool m_targetRegistered;

        private bool RegisterSpottedPlayer(int hitCount)
        {
            var success = hitCount == 0;
            if (success)
            {
                SetTargetToBrain(m_spottedTarget, m_targetComponent);
                m_targetRegistered = true;
            }
            return success;
        }

        private void FixedUpdate()
        {
            if (m_spottedTarget != null && m_targetRegistered == false)
            {
                var rayOrigin = (Vector2)m_model.position;
                var toRayTarget = m_targetComponent.position - rayOrigin;
                int hitCount = 0;
                Raycaster.SetLayerMask(DChildUtility.GetEnvironmentMask());
                Raycaster.Cast(rayOrigin, toRayTarget.normalized, toRayTarget.magnitude, true, out hitCount);
                if (RegisterSpottedPlayer(hitCount) == false)
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
            //    SetTargetToBrain();
            //}
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Sensor" && collision.tag == "Hitbox")
            {
                if (m_spottedTarget == null)
                {
                    var target = collision.GetComponentInParent<ITarget>();
                    if (target != null)
                    {
                        m_spottedTarget = collision;
                        m_targetComponent = target;
                        m_targetRegistered = false;
                    }
                }
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (m_spottedTarget != null)
            {
                if (m_spottedTarget == collision)
                {
                    m_spottedTarget = null;
                    m_targetComponent = null;
                    m_targetRegistered = false;
                    m_brain.SetTarget(null);
                }
            }
        }
    }
}