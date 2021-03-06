﻿using DChild;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    [AddComponentMenu("DChild/Gameplay/Audio/Audio Listener Positioner")]
    public class AudioListenerPositioner : MonoBehaviour
    {
        private Transform m_camera;

        private static AudioListenerPositioner m_instance;

        private void OnCameraChange(object sender, CameraChangeEventArgs eventArgs)
        {
            m_camera = eventArgs.camera?.transform ?? null;
            enabled = m_camera;
        }

        private void Awake()
        {
            if (m_instance)
            {
                Destroy(gameObject);
            }
            else
            {
                m_instance = this;
            }
        }

        private void Start()
        {
            GameSystem.CameraChange += OnCameraChange;
            m_camera = GameSystem.mainCamera?.transform ?? null;
            enabled = m_camera;
        }

        private void LateUpdate()
        {
            if (m_camera == null)
            {
                m_camera = GameSystem.mainCamera?.transform ?? null;
            }
            else
            {
                var position = m_camera.position;
                position.z = 0;
                transform.position = position;
            }
        }

        private void OnDestroy()
        {
            if (this == m_instance)
            {
                m_instance = null;
            }
        }
    } 
}
