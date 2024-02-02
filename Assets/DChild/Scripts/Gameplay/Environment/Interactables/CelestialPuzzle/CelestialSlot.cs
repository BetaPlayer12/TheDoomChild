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
        private bool m_useApproximation;
        [SerializeField]
        private float m_approximation;
        [ShowInInspector, HideInEditorMode, OnValueChanged("CallStateChange")]
        private bool m_isOccupied;

        private float m_proximitymin;
        private float m_cubePosition;
        private float m_proximitymax;
        private Collider2D m_triggerCollider;

        public event EventAction<EventActionArgs> StateChange;
        public bool isOccupied => m_isOccupied;
        public SerializeID ID => m_ID;

        private Vector3 triggerPosition
        {
            get
            {
                if (m_triggerCollider == null)
                {
                    m_triggerCollider = GetComponent<Collider2D>();
                }

                var position = m_triggerCollider.bounds.center;
                position.x += m_triggerCollider.offset.x;
                return position;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.color = new Color(1, 1, 1, 1);
            // Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 5);

            if (m_useApproximation == false)
                return;

            Gizmos.color = new Color(1, 1, 1, 1);
            m_proximitymin = triggerPosition.x;
            m_proximitymin -= m_approximation;
            var startLine = triggerPosition;
            startLine.x = m_proximitymin;

            m_proximitymax = triggerPosition.x;
            m_proximitymax += m_approximation;
            var endLine = triggerPosition;
            endLine.x = m_proximitymax;
            Gizmos.DrawLine(startLine, endLine);
            Debug.Log($"{startLine.x}~{endLine.x}");
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
        private void Awake()
        {
            m_triggerCollider = GetComponent<Collider2D>();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponentInParent(out CelestialCube cube))
            {
                if (m_cube == cube && m_isOccupied == false)
                {
                    if (m_useApproximation)
                    {
                        EvaluateCubeProximity(cube);
                    }
                    else
                    {
                        m_isOccupied = true;
                        StateChange?.Invoke(this, EventActionArgs.Empty);
                        //Do Something;
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponentInParent(out CelestialCube cube))
            {
                if (m_cube == cube && m_isOccupied == true)
                {
                    m_isOccupied = false;
                    StateChange?.Invoke(this, EventActionArgs.Empty);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (m_useApproximation == false)
                return;

            if (collision.gameObject.TryGetComponentInParent(out CelestialCube cube))
            {
                if (m_cube == cube)
                {
                    EvaluateCubeProximity(cube);
                }
            }
        }

        private void EvaluateCubeProximity(CelestialCube cube)
        {
            m_proximitymin = triggerPosition.x;
            m_proximitymin = m_proximitymin - m_approximation;
            m_proximitymax = triggerPosition.x;
            m_proximitymax = m_proximitymax + m_approximation;

            m_cubePosition = cube.transform.position.x;
            if (m_cubePosition >= m_proximitymin && m_cubePosition <= m_proximitymax)
            {
                if (m_isOccupied == false)
                {
                    Debug.Log($"Celestial Slot Success {m_cubePosition} vs {m_proximitymin}~ {m_proximitymax}");
                    m_isOccupied = true;
                    StateChange?.Invoke(this, EventActionArgs.Empty);
                }

            }
            else
            {
                if (m_isOccupied == true)
                {
                    m_isOccupied = false;
                    StateChange?.Invoke(this, EventActionArgs.Empty);
                }
            }
        }
    }
}
