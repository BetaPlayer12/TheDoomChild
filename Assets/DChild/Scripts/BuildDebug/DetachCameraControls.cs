
using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachCameraControls : MonoBehaviour
{
    [SerializeField]
    private float speed = 30.0f;
    private Camera detachable => GameplaySystem.cinema.mainCamera;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 position = detachable.transform.position;
            position.x--;
            detachable.transform.position = position;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Vector3 position = detachable.transform.position;
            position.x++;
            detachable.transform.position = position;
        }

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 position = detachable.transform.position;
            position.y++;
            detachable.transform.position = position;
        }

        if (Input.GetKey(KeyCode.S))
        {
            Vector3 position = detachable.transform.position;
            position.y--;
            detachable.transform.position = position;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 position = detachable.transform.position;
            position.z++;
            detachable.transform.position = position;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            Vector3 position = detachable.transform.position;
            position.z--;
            detachable.transform.position = position;
        }
    }
}
