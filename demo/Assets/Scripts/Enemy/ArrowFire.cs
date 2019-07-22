using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFire : MonoBehaviour
{
    protected Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Destroy(this.gameObject, 5.0f);
    }

    private void Update()
    {
        Vector3 playerDirection = player.transform.localPosition - transform.localPosition;
        //Debug.Log("X side: " + playerDirection.x);
        if (playerDirection.x > 0 )
        {
            transform.Translate(Vector3.right * 3 * Time.deltaTime);
        }
        else if (playerDirection.x < 0)
        {
            transform.Translate(Vector3.left * 3 * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            IDamagable hit = other.GetComponent<IDamagable>();

            if(hit != null)
            {
                hit.Damage(20);
                Destroy(this.gameObject);
            }
        }        


    }
}
