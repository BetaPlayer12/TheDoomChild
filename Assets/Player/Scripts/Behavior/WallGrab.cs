using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrab : PlayerBehaviour
{

    private FaceDirection facing;

    public bool canLedgeGrab = false;
    public bool ledgeDetected;

    private float defaultGravityScale;
    private float defaultDrag;

    public Vector2 ledgeBotPos;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    public float ledgeClimbXOffset1;
    public float ledgeClimbYOffset1;
    public float ledgeClimbXOffset2;
    public float ledgeClimbYOffset2;


    // Start is called before the first frame update
    void Start()
    {
        facing = GetComponent<FaceDirection>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (collisionState.onWall && !collisionState.grounded && !ledgeDetected)
        //{
        //    ledgeDetected = true;
        //    ledgeBotPos = trans.position;
        //    ToggleScripts(false);
        //    CheckLedgeClimb();

        //    //call animation
        //}
        if (!collisionState.grounded && !collisionState.isTouchingLedge && collisionState.onWall)
        {
            OnWallGrab();
        }

    }

    private void OnWallGrab()
    {
       
        if (!canLedgeGrab)
        {
            
            ledgeBotPos = trans.position;
            if (facing.isFacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgeBotPos.x + collisionState.rightPosition.x) - ledgeClimbXOffset1, Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgeBotPos.x + collisionState.rightPosition.x) + ledgeClimbXOffset2, Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset2);
            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgeBotPos.x - collisionState.rightPosition.x) + ledgeClimbXOffset1, Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgeBotPos.x - collisionState.rightPosition.x) - ledgeClimbXOffset2, Mathf.Floor(ledgeBotPos.y) + ledgeClimbYOffset2);
            }
           
            ToggleScripts(false);
            canLedgeGrab = true;
            ledgeDetected = true;
            
        }

        if (canLedgeGrab)
        {
            body2d.gravityScale = 0;
            body2d.drag = 100;
           // StartCoroutine(FinishedLedgeClimbRoutine());
            //ToggleScripts(false);
        }

        //FinishedLedgeClimb();

    }

    //IEnumerator FinishedLedgeClimbRoutine()
    //{
    //    yield return new WaitForSeconds(1.5f);
    //    transform.position = ledgePos1;
    //    ledgeDetected = false;
    //    ledgeBotPos = Vector2.zero;
    //    canLedgeGrab = false;
    //    ToggleScripts(true);

    //}
    private void FinishedLedgeClimb()
    {
        transform.position = ledgePos1;
        ledgeBotPos = Vector2.zero;
        ledgeDetected = false;
        canLedgeGrab = false;
        ToggleScripts(true);
        Debug.Log("ledge grab finish");
    }
}


