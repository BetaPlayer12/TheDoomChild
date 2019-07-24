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


    public void IdleLeft()
    {
        _anim.SetBool("isFacingLeft", true);
    }
    public void IdleRight()
    {
        _anim.SetBool("isFacingLeft", false);
    }

   public void JogRun(bool isFacingLeft, int move)
    {
        _anim.SetBool("isFacingLeft", isFacingLeft);
        _anim.SetInteger("SpeedX", move);
    }

    public void LedgeGrab(bool isFacingLeft)
    {
        _anim.SetBool("isFacingLeft", isFacingLeft);
        _anim.SetTrigger("LedgeGrab");
    }

    public void Falling( bool isFacingLeft, bool isMidAir, bool dash, int speedY)
    {
        _anim.SetInteger("SpeedY", speedY);
        _anim.SetBool("Dash", dash);
        _anim.SetBool("isFacingLeft", isFacingLeft);
        _anim.SetBool("isMidAir", isMidAir);
    }
    //No Animation on Dash
    public void Land(bool isFacingLeft)
    {
        _anim.SetBool("isFacingLeft", isFacingLeft);
        _anim.SetTrigger("Land");
    }

    public void Dash(bool isFacingLeft, bool dash)
    {
        _anim.SetBool("isFacingLeft", isFacingLeft);
        _anim.SetBool("Dash", dash);
    }

    public void Crouch(bool isFacingLeft, bool crouch)
    {
        _anim.SetBool("isFacingLeft", isFacingLeft);
        _anim.SetBool("Crouch", crouch);
    }
    
    //Need to Check on the Animator Controller
    public void CrouchWalk(bool isFacingLeft, bool crouch, int move)
    {
        _anim.SetBool("isfacingLeft", isFacingLeft);
        _anim.SetBool("Crouch", crouch);
        _anim.SetInteger("SpeedX", move);
    }
}
