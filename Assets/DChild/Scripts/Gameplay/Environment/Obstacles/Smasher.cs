using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Environment.Obstacles
{
    public class Smasher : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D m_modelRigidbody;
        [SerializeField]
        private Collision2DEventSender m_smasherCollisionEvent;
        [SerializeField]
        private Vector2 m_shakeMaxOffset;
        [SerializeField]
        private GameObject m_smashFX;

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

        [SerializeField]
        private Collider2D[] m_collisionExceptions;

        private Transform m_modelTransfrom;
        private Vector2 m_startPosition;
        private WaitForFixedUpdate m_fixedUpdateWait;
        private float m_animationCurveTimer;
        private bool m_isDropping;
        private bool m_isReturning;

        private static FXSpawnHandle<FX> m_fxSpawner = new FXSpawnHandle<FX>();

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
            //if (withDelay)
            //{
            //    StartCoroutine(DelayedRoutine(m_dropDelay, DropRoutine));
            //}
            //else
            //{
            StartCoroutine(DropRoutine());
            //}
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
            yield return WarningShakeRoutine(m_dropDelay);
            m_animationCurveTimer = 0;
            while (true)
            {
                var deltaTime = GameplaySystem.time.deltaTime;
                var target = m_modelTransfrom.position + (m_modelTransfrom.right * 100f);
                var deltaSpeed = m_dropSpeed.Evaluate(m_animationCurveTimer) * m_maxDropSpeed * deltaTime;
                //m_modelRigidbody.MovePosition(Vector2.MoveTowards(m_modelRigidbody.position, target, deltaSpeed));
                m_modelTransfrom.position = Vector2.MoveTowards(m_modelTransfrom.position, target, deltaSpeed);
                yield return null;
            }
        }

        private IEnumerator ReturnRoutine()
        {
            m_animationCurveTimer = 0;
            while (m_modelRigidbody.position != m_startPosition)
            {
                var deltaTime = GameplaySystem.time.deltaTime;
                var target = m_modelTransfrom.position + (m_modelTransfrom.right * 100f);
                var deltaSpeed = m_returnSpeed.Evaluate(m_animationCurveTimer) * m_maxReturnSpeed * deltaTime;
                //m_modelRigidbody.MovePosition(Vector2.MoveTowards(m_modelRigidbody.position, m_startPosition, deltaSpeed));
                m_modelTransfrom.position = Vector2.MoveTowards(m_modelTransfrom.position, m_startPosition, deltaSpeed);
                m_animationCurveTimer += deltaTime;
                yield return null;
            }
            m_isReturning = false;
        }

        private IEnumerator WarningShakeRoutine(float duration)
        {
            var originalLocalPosition = (Vector2)m_modelTransfrom.localPosition;
            var timer = duration;
            do
            {
                var offset = UnityEngine.Random.insideUnitCircle;
                offset.x *= m_shakeMaxOffset.x;
                offset.y *= m_shakeMaxOffset.y;
                m_modelTransfrom.localPosition = originalLocalPosition + offset;
                timer -= GameplaySystem.time.deltaTime;
                yield return null;
            } while (timer > 0);
            m_modelTransfrom.localPosition = originalLocalPosition;
            yield return null;
        }

        private IEnumerator DelayedRoutine(float delay, Func<IEnumerator> NextRoutine)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(NextRoutine());
        }

        private void StopDrop()
        {
            StopAllCoroutines();
            //m_modelRigidbody.velocity = Vector2.zero;
            //m_modelTransfrom.position = m_modelTransfrom.localPosition;
            m_isReturning = false;
            m_isDropping = false;
        }

        private void OnCollision(object sender, CollisionEventActionArgs eventArgs)
        {
            if (m_isDropping && m_isReturning == false)
            {
                var collision = eventArgs.collision;
                if (DChildUtility.IsAnEnvironmentLayerObject(collision.gameObject))
                {
                    if (ShouldIgnoreCollisionWith(collision.collider) == false)
                    {
                        m_fxSpawner.InstantiateFX(m_smashFX, collision.GetContact(0).point);
                        StopDrop();
                        Return(true);
                    }
                }
            }
        }

        private bool ShouldIgnoreCollisionWith(Collider2D collider2D)
        {
            for (int i = 0; i < m_collisionExceptions.Length; i++)
            {
                if (collider2D == m_collisionExceptions[i])
                {
                    return true;
                }
            }
            return false;
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
            m_fixedUpdateWait = null;
        }

#if UNITY_EDITOR
        [SerializeField,BoxGroup("Naming Config")]
        private GameObject m_smasher;
        [SerializeField, BoxGroup("Naming Config")]
        private GameObject m_trigger;
        [SerializeField, BoxGroup("Naming Config")]
        private string m_objectName;
        [SerializeField, BoxGroup("Naming Config"), OnValueChanged("RenameObject"),HideInPrefabAssets]
        private int m_objectIndex;

        private void RenameObject()
        {
            gameObject.name = $"{m_objectName} ({m_objectIndex})";
            m_smasher.name = $"Smasher ({m_objectIndex})";
            m_trigger.name = $"SmasherTrigger ({m_objectIndex})";
            EditorUtility.SetDirty(gameObject);
            EditorUtility.SetDirty(m_smasher);
            EditorUtility.SetDirty(m_trigger);
        }
#endif
    }
}
