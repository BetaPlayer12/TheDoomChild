using PlayerNew;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerBehaviour
{
    [SerializeField]
    private float m_movementSpeed = 20f;
    [SerializeField]
    private float m_crouchSpeedDivisor = 3f;

    public int direction = 1;

    private float originalXScale;
    public bool isEnabled = true;

    void Start()
    {
        originalXScale = transform.localScale.x;
    }

    private void FixedUpdate()
    {
        if (isEnabled)
        {
            GroundMovement();
            MidAirMovement();
        }
    }

    void GroundMovement()
    {
        float xVelocity = m_movementSpeed * inputState.horizontal;

        if (Mathf.Abs(xVelocity) > 0)
        {
            stateManager.isIdle = false;
        }

        if (xVelocity * direction < 0)
        {
            FlipCharacterDirection();
        }

        if (stateManager.isCrouching)
        {
            xVelocity /= m_crouchSpeedDivisor;
        }

        rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);
    }

    void MidAirMovement()
    {

    }

    public void FlipCharacterDirection()
    {
        direction *= -1;

        Vector3 scale = transform.localScale;

        scale.x = originalXScale * direction;

        if (scale.x > 0)
        {
            facing.isFacingRight = true;
            inputState.direction = Directions.Right;
            character.SetFacing(DChild.Gameplay.Characters.HorizontalDirection.Right);
        }
        else if (scale.x < 0)
        {
            facing.isFacingRight = false;
            inputState.direction = Directions.Left;
            character.SetFacing(DChild.Gameplay.Characters.HorizontalDirection.Left);
        }

        transform.localScale = scale;
    }

    public void EnableMovement()
    {
        isEnabled = true;
    }

    public void DisableMovement()
    {
        isEnabled = false;
        inputState.horizontal = 0f;
    }
}
