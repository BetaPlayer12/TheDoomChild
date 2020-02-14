using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Slope : PlayerBehaviour
    {
        

        private FaceDirection facing;

        public bool facingRight;
        public float maxClimbAngle;
        // Start is called before the first frame update
        void Start()
        {
            facing = GetComponent<FaceDirection>();
        }

        // Update is called once per frame
        void Update()
        {


            if (body2d.velocity.x != 0)
            {
                RaycastHit2D hit = facing.isFacingRight ? collisionState.slopeRightHit : collisionState.slopeLeftHit;
                if (hit.collider)
                {
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    Debug.Log("horizontal angle:" + slopeAngle);
                }
            }
            if(body2d.velocity.y != 0)
            {
                RaycastHit2D hit = collisionState.slopeBotHit;
                if (hit.collider)
                {
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    Debug.Log("vertical angle:" + slopeAngle);
                }
            }

           
        }
    }


}