using DChild.Serialization;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class CelestialSlot : MonoBehaviour
    {
        [SerializeField]
        private SerializeID m_ID = new SerializeID(true);

        private CelestialCube m_storedCube;
        private bool m_lockDownWhenStored;

        public event EventAction<EventActionArgs> StateChange;
        public bool isOccupied => m_storedCube;
        public SerializeID ID => m_ID;

        public void SetLockDown(bool lockDown)
        {
            m_lockDownWhenStored = lockDown;
            if(m_storedCube != null)
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
                    StateChange?.Invoke(this,EventActionArgs.Empty);
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
                }
                cube.OnStateChange -= OnCubeStateChange;
            }
        }
    }
}
