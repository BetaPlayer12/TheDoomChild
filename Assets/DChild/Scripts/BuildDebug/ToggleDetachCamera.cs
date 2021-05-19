using Cinemachine;
using DChild.Gameplay;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{

    public class ToggleDetachCamera : MonoBehaviour, IToggleDebugBehaviour
    {
        [SerializeField]
        public  Camera m_detachable;
        [SerializeField]
        public bool detached = false;
        public bool value => detached;

        [Button]
        public void ToggleOn()
        {
           // m_detachable = GameplaySystem.cinema.mainCamera;
            GameplaySystem.playerManager.OverrideCharacterControls();
            m_detachable.GetComponent<CinemachineBrain>().enabled=false;
            detached = true;
        }

        [Button]
        public void ToggleOff()
        {
           // m_detachable = GameplaySystem.cinema.mainCamera;
            GameplaySystem.playerManager.StopCharacterControlOverride();
            m_detachable.GetComponent<CinemachineBrain>().enabled = true;
            detached = false;
        }
        void Update()
        {
            if (detached == true)
            {
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
                if (Input.GetKey(KeyCode.D))
                {
                    Vector3 position = m_detachable.transform.position;
                    position.x++;
                    m_detachable.transform.position = position;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    Vector3 position = m_detachable.transform.position;
                    position.x--;
                    m_detachable.transform.position = position;
                }
            }
        }
    }
}
