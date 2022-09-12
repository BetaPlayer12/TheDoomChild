using DChild.Gameplay.Physics;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections;

namespace DChild.Gameplay
{
    [System.Serializable]
    public class StepClimber
    {
        [SerializeField, MinValue(0.01f)]
        private float m_maxStepHeight;
        [SerializeField, MinValue(0.01f)]
        private float stepSearchOvershoot = 0.01f;
        [SerializeField]
        private CollisionDetector m_detector;

        private RaycastHit2D[] m_hitbuffers;
        private ContactPoint2D m_groundCP;
        private Vector2 m_stepUpOffset;
        private Vector2 m_lastVelocity;

        public void Execute(Rigidbody2D rigidbody2D)
        {
            Vector2 velocity = rigidbody2D.velocity;
            //Filter through the ContactPoints to see if we're grounded and to see if we can step up
            //Steps
            if (FindGround(out m_groundCP))
            {
                if (FindStep(out m_stepUpOffset, m_groundCP, velocity))
                {
                    rigidbody2D.position += m_stepUpOffset;
                    rigidbody2D.velocity = m_lastVelocity;
                }
            }

            m_detector?.ClearContactPoints();
            m_lastVelocity = velocity;
        }

        public void Reset()
        {
            m_detector.ClearContactPoints();
        }

        public void Initialize()
        {
            m_hitbuffers = new RaycastHit2D[16];
        }

        public bool FindGround(out ContactPoint2D groundCP)
        {
            groundCP = default(ContactPoint2D);
            bool found = false;

            for (int i = 0; i < (m_detector?.contactPointCount ?? 0); i++)
            {
                var cp = m_detector.GetContactPoint(i);
                if (cp.normal.y > 0.0001f && (found == false || cp.normal.y > groundCP.normal.y))
                {
                    groundCP = cp;
                    found = true;
                    break;
                }
            }
            return found;
        }

        private bool FindStep(out Vector2 stepUpOffset, ContactPoint2D groundCP, Vector3 currVelocity)
        {
            stepUpOffset = default(Vector3);

            //No chance to step if the player is not moving
            Vector2 velocityXZ = new Vector2(currVelocity.x, currVelocity.z);
            if (currVelocity.sqrMagnitude < 0.0001f)
                return false;

            for (int i = 0; i < m_detector.contactPointCount; i++)
            {
                var cp = m_detector.GetContactPoint(i);
                bool test = ResolveStepUp(out stepUpOffset, cp, groundCP);
                if (test)
                    return test;
            }
            return false;
        }

        private bool ResolveStepUp(out Vector2 stepUpOffset, ContactPoint2D stepTestCP, ContactPoint2D groundCP)
        {
            stepUpOffset = default(Vector2);

            //( 1 ) Check if the contact point normal matches that of a step (y close to 0)
            if (Mathf.Abs(stepTestCP.normal.y) >= 0.03f)
            {
                return false;
            }

            var stepPoint = stepTestCP.point.y - groundCP.point.y;
            //( 2 ) Make sure the contact point is low enough to be a step
            if (!(stepPoint < m_maxStepHeight))
            {
                return false;
            }

            //( 3 ) Check to see if there's actually a place to step in front of us
            //Fires one Raycast

            float stepHeight = groundCP.point.y + m_maxStepHeight + 0.0001f;
            Vector2 stepTestInvDir = new Vector2(-stepTestCP.normal.x, 0).normalized;
            Vector2 origin = new Vector2(stepTestCP.point.x, stepHeight) + (stepTestInvDir * stepSearchOvershoot);

            Raycaster.SetLayerMask(m_detector.collisionMask);
            m_hitbuffers = Raycaster.Cast(origin, Vector2.down, true, out int hitcount);
            RaycastHit2D hitInfo = new RaycastHit2D();
            bool isValid = false;
            for (int i = 0; i < hitcount; i++)
            {
                if (m_hitbuffers[i].collider == stepTestCP.collider)
                {
                    isValid = true;
                    hitInfo = m_hitbuffers[i];
                    break;
                }
            }

            if (isValid)
            {
                //We have enough info to calculate the points
                Vector2 stepUpPoint = new Vector2(stepTestCP.point.x, hitInfo.point.y + 0.0001f) + (stepTestInvDir * stepSearchOvershoot);
                Vector2 stepUpPointOffset = stepUpPoint - new Vector2(stepTestCP.point.x, groundCP.point.y);

                //We passed all the checks! Calculate and return the point!
                stepUpOffset = stepUpPointOffset;
            }
            return isValid;
        }

#if UNITY_EDITOR
        public void DrawGizmos(Transform transform)
        {
            var maxHeight = transform.position + (transform.up * m_maxStepHeight);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, maxHeight);
            Gizmos.DrawLine(maxHeight, maxHeight + (transform.right * 2.5f));
            Gizmos.DrawLine(maxHeight, maxHeight + (-transform.right * 2.5f));
        }
#endif
    }
}
