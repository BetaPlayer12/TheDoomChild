using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public class TukkoBomb : FragileBomb
    {
        [SerializeField]
        private float m_travelSpeed;
        //[SerializeField]
        //private GameObject m_deezNuts;

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    //Debug.Log("OHOO");
        //    GameObject ohoo = Instantiate(m_deezNuts, transform.position, Quaternion.identity);
        //    Destroy(this.gameObject);
        //}

        protected override void Awake()
        {
            base.Awake();
            m_rigidbody = GetComponent<Rigidbody2D>();
            GetComponent<Rigidbody2D>().AddForce(transform.right * m_travelSpeed, ForceMode2D.Impulse);
        }
    }
}
