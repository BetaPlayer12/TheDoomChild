using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class WallStick : PlayerBehaviour
    {
        [SerializeField]
        private LongJump m_doubleJump;
        public List<Collider2D> m_colliderList;
        private WallGrab wallGrab;

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

            m_colliderList = new List<Collider2D>();
            wallGrab = GetComponent<WallGrab>();
            defaultGravityScale = rigidBody.gravityScale;
            defaultDrag = rigidBody.drag;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (stateManager.onWall || stateManager.onWallLeg)
            {
                groundWallStick = true;
            }
            else
            {
                groundWallStick = false;
            }

            if (stateManager.onWall && stateManager.onWallLeg)
            {
                if (!onWallDetected)
                {
                    Onstick();
                    ToggleScripts(false);
                    onWallDetected = true;
                    wallSticking = true;
                }
                else
                {
                    //Already on wall
                    OnWall();
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
            if (!stateManager.isGrounded && stateManager.onWall && stateManager.onWallLeg)
            {
                //rigidBody.gravityScale = 0;
                //rigidBody.drag = 20;

                wallSticking = true;
                wallGrounded = false;

                m_doubleJump.jumpsRemaining = 1;
            }

            if (stateManager.isGrounded && stateManager.onWall && stateManager.onWallLeg)
            {
                wallGrounded = true;
            }
        }

        protected virtual void OnWall()
        {
            if (stateManager.isGrounded && stateManager.onWall && stateManager.onWallLeg)
            {
                rigidBody.gravityScale = defaultGravityScale;
                rigidBody.drag = defaultDrag;

                wallGrounded = true;
            }
        }

        protected virtual void Offwall()
        {
            ResetColliders();
            stateManager.ToggleGroundChecker(true);

            if (rigidBody.gravityScale != defaultGravityScale)
            {
                rigidBody.gravityScale = defaultGravityScale;
                rigidBody.drag = defaultDrag;
                wallSticking = false;
            }
        }

        public void ResetColliders()
        {
            if (m_colliderList.Count > 0)
            {
                for (int i = 0; i < m_colliderList.Count; i++)
                {
                    Physics2D.IgnoreCollision(capsuleCollider, m_colliderList[i], false);
                }

                m_colliderList.Clear();
            }
        }
    }
}
