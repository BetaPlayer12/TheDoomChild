using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public void Move(float move)
    {
        //Get Absolute Value of directional movement
        _anim.SetFloat("Jog R", Mathf.Abs(move));
    }

    public void Jump(bool _isJumping)
    {
        _anim.SetBool("Jumping", _isJumping);
    }

    public void Attack()
    {
        _anim.SetTrigger("Attack");
    }

    public void Death()
    {
        _anim.SetTrigger("Death");
    }
}
