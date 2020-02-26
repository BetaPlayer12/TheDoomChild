using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class GroundShaker : PlayerBehaviour
    {

        public float midAirDelay;
        public bool groundSmash;
        public float smashMultiplier;
        private float defGravity;
        // Start is called before the first frame update
        void Start()
        {
            defGravity = body2d.gravityScale;
        }

        // Update is called once per frame
        void Update()
        {
            var down = inputState.GetButtonValue(inputButtons[0]);
            var attack = inputState.GetButtonValue(inputButtons[1]);

            if (!collisionState.grounded && down && attack)
            {
               
                body2d.velocity = Vector2.zero;
                groundSmash = true;
                body2d.gravityScale = 0f;
                ToggleScripts(false);
                StartCoroutine(GroundSmashDelayRoutine());
            }
            else
            {
               // Debug.Log("grounded");
            }
        }

        IEnumerator GroundSmashDelayRoutine()
        {
            Debug.Log("yeild");
            yield return new WaitForSeconds(midAirDelay);
            body2d.gravityScale = defGravity* smashMultiplier;
            

        }
        

        public void GroundSmashFinishAnimation()
        {
            groundSmash = false;
            Debug.Log("finish animation");
            body2d.gravityScale = defGravity;
            ToggleScripts(true);
        }

    }

}
