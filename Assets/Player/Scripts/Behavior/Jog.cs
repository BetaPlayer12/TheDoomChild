using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jog : PlayerBehaviour
{

    public float speed = 10f;
    public float speedMultiplier = 1.5f;
    public float jogTimer = 3.5f;
    public bool jogging = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        var right = inputState.GetButtonValue(inputButtons[0]);
        var left = inputState.GetButtonValue(inputButtons[1]);
        
        var velX = speed;
       

        if (right || left)
        {
            jogTimer -= Time.deltaTime;
        
            if (jogTimer < 0)
            {
                velX *= speedMultiplier;
            }
           
            
            velX *= left ? -1 : 1;
            jogging = true;

        }
        else
        {
            velX = 0;
            jogging = false;
            //No reset script yet
            jogTimer = 3.5f;
        }

        body2d.velocity = new Vector2(velX, body2d.velocity.y);
       

        //if(right || left)
        //{
        //    var tmpSpeed = speed;
        //    var velX = tmpSpeed * (float)inputState.direction;
        //    body2d.velocity = new Vector2(velX, body2d.velocity.y);
        //}

    }
}
