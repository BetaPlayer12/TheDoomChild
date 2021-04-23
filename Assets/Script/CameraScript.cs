using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5.0f;
    void Start()
    {
        //transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        Movement();
    }

    private void Movement()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * speed * hor *Time.deltaTime);


        transform.Translate(Vector3.up * speed * ver *Time.deltaTime);
    }
}
