using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldCharacterAnimatorHandle : MonoBehaviour
{
    private Animator m_animator;

    private int m_parameterIDMoveX;
    private int m_parameterIDMoveY;
    private int m_parameterIDMoveSpeed;

    public void UpdateAnimator(Vector2 input)
    {
        var speed = input.sqrMagnitude;
        m_animator.SetFloat(m_parameterIDMoveSpeed, speed);
        if (speed > 0)
        {
            SetDirection(input);
        }
    }

    private void SetDirection(Vector2 input)
    {
        SetMoveDirection(m_parameterIDMoveX, input.x);
        SetMoveDirection(m_parameterIDMoveY, input.y);
    }

    private void SetMoveDirection(int parameterID, float value)
    {
        if (value == 0)
        {
            m_animator.SetFloat(parameterID, 0);
        }
        else
        {
            m_animator.SetFloat(parameterID, Mathf.Sign(value));
        }
    }

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_parameterIDMoveX = Animator.StringToHash("MoveX");
        m_parameterIDMoveY = Animator.StringToHash("MoveY");
        m_parameterIDMoveSpeed = Animator.StringToHash("MoveSpeed");

        SetDirection(Vector2.down);
    }
}
