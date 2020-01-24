using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : PlayerBehaviour
{
    // Start is called before the first frame update
    public bool isFacingRight;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var right = inputState.GetButtonValue(inputButtons[0]);
        var left = inputState.GetButtonValue(inputButtons[1]);

        if (right)
        {
            inputState.direction = Directions.Right;
            isFacingRight = true;
        }else if (left)
        {
            inputState.direction = Directions.Left;
            isFacingRight = false;
        }

        transform.localScale = new Vector3((float)inputState.direction, 1, 1);
    }
}
