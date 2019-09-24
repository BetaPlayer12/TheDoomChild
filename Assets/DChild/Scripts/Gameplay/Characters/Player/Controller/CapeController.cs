using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeController : MonoBehaviour
{
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private Character m_character;

    private void Update()
    {
        if(Input.GetAxisRaw("Horizontal") == 0)
        {
            m_animator.SetTrigger(m_character.facing == HorizontalDirection.Right ? "isFacingRight" : "isFacingLeft");
            //Debug.Log(m_character.facing == HorizontalDirection.Right ? "isFacingRight" : "isFacingLeft");
        }
        else
        {
            m_animator.SetTrigger("isMoving");
        }

    }
}
