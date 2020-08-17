using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    [SerializeField]
    private float m_factor;
    [SerializeField]
    private float m_offset;

    void Update()
    {
        if (GetComponent<IsolatedObjectPhysics2D>().simulateGravity)
        {
            /*
            float rotateSpeed = GetComponent<Rigidbody2D>().velocity.magnitude * (m_factor * transform.localScale.x);
            var rotateVelocity = (GetComponent<Rigidbody2D>().velocity.x * GetComponent<Rigidbody2D>().velocity.y) * m_offset;
            //transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotateVelocity));*/
            Vector2 v = GetComponent<Rigidbody2D>().velocity;
            var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
