using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class WallSlide : WallStick
    {
        public float slideVelocity = -5f;
        public float slideMultiplier = 5f;
        override protected void Update()
        {
            base.Update();

            if (onWallDetected)
            {
                var velY = slideVelocity;
                if (inputState.GetButtonValue(inputButtons[0]))
                {
                    velY *= slideMultiplier;
                }
                body2d.velocity = new Vector2(body2d.velocity.x, velY);
            }
        }

        override protected void Onstick()
        {
            base.Onstick();
            body2d.velocity = Vector2.zero;
        }

        protected override void Offwall()
        {
            base.Offwall();
        }
    }

}
