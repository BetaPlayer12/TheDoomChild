﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStick : PlayerBehaviour
{
    private WallGrab wallGrab;
    public bool onWallDetected;
    public bool wallSticking;

    protected float defaultGravityScale;
    protected float defaultDrag;
    // Start is called before the first frame update
    void Start()
    {
        wallGrab = GetComponent<WallGrab>();
        defaultGravityScale = body2d.gravityScale;
        defaultDrag = body2d.drag;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (collisionState.onWall && !wallGrab.canLedgeGrab) {
           
            if (!onWallDetected)
            {
                Onstick();
                ToggleScripts(false);
                onWallDetected = true;
                wallSticking = true;
            }
        } else {
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
        if (!collisionState.grounded)
        {
            body2d.gravityScale = 0;
            body2d.drag = 100;
            wallSticking = true;
        }
    }

    protected virtual void Offwall()
    {
        if(body2d.gravityScale != defaultGravityScale)
        {
            body2d.gravityScale = defaultGravityScale;
            body2d.drag = defaultDrag;
            wallSticking = false;
        }
    }
}
