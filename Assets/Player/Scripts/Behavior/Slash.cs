using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Slash : PlayerBehaviour
    {

        private float timeBtwnAtck;
        private float attackTimeCounter;
        public float attackingTime;
        public int attackCounter;
        private float attackHold;
        private float comboTimer;

        public float startTimeBtwAttck;


        public bool attackHolding;
        public bool attacking;
       // public float attackHold = 0.5f;
        public int comboCount;
        public float resetTime;

        public Transform attackPos;
        public LayerMask whatIsEnemies;
        public float attackRange;

        private Crouch crouchState;

        private void Start()
        {
            crouchState = GetComponent<Crouch>();
        }

        // Update is called once per frame
        void Update()
        {
            var canSlash = inputState.GetButtonValue(inputButtons[0]);
            var holdTime = inputState.GetButtonHoldTime(inputButtons[0]);
           


            //if(canSlash && attackCounter <= comboCount)
            //{
            //    Debug.Log(startTimeBtwAttck);
            //    if(startTimeBtwAttck > 0.1f)
            //    {
            //        Debug.Log("attacking:" + attackCounter);
            //        attackCounter++;
            //        timeBtwnAtck -= Time.deltaTime;
            //    }
            //    else
            //    {
            //        attackCounter = 0;
            //        timeBtwnAtck = resetTime;
            //    }

            //}



            //if (attackTimeCounter > 0.1f)
            //{
            //    attackTimeCounter -= Time.deltaTime;
            //    //attacking = true;
            //    if (canSlash && holdTime < 0.1f)
            //    {
            //        attackCounter++;
            //    }
            //    Debug.Log("attack:" + attackCounter);
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

                        if (attackCounter == 0)
                        {
                            comboTimer = attackingTime;
                        }
                        else if (attackCounter == 3)
                        {
                            attackCounter = 0;
                            comboTimer = 1.0f;
                        }
                        else
                        {
                           //nothing yet
                        }
                        ToggleScripts(false);
                        Collider2D[] objToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                        attacking = true;
                    }

                    

                    if (holdTime > attackHold)
                    {
                        //Debug.Log("holding attack");
                    }

                    

                    timeBtwnAtck = startTimeBtwAttck;
                }
                else
                {
                    timeBtwnAtck -= Time.deltaTime;
                }
            if (comboTimer > 0.1f && attackCounter >= 1) {
                comboTimer -= Time.deltaTime;
            } else
            {
                comboTimer = attackingTime;
                attackCounter = 0;
            }

            //Debug.Log("Combo timer:" + attackCounter);
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos.position, attackRange);
        }

        private void FinishAttackAnim()
        {
            attackCounter++;
            attacking = false;
            ToggleScripts(true);
            
        }
    }
}

