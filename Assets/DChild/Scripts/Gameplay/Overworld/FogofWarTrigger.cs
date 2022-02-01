using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogofWarTrigger : MonoBehaviour
{
    [SerializeField]
    private bool m_reveal=false;

    private void OnTriggerEnter2D(Collider2D collision)
        {

            var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
            if (playerObject != null && collision.tag != "Sensor")
            {
                reveal();
            }
        }
        private void reveal(){
                m_reveal=true;
        }
}
