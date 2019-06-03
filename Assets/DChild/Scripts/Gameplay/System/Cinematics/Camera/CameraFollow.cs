using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Cinematics.Cameras
{
    public class CameraFollow : MonoBehaviour
    {
        private Camera m_mainCamera;

        private void Start() => m_mainCamera = GameplaySystem.cinema.mainCamera;

        private void Update()
        {
            var position = m_mainCamera.transform.position;
            transform.position = new Vector3(position.x, position.y, 0);
        }
    }
}