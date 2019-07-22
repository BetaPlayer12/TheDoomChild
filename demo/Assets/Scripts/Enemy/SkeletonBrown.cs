using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBrown : Enemy, IDamagable
{
    public int Health { get; set; }

    public override void Init()
    {
        base.Init();
        Health = base.health;
    }

    public override void Movement()
    {
        base.Movement();
      
    }

    public void Damage(int damageAmount)
    {
        if (isDead == true)
            return;
        Health--;
        anim.SetTrigger("Hit");
        isHit = true;
        anim.SetBool("InCombat", true);
        //Debug.Log("Hitting");
        if(Health < 1)
        {
            isDead = true;
            anim.SetTrigger("Death");
            //Change local Diamond Value 
            GameObject diamond = Instantiate(diamondPrefab, transform.position, Quaternion.identity) as GameObject;
            diamond.GetComponent<Diamond>().gems = base.gems;
        }
    }
}
