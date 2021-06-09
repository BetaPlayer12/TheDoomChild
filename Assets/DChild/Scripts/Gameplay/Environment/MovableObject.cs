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
        private Transform m_source;
        [SerializeField]
        private bool m_isHeavy;
        [SerializeField]
        private bool m_canBeMoved;
        [SerializeField, TabGroup("Grabbed")]
        private UnityEvent m_onGrabbed;
        [SerializeField, TabGroup("Let Go")]
        private UnityEvent m_onLetGo;

        private Rigidbody2D m_rigidbody;
        private bool m_isGrabbed = false;
        private bool m_isTouchingPlayer = false;
        public event EventAction<EventActionArgs> BecameUnmovable;

        public bool isHeavy => m_isHeavy;
        public bool canBeMoved => m_canBeMoved;

        public void Load(ISaveData data)
        {
            var saveData = ((SaveData)data);
            m_source.position = saveData.position;
            m_canBeMoved = saveData.canBeMoved;
        }

        public ISaveData Save() => new SaveData(transform.position, m_canBeMoved);

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
                m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                StopMovement();
                m_isGrabbed = false;
                m_onLetGo?.Invoke();
                m_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
        }

        public void MoveObject(float direction, float moveForce)
        {
            if (m_canBeMoved == true)
            {
                m_rigidbody.velocity = new Vector2(direction * moveForce, m_rigidbody.velocity.y);
            }
        }

        public void StopMovement()
        {
            m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
        }

        private void Awake()
        {
            m_rigidbody = m_source.GetComponentInParent<Rigidbody2D>();
        }



        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
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
            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
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
            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
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
    }
}