using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class MovableObject : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private SerializedVector3 m_position;
            [SerializeField]
            private bool m_canBeMoved;

            public SaveData(Vector3 position, bool canBeMoved)
            {
                m_position = position;
                m_canBeMoved = canBeMoved;
            }

            public Vector3 position => m_position;
            public bool canBeMoved => m_canBeMoved;

            ISaveData ISaveData.ProduceCopy() => new SaveData(position, canBeMoved);
        }

        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private Transform m_source;
        [SerializeField, ReadOnly]
        private bool m_canBeMoved = true;
        [SerializeField]
        private float m_grabbedMoveModifier = 1f;
        [SerializeField, TabGroup("Grabbed"), LabelText("Constraints")]
        private RigidbodyConstraints2D m_onGrabbedConstraints = RigidbodyConstraints2D.FreezeRotation;
        [SerializeField, TabGroup("Let Go"), LabelText("Constraints")]
        private RigidbodyConstraints2D m_onLetGoConstraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        [SerializeField, TabGroup("Grabbed"), LabelText("Callback")]
        private UnityEvent m_onGrabbed;
        [SerializeField, TabGroup("Let Go"), LabelText("Callback")]
        private UnityEvent m_onLetGo;
        [SerializeField]
        private bool m_useTransform;

        private Rigidbody2D m_rigidbody;
        private bool m_isGrabbed = false;
        private bool m_isTouchingPlayer = false;
        public event EventAction<EventActionArgs> BecameUnmovable;

        public Vector3 promptPosition => transform.position + m_promptOffset;
        public bool canBeMoved => m_canBeMoved;
        public float grabbedMoveModifier => m_grabbedMoveModifier;
        public bool isGrabbed => m_isGrabbed;
        public Transform source => m_source;

        public void Load(ISaveData data)
        {
            var saveData = ((SaveData)data);
            m_source.position = saveData.position;
            m_canBeMoved = saveData.canBeMoved;
        }

        public ISaveData Save() => new SaveData(m_source.position, m_canBeMoved);
        public void Initialize()
        {
            m_canBeMoved = true;
        }
        public void SetMovable(bool value)
        {
            m_canBeMoved = value;
            if (value == false)
            {
                m_rigidbody.velocity = Vector2.zero;
                BecameUnmovable?.Invoke(this, EventActionArgs.Empty);
            }
        }

        public void SetGrabState(bool isGrabbed)
        {
            if (isGrabbed)
            {
                m_isGrabbed = true;
                m_onGrabbed?.Invoke();
                m_rigidbody.constraints = m_onGrabbedConstraints;
            }
            else
            {
                StopMovement();
                m_isGrabbed = false;
                m_onLetGo?.Invoke();
                m_rigidbody.constraints = m_onLetGoConstraints;
            }
        }

        public void MoveObject(float direction, float moveForce)
        {
            if (m_canBeMoved == true)
            {
                if (m_useTransform)
                {
                    var currentpos = m_source.position;
                    currentpos.x += direction * moveForce * GameplaySystem.time.deltaTime;
                    m_source.position = currentpos;

                }
                else
                {
                    m_rigidbody.velocity = Vector2.zero;
                    m_rigidbody.velocity += new Vector2(direction * (moveForce * GameplaySystem.time.fixedDeltaTime), m_rigidbody.velocity.y);
                }

                //////
                //GetComponent<TestBox>().Move();
            }
        }

        public void StopMovement()
        {
            m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);

            //////
            //GetComponent<TestBox>().Idle();
        }

        private void Awake()
        {
            m_rigidbody = m_source.GetComponent<Rigidbody2D>();
        }



        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (DChildUtility.IsAnEnvironmentLayerObject(collision.gameObject))
            {
                m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (m_isGrabbed == false)
                {
                    m_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                }
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (DChildUtility.IsAnEnvironmentLayerObject(collision.gameObject))
            {
                if (m_isGrabbed == false && m_isTouchingPlayer == true)
                {
                    m_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                }
                else
                {
                    m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                m_isTouchingPlayer = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (DChildUtility.IsAnEnvironmentLayerObject(collision.gameObject))
            {
                m_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                m_isTouchingPlayer = false;
            }
        }

        private void OnValidate()
        {
            if (m_source == null)
            {
                m_source = transform;
            }
        }

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }
    }
}