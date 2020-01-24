using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHop : PlayerBehaviour
{
    private FaceDirection facingDirection;

    public float wallHopForce;
    public float wallJumpForce;
    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    // Start is called before the first frame update
    void Start()
    {
        facingDirection = GetComponent<FaceDirection>();
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        var canJump = inputState.GetButtonValue(inputButtons[0]);
        //if (canJump && !jumpingOffWall)
    }
}
