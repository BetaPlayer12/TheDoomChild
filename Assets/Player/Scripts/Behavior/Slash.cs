using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : PlayerBehaviour
{

    private float timeBtwnAtck;
    public float startTimeBtwAttck;


    public bool attacking;
    public float attackHold = 0.5f;
    public float resetTime;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;

    // Update is called once per frame
    void Update()
    {
        var canSlash = inputState.GetButtonValue(inputButtons[0]);
        var holdTime = inputState.GetButtonHoldTime(inputButtons[0]);
        
       if(timeBtwnAtck <= 0)
        {
            if (canSlash)
            {
                Collider2D[] objToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                attacking = true;
                
            }
            timeBtwnAtck = startTimeBtwAttck;
        }
        else
        {
            timeBtwnAtck -= Time.deltaTime;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private void FinishAttackAnim()
    {
        attacking = false;
        Debug.Log("finish attack");
    }
}
