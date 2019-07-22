using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerAnimation : MonoBehaviour
{
    private Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public void Move(float move, bool facing)
    {
        
        //Get Absolute Value of directional movement
        _anim.SetInteger("SpeedX", (int) move);
        _anim.SetBool("isFacingLeft", facing);
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
