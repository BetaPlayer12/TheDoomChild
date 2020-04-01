using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnewayPlatform : MonoBehaviour
{
    private PlatformEffector2D platformEffector;
    public float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput == 0)
        {
            platformEffector.rotationalOffset = 0.0f;
            waitTime = 0.5f;
        }

        if (verticalInput < 0)
        {
            if(waitTime <= 0)
            {
                platformEffector.rotationalOffset = 180.0f;
                waitTime = 0.5f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }    

        }
        
    }
}
