using Cinemachine;
using DChild.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChildDebug.Window
{

    public class ToggleDetachCamera : MonoBehaviour, IToggleDebugBehaviour
    {
        [SerializeField]
        private float speed = 30.0f;
        private Camera m_detachable;
        private bool detached = false;
        public bool value => detached;

        [Button]
        public void ToggleOn()
        {
            m_detachable = GameplaySystem.cinema.mainCamera;
            m_detachable.GetComponent<CinemachineBrain>().enabled = false;
            GameplaySystem.playerManager.OverrideCharacterControls();
            detached = true;
        }

        [Button]
        public void ToggleOff()
        {
            m_detachable = GameplaySystem.cinema.mainCamera;
            m_detachable.GetComponent<CinemachineBrain>().enabled = true;
            GameplaySystem.playerManager.StopCharacterControlOverride();
            detached = false;
        }
        void Update()
        {
            if (detached == true)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    Vector3 position = m_detachable.transform.position;
                    position.x--;
                    m_detachable.transform.position = position;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    Vector3 position = m_detachable.transform.position;
                    position.x++;
                    m_detachable.transform.position = position;
                }

                if (Input.GetKey(KeyCode.W))
                {
                    Vector3 position = m_detachable.transform.position;
                    position.y++;
                    m_detachable.transform.position = position;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    Vector3 position = m_detachable.transform.position;
                    position.y--;
                    m_detachable.transform.position = position;
                }
                if (Input.GetKey(KeyCode.Z))
                {
                    Vector3 position = m_detachable.transform.position;
                    position.z++;
                    m_detachable.transform.position = position;
                }

                if (Input.GetKey(KeyCode.X))
                {
                    Vector3 position = m_detachable.transform.position;
                    position.z--;
                    m_detachable.transform.position = position;
                }
            }
        }
    }
}
