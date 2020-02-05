using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Slash : PlayerBehaviour
    {

        private float timeBtwnAtck;
        private float attackTimeCounter;
        private float attackingTime;
        private int attackCounter;
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

            //if(attackTimeCounter > 0.1f)
            //{
            //    attackTimeCounter -= Time.deltaTime;
            //    attacking = true;
            //    if (canSlash)
            //    {
            //        attackCounter++;
            //    }
            //    Debug.Log(attackCounter);
            //}
            //else
            //{
            //    attackTimeCounter = resetTime;
            //    attackCounter = 0;
            //}

            if (timeBtwnAtck < 0)
            {
                if (canSlash && holdTime < 0.1f && attacking == false)
                {
                    ToggleScripts(false);
                    Collider2D[] objToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                    attacking = true;

                }

                if (holdTime > attackHold)
                {
                    Debug.Log("holding attack");
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
            ToggleScripts(true);
            Debug.Log("attack end");
        }
    }
}

