using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class WallStick : PlayerBehaviour
    {
        //private WallGrab wallGrab;
        public bool onWallDetected;
        public bool wallSticking;
        public bool groundWallStick;
        public bool wallGrounded;

        protected float defaultGravityScale;
        protected float defaultDrag;


        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            //wallGrab = GetComponent<WallGrab>();
            defaultGravityScale = body2d.gravityScale;
            defaultDrag = body2d.drag;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (collisionState.onWall || collisionState.onWallLeg)
            {
                groundWallStick = true;
            }
            else
            {
                groundWallStick = false;
            }

           

            if (collisionState.onWall && collisionState.onWallLeg)
            {

                if (!onWallDetected)
                {
                    Onstick();
                    ToggleScripts(false);
                    onWallDetected = true;
                    wallSticking = true;
                }
            }
            else
            {
                if (onWallDetected)
                {
                    Offwall();
                    ToggleScripts(true);
                    onWallDetected = false;
                    wallSticking = false;
                }
            }
        }

        protected virtual void Onstick()
        {
            //if (!collisionState.grounded && body2d.velocity.y > 0)

            if (!collisionState.grounded && collisionState.onWall && collisionState.onWallLeg)
            {
                //body2d.gravityScale = 0;
                //body2d.drag = 100;
                wallSticking = true;
                wallGrounded = false;
            }
            if (collisionState.grounded && collisionState.onWall && collisionState.onWallLeg)
                wallGrounded = true;


        }

        protected virtual void Offwall()
        {
            if (body2d.gravityScale != defaultGravityScale)
            {
                
                body2d.gravityScale = defaultGravityScale;
                body2d.drag = defaultDrag;
                wallSticking = false;
            }
        }
    }

}
