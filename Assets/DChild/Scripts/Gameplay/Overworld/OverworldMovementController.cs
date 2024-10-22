using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class OverworldMovementController : MonoBehaviour
{
    
    [SerializeField]
    private float m_moveSpeed;
    [SerializeField]
    private Rigidbody2D m_rigidbody;
    [SerializeField]
    private PlayerInput m_playerinput;
    [SerializeField]
    private OverworldObjectInteraction m_objectInteraction;

    private float m_currentSpeed;
    public float horizontalInput;
    public float verticalInput;
    public bool interactPressed;
    public OverworldCharacterAnimatorHandle m_animationhandler;

    public void Move(float directionx, float directiony)
    {
        var xVelocity = m_moveSpeed * directionx;
        var yVelocity = m_moveSpeed * directiony;
        m_rigidbody.velocity = new Vector2(xVelocity, yVelocity);
        m_animationhandler.UpdateAnimator(new Vector2(xVelocity, yVelocity));
    }
    public void Disable()
    {
        if (this.enabled)
        {
            Reset();
        }
        m_playerinput.enabled = false;
        this.enabled = false;
    }

    public void Enable()
    {
        if (this.enabled == false)
        {
            Reset();
        }
        m_playerinput.enabled = true;
        this.enabled = true;
    }
   
        private void OnHorizontalInput(InputValue value)
    {
        if (enabled == true)
        {
            horizontalInput = value.Get<float>();

            if (horizontalInput < 1 && horizontalInput > -1)
            {
                horizontalInput = 0;
            }
        }
    }

    private void OnVerticalInput(InputValue value)
    {
        if (enabled == true)
        {
            verticalInput = value.Get<float>();

            if (verticalInput < 1 && verticalInput > -1)
            {
                verticalInput = 0;
            }
        }
    }
    private void OnInteract(InputValue value)
    {
        if (enabled == true)
        {
            interactPressed = value.Get<float>() == 1;
        }
    }
    private void MoveCharacter()
    {
       Move(horizontalInput, verticalInput);
    }

    private void Awake()
    {
        //m_playerinput = GetComponent<PlayerInput>();
        //m_rigidbody = GetComponent<Rigidbody2D>();
       // m_objectInteraction = GetComponent<OverworldObjectInteraction>();
    }

    private void Reset()
    {
        horizontalInput = 0;
        verticalInput = 0;
        interactPressed = false;
    }

    private void Update()
    {
        MoveCharacter();
        if (interactPressed)
        {
            m_objectInteraction?.Interact();
            interactPressed = false;
        }
    }
}
