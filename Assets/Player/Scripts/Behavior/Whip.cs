using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Whip : PlayerBehaviour
    {
        public bool whipAtk = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var canWhip = inputState.GetButtonValue(inputButtons[0]);

            if (canWhip && !whipAtk)
            {
                whipAtk = true;
                ToggleScripts(false);
            }
        }

        private void WhipFinishAttack()
        {
            whipAtk = false;
            ToggleScripts(true);
        }
    }

}

