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
        private CelestialCube m_storedCube;
        [SerializeField, TabGroup("On")]
        private UnityEvent m_onEvents;
        [SerializeField, TabGroup("Off")]
        private UnityEvent m_offEvents;
        private bool m_lockDownWhenStored;
        private bool m_readyLock;
        private float m_proximitymin;
        private float m_proximitymax;
        private float m_cubePosition;
        [SerializeField]
        private float m_approximation;

        public event EventAction<EventActionArgs> StateChange;
        public bool isOccupied => m_storedCube;
        public bool readyLock => m_readyLock;
        public SerializeID ID => m_ID;

        private void OnDrawGizmos()
        {
            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.color = new Color(1, 1, 1, 1);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 5);
        }

        public void SetLockDown(bool lockDown)
        {
            m_lockDownWhenStored = lockDown;
            if (m_storedCube != null)
            {
                m_storedCube.SetInteraction(!m_lockDownWhenStored);
            }
        }

        private void OnCubeStateChange(object sender, EventActionArgs eventArgs)
        {
            var cube = (CelestialCube)sender;
            if (m_storedCube == null)
            {
                if (cube.isInASlot == false)
                {
                    m_storedCube = cube;
                    //Do Something;
                    StateChange?.Invoke(this, EventActionArgs.Empty);
                    if (m_lockDownWhenStored)
                    {
                        m_storedCube.SetInteraction(false);
                    }
                    m_storedCube.OnStateChange -= OnCubeStateChange;
                }
            }
            else
            {
                cube.OnStateChange -= OnCubeStateChange;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_storedCube == null)
            {
                if (collision.gameObject.TryGetComponentInParent(out CelestialCube cube))
                {
                    if (cube.isInASlot)
                    {
                        cube.OnStateChange += OnCubeStateChange;
                    }
                    else
                    {
                        m_storedCube = cube;
                        cube.SetState(true);
                        StateChange?.Invoke(this, EventActionArgs.Empty);
                        if (m_lockDownWhenStored)
                        {
                            m_storedCube.SetInteraction(false);
                        }
                        //Do Something;
                    }
                }
            }

        }



        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponentInParent(out CelestialCube cube))
            {
                if (m_storedCube == cube)
                {
                    m_storedCube.SetState(false);
                    m_storedCube = null;
                    StateChange?.Invoke(this, EventActionArgs.Empty);
                    m_readyLock = false;
                }
                cube.OnStateChange -= OnCubeStateChange;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            m_proximitymin = transform.position.x;
            m_proximitymin = m_proximitymin - m_approximation;
            m_proximitymax = transform.position.x;
            m_proximitymax = m_proximitymax + m_approximation;
            if (collision.gameObject.TryGetComponentInParent(out CelestialCube cube))
            {
                if (cube.isInASlot)
                {
                    m_cubePosition = cube.transform.position.x;
                    if (m_cubePosition >= m_proximitymin && m_cubePosition <= m_proximitymax)
                    {
                        m_readyLock = true;

                    }
                    else
                    {
                        m_readyLock = false;
                    }
                }
            }
        }
    }
}
