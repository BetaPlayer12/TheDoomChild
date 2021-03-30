using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastRadius : MonoBehaviour
{
    public float radius = 5.0F;
    public float power = 10.0F;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 explosionPos = transform.position;
       
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Enemy" || LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0,0);

            if (rb != null)
                {
                   
                var dir = (rb.transform.position - explosionPos);
                float wearoff = 1 - (dir.magnitude / radius);
                rb.AddForce(dir.normalized * power * wearoff);
                
                }
        }
  
    }
 
}
