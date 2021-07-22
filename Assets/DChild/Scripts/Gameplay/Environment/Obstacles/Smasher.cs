using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class Smasher : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D m_modelRigidbody;
        [SerializeField]
        private Collision2DEventSender m_smasherCollisionEvent;

        [SerializeField, TabGroup("Drop Config")]
        private float m_maxDropSpeed;
        [SerializeField, TabGroup("Drop Config")]
        private AnimationCurve m_dropSpeed;
        [SerializeField, MinValue(0), TabGroup("Drop Config")]
        private float m_dropDelay;

        [SerializeField, TabGroup("Return Config")]
        private float m_maxReturnSpeed;
        [SerializeField, TabGroup("Return Config")]
        private AnimationCurve m_returnSpeed;
        [SerializeField, MinValue(0), TabGroup("Return Config")]
        private float m_returnDelay;

        private Transform m_modelTransfrom;
        private Vector2 m_startPosition;
        private WaitForFixedUpdate m_fixedUpdateWait;
        private float m_animationCurveTimer;
        private bool m_isDropping;
        private bool m_isReturning;

        [Button, HideInEditorMode]
        public void ExecuteSmartDrop()
        {
            if (m_isDropping == false && m_isReturning == false)
            {
                Drop(true);
            }
        }

        public void Drop(bool withDelay)
        {
            StopAllCoroutines();
            m_isDropping = true;
            m_isReturning = false;
            if (withDelay)
            {
                StartCoroutine(DelayedRoutine(m_dropDelay, DropRoutine));
            }
            else
            {
                StartCoroutine(DropRoutine());
            }
        }

        public void Return(bool withDelay)
        {
            StopAllCoroutines();
            m_isDropping = false;
            m_isReturning = true;
            if (withDelay)
            {
                StartCoroutine(DelayedRoutine(m_returnDelay, ReturnRoutine));
            }
            else
            {
                StartCoroutine(ReturnRoutine());
            }
        }

        private IEnumerator DropRoutine()
        {
            m_animationCurveTimer = 0;
            while (true)
            {
                var deltaTime = GameplaySystem.time.fixedDeltaTime;
                var target = m_modelTransfrom.position + (m_modelTransfrom.right * 100f);
                var deltaSpeed = m_dropSpeed.Evaluate(m_animationCurveTimer) * m_maxDropSpeed * deltaTime;
                m_modelRigidbody.MovePosition(Vector2.MoveTowards(m_modelRigidbody.position, target, deltaSpeed));
                m_animationCurveTimer += deltaTime;
                yield return m_fixedUpdateWait;
            }
        }

        private IEnumerator ReturnRoutine()
        {
            m_animationCurveTimer = 0;
            while (m_modelRigidbody.position != m_startPosition)
            {
                var deltaTime = GameplaySystem.time.fixedDeltaTime;
                var target = m_modelTransfrom.position + (m_modelTransfrom.right * 100f);
                var deltaSpeed = m_returnSpeed.Evaluate(m_animationCurveTimer) * m_maxReturnSpeed * deltaTime;
                m_modelRigidbody.MovePosition(Vector2.MoveTowards(m_modelRigidbody.position, m_startPosition, deltaSpeed));
                m_animationCurveTimer += deltaTime;
                yield return m_fixedUpdateWait;
            }
            m_isReturning = false;
        }

        private IEnumerator DelayedRoutine(float delay, Func<IEnumerator> NextRoutine)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(NextRoutine());
        }

        private void StopDrop()
        {
            StopAllCoroutines();
            m_modelRigidbody.velocity = Vector2.zero;
            m_isReturning = false;
            m_isDropping = false;
        }

        private void OnCollision(object sender, CollisionEventActionArgs eventArgs)
        {
            if (m_isDropping && m_isReturning == false)
            {
                if (eventArgs.collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
                {
                    StopDrop();
                    Return(true);
                }
            }
        }


        public void Reset()
        {
            StopDrop();
            if (m_modelTransfrom != null)
            {
                m_modelTransfrom.position = m_startPosition;
            }
        }

        private void Awake()
        {
            m_modelTransfrom = m_modelRigidbody.transform;
            m_startPosition = m_modelTransfrom.position;
            m_smasherCollisionEvent.OnEnter += OnCollision;
            m_fixedUpdateWait = new WaitForFixedUpdate();
        }
    }
}
