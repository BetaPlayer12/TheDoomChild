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
        _anim.SetFloat("SpeedX", move);
       if(move < 0)
        {
            _anim.SetBool("isFacingLeft", true);
        }else if(move > 0)
        {
            _anim.SetBool("isFacingLeft", false);
        }
    }

    public void Jump(bool isMidAir, bool isJumping)
    {
        if(isJumping == true)
        {
            _anim.SetBool("isMidAir", isMidAir);
            _anim.SetTrigger("Jump");
        }
        else
        {
            _anim.SetBool("isMidAir", isMidAir);
            _anim.ResetTrigger("Jump");
        }
        
    }

    public void Attack()
    {
        _anim.SetTrigger("Attack");
    }

    public void Death()
    {
        _anim.SetTrigger("Death");
    }

    public void Grounded()
    {
        _anim.ResetTrigger("Jump");
        _anim.SetBool("isMidAir", false);
    }
}
