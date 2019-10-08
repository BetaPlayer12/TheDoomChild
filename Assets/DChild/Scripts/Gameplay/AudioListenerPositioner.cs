using DChild;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerPositioner : MonoBehaviour
{
    private Transform m_camera;

    private void OnCameraChange(object sender, CameraChangeEventArgs eventArgs)
    {
        m_camera = eventArgs.camera?.transform ?? null;
    }

    private void Start()
    {
        m_camera = GameSystem.mainCamera?.transform ?? null;
        GameSystem.CameraChange += OnCameraChange;
    }

    private void LateUpdate()
    {
        var position = m_camera.position;
        position.z = 0;
        transform.position = position;
    }
}
