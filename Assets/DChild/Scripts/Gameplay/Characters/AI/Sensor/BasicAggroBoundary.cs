using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    public class BasicAggroBoundary : AggroBoundary
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Sensor")
            {
                var target = collision.GetComponentInParent<ITarget>();
                if (target != null)
                {
                    Debug.Log("check target");
                    SetTargetToBrain(collision, target);
                } 
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag != "Sensor")
            {
                var target = collision.GetComponentInParent<ITarget>();
                if (target != null)
                {
                    m_brain.SetTarget(null);
                }
            }
        }
    }
}