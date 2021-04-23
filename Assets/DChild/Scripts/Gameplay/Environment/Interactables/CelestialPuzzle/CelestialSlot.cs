using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class CelestialSlot : MonoBehaviour
    {
        [SerializeField]
        private SerializeID m_ID = new SerializeID(true);
        [SerializeField]
        private CelestialCube m_cube;
        [SerializeField]
        private float m_approximation;
        [ShowInInspector, HideInEditorMode, OnValueChanged("CallStateChange")]
        private bool m_isOccupied;

        private float m_proximitymin;
        private float m_cubePosition;
        private float m_proximitymax;

        public event EventAction<EventActionArgs> StateChange;
        public bool isOccupied => m_isOccupied;
        public SerializeID ID => m_ID;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.color = new Color(1, 1, 1, 1);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 5);
        }

        public void SetLockDown(bool lockDown)
        {

        }
#if UNITY_EDITOR
        private void CallStateChange()
        {
            StateChange?.Invoke(this, EventActionArgs.Empty);
        }
#endif

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponentInParent(out CelestialCube cube))
            {
                if (m_cube == cube)
                {
                    m_isOccupied = true;
                    StateChange?.Invoke(this, EventActionArgs.Empty);
                    //Do Something;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponentInParent(out CelestialCube cube))
            {
                if (m_cube == cube)
                {
                    StateChange?.Invoke(this, EventActionArgs.Empty);
                    m_isOccupied = false;
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponentInParent(out CelestialCube cube))
            {
                if (m_cube == cube)
                {
                    m_proximitymin = transform.position.x;
                    m_proximitymin = m_proximitymin - m_approximation;
                    m_proximitymax = transform.position.x;
                    m_proximitymax = m_proximitymax + m_approximation;

                    m_cubePosition = cube.transform.position.x;
                    if (m_cubePosition >= m_proximitymin && m_cubePosition <= m_proximitymax)
                    {
                        m_isOccupied = true;

                    }
                    else
                    {
                        m_isOccupied = false;
                    }

                }
            }
        }
    }
}
