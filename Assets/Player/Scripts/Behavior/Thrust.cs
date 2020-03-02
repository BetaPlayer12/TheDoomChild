using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Thrust : PlayerBehaviour
    {
        public bool chargingAttack;
        public bool thrustAttack;
        public bool thrustHasStarted;
        public float timeToCharge;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // add horizontal movement

            var holdTime = inputState.GetButtonHoldTime(inputButtons[0]);
            // Debug.Log(holdTime + " holding");
            if (holdTime > timeToCharge && !chargingAttack)
            {
                Debug.Log("Charging");
                thrustAttack = true;
                ToggleScripts(false);

            }
            else if (chargingAttack && holdTime == 0)
            {
                Debug.Log("Attack");
                chargingAttack = false;
            }
        }

        private void StartChargeLoop()
        {
            chargingAttack = true;
            thrustHasStarted = true;
        }

        private void FinishThrustAttackAnime()
        {
            thrustAttack = false;
            thrustHasStarted = false;
            ToggleScripts(true);
        }
    }

}
