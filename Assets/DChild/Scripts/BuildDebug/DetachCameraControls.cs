
using DChild.Gameplay;
using DChild.Gameplay.Characters.Players.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetachCameraControls : MonoBehaviour
{
    [SerializeField]
    private PlayerInput m_input;

    [SerializeField]
    private InputActionReference m_VerticalMovement;
    [SerializeField]
    private InputActionReference m_HorizontalMovement;
    [SerializeField]
    private InputActionReference m_mouseMovement;

    [SerializeField]
    private float speed = 30.0f;
    private Camera detachable => GameplaySystem.cinema.mainCamera;

    public float horizontalMovement;
    public float verticalMovement;
    public float mouseMovement;

    public void ActivateControls()
    {
        horizontalMovement = 0;
        verticalMovement = 0;
        m_input.currentActionMap.Disable();
        m_input.SwitchCurrentActionMap("Debug Camera");
        m_input.currentActionMap.Enable();
        enabled = true;
    }

    public void DeactivateControls()
    {
        m_input.currentActionMap.Disable();
        m_input.SwitchCurrentActionMap("Gameplay");
        m_input.currentActionMap.Enable();
        enabled = false;
    }

    public void onCameraMoveHorizontal(InputAction.CallbackContext obj)
    {
        horizontalMovement = obj.ReadValue<float>();

    }
    public void onCameraMoveVertical(InputAction.CallbackContext obj)
    {
        verticalMovement = obj.ReadValue<float>();
    }

    public void onMouseZoom(InputAction.CallbackContext obj)
    {
        mouseMovement = obj.ReadValue<float>();
    }



    private void Start()
    {
        m_HorizontalMovement.action.performed += onCameraMoveHorizontal;
        m_VerticalMovement.action.performed += onCameraMoveVertical;
        m_mouseMovement.action.performed += onMouseZoom;
    }

    //Update is called once per frame
    void Update()
    {
        Vector3 position = detachable.transform.position;
        position.x += horizontalMovement;
        position.y += verticalMovement;
        position.z += mouseMovement;
        detachable.transform.position = position;
    }
}
