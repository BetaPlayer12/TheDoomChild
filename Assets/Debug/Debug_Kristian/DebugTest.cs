using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
    public Vector3 pos;
    public float yPos;
    public float xPos;


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        pos = new Vector3(this.transform.position.x + xPos, this.transform.position.y + yPos, this.transform.position.z);
        Debug.DrawLine(this.transform.position, pos, Color.red);
    }

}
