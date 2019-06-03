using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters.Players;
using UnityChan;

public class Cloth2D : MonoBehaviour {
    private PlayerAnimation m_animation;
    [SerializeField]
    private SpringBone m_springBone;

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_animation = GetComponentInParent<PlayerAnimation>();
    }

    private void Update()
    {
        if(m_animation.GetCurrentAnimation(0).ToString() != "Idle1_Right" && m_animation.GetCurrentAnimation(0).ToString() != "Idle1_Left")
        {
            m_animator.SetBool("isMoving", true);
        }
        else
        {
            m_animator.SetBool("isMoving", false);
        }

        //if (m_animation.GetCurrentAnimation(0).ToString() != "Idle1_Right")
        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            m_springBone.springForce = new Vector3(-0.1f, m_springBone.springForce.y, m_springBone.springForce.z);
        }
        //else if (m_animation.GetCurrentAnimation(0).ToString() != "Idle1_Left")
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            m_springBone.springForce = new Vector3(0.1f, m_springBone.springForce.y, m_springBone.springForce.z);
        }

        if(m_animation.GetCurrentAnimation(0).ToString() == "Idle3_Right")
        {
            m_springBone.springForce = new Vector3(-0.1f, m_springBone.springForce.y, m_springBone.springForce.z);
            m_animator.SetBool("isIdle3_Right", true);
        }
        else
        {
            m_animator.SetBool("isIdle3_Right", false);
        }


        if (m_animation.GetCurrentAnimation(0).ToString() == "Idle3_Left")
        {
            m_springBone.springForce = new Vector3(0.1f, m_springBone.springForce.y, m_springBone.springForce.z);
            m_animator.SetBool("isIdle3_Left", true);
        }
        else
        {
            m_animator.SetBool("isIdle3_Left", false);
        }
    }
}
