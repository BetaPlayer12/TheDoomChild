using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public class FragileBomb : Bomb
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Detonate(collision.contacts[0].point);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(CollidedWithSensor(collision) == false)
            {
                Detonate(transform.position);
            }
        }
    }
}