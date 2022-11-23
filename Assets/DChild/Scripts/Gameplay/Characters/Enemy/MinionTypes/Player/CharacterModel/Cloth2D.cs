using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters;
//using UnityChan;

public class Cloth2D : MonoBehaviour {
    private Animator m_animator;
    private Player m_player;
    [SerializeField]
    private Transform m_frontColliderTF;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        m_animator.SetBool("IsFacingRight", true);
        //if (GetComponentInParent<PlayerAnimation>().GetCurrentAnimation(0).ToString() == "Idle1_Right")
        //{
        //    m_animator.SetBool("IsFacingRight", true);
        //}
        //else if (GetComponentInParent<PlayerAnimation>().GetCurrentAnimation(0).ToString() == "Idle1_Left")
        //{
        //    m_animator.SetBool("IsFacingLeft", true);
        //}
    }

    private void Update()
    {
        //if (m_player.currentFacingDirection == HorizontalDirection.Right)
        //{
        //    if(Input.GetAxisRaw("Horizontal") > 0)
        //    {
        //        m_animator.SetBool("IsRunningRight", true);
        //        m_animator.SetBool("IsFacingRight", false);
        //    }
        //    else
        //    {
        //        m_animator.SetBool("IsFacingRight", true);
        //        m_animator.SetBool("IsRunningRight", false);
        //    }
        //    m_animator.SetBool("IsFacingLeft", false);
        //    m_animator.SetBool("IsRunningLeft", false);
        //    //Debug.Log("Is Facing Right");
        //    m_frontColliderTF.localPosition = new Vector3(2, m_frontColliderTF.localPosition.y, 0);
        //}
        //else if (m_player.currentFacingDirection == HorizontalDirection.Left)
        //{
        //    if (Input.GetAxisRaw("Horizontal") < 0)
        //    {
        //        m_animator.SetBool("IsRunningLeft", true);
        //        m_animator.SetBool("IsFacingLeft", false);
        //    }
        //    else
        //    {
        //        m_animator.SetBool("IsFacingLeft", true);
        //        m_animator.SetBool("IsRunningLeft", false);
        //    }
        //    m_animator.SetBool("IsFacingRight", false);
        //    m_animator.SetBool("IsRunningRight", false);
        //    //Debug.Log("Is Facing Left");
        //    m_frontColliderTF.localPosition = new Vector3(-2, m_frontColliderTF.localPosition.y, 0);
        //}

        //if (m_player.characterState.isAttacking)
        //{
        //    Debug.Log("Player is attacking");
        //    if (m_player.currentFacingDirection == HorizontalDirection.Right)
        //    {
        //        m_animator.SetBool("IsAttackingRight", true);
        //        m_animator.SetBool("IsAttackingLeft", false);
        //    }
        //    if (m_player.currentFacingDirection == HorizontalDirection.Left)
        //    {
        //        m_animator.SetBool("IsAttackingLeft", true);
        //        m_animator.SetBool("IsAttackingRight", false);
        //    }
        //}
        //else
        //{
        //    m_animator.SetBool("IsAttackingRight", false);
        //    m_animator.SetBool("IsAttackingLeft", false);
        //}
    }
}
