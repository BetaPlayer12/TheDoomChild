using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPlayerChecker : MonoBehaviour
{
    [SerializeField]
    private TentacleGrab m_tentacleGrab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DamageCollider") || collision.CompareTag("Sensor"))
            m_tentacleGrab.GrabbedPlayer();
            return;
    }
}
