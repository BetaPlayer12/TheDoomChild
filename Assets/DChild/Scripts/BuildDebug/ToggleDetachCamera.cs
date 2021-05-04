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
        public Camera m_detachable;
        private bool detached = false;
        private float speed = 30.0f;
        public bool value => throw new System.NotImplementedException();

        [Button]
        public void ToggleOn()
        {
            m_detachable = GameplaySystem.cinema.mainCamera;
            GameplaySystem.playerManager.OverrideCharacterControls();
            detached = true;
        }

        [Button]
        public void ToggleOff()
        {
            m_detachable = GameplaySystem.cinema.mainCamera;
            GameplaySystem.playerManager.StopCharacterControlOverride();
            detached = false;
        }
        void Update()
        {
            if (detached == true)
            {
                if (Input.GetKey(KeyCode.A))
                    m_detachable.transform.position += Vector3.left * speed * Time.deltaTime;
                if (Input.GetKey(KeyCode.D))
                    m_detachable.transform.position += Vector3.right * speed * Time.deltaTime;
                if (Input.GetKey(KeyCode.W))
                    m_detachable.transform.position += Vector3.up * speed * Time.deltaTime;
                if (Input.GetKey(KeyCode.S))
                    m_detachable.transform.position += Vector3.down * speed * Time.deltaTime;
            }
        }
    }
}
