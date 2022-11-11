
using DChild.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetachCameraControls : MonoBehaviour
{
    [SerializeField]
    private InputActionReference m_horizontalInput;
    [SerializeField]
    private InputActionReference m_verticalInput;
    [SerializeField]
    private InputActionReference m_zoomInput;

    [SerializeField]
    private float speed = 30.0f;

    private bool m_isUsingCameraInput;
    private Camera detachable => GameplaySystem.cinema.mainCamera;

    public void SwitchInputToCamera(bool switchToCamera)
    {
        if (switchToCamera)
        {
            if (m_isUsingCameraInput == false)
            {
                m_horizontalInput.action.performed += OnHorizontalInput;
                m_verticalInput.action.performed += OnVerticalInput;
                m_zoomInput.action.performed += OnZoomInput;
                m_isUsingCameraInput = true;
            }
        }
        else
        {
            if (m_isUsingCameraInput)
            {
                m_horizontalInput.action.performed -= OnHorizontalInput;
                m_verticalInput.action.performed -= OnVerticalInput;
                m_zoomInput.action.performed -= OnZoomInput;
                m_isUsingCameraInput = false;
            }
        }
    }

    private void OnZoomInput(InputAction.CallbackContext obj)
    {
        obj.ReadValue<float>();
        Vector3 position = detachable.transform.position;
        position.z += obj.ReadValue<float>();
        detachable.transform.position = position;
    }

    private void OnVerticalInput(InputAction.CallbackContext obj)
    {
        obj.ReadValue<float>();
        Vector3 position = detachable.transform.position;
        position.y += obj.ReadValue<float>();
        detachable.transform.position = position;
    }

    private void OnHorizontalInput(InputAction.CallbackContext obj)
    {
        obj.ReadValue<float>();
        Vector3 position = detachable.transform.position;
        position.x += obj.ReadValue<float>(); 
        detachable.transform.position = position;
    }
}
