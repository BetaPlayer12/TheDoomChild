using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Jog : PlayerBehaviour
    {

        public float speed = 10f;
        public float speedMultiplier = 1.5f;
        public float jogTimer = 3.5f;
        public float crouchSpeedMultiplier = 0.5f;
        public bool jogging = false;
        

        private CapsuleCollider2D collider2D;

        // Start is called before the first frame update
        void Start()
        {
            collider2D = GetComponent<CapsuleCollider2D>();
        }

        // Update is called once per frame
        protected virtual void FixedUpdate()
        {
            var right = inputState.GetButtonValue(inputButtons[0]);
            var left = inputState.GetButtonValue(inputButtons[1]);
            var down = inputState.GetButtonValue(inputButtons[2]);

            var velX = speed;


            if (right || left)
            {
                jogTimer -= Time.deltaTime;

                if (jogTimer < 0)
                {
                    velX *= speedMultiplier;
                }

                
                velX *= left ? -1 : 1;
                jogging = true;

            }
            else
            {
                velX = 0;
                jogging = false;
                //No reset script yet
                jogTimer = 3.5f;
            }


            if (down) {
                body2d.velocity = new Vector2(velX * crouchSpeedMultiplier, body2d.velocity.y);
            } else
            {
                body2d.velocity = new Vector2(velX, body2d.velocity.y);
            }
            


            //if(right || left)
            //{
            //    var tmpSpeed = speed;
            //    var velX = tmpSpeed * (float)inputState.direction;
            //    body2d.velocity = new Vector2(velX, body2d.velocity.y);
            //}

        }

    }
}

