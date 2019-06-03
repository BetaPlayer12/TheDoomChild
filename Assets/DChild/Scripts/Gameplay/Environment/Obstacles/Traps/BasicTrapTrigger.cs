using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class BasicTrapTrigger : MonoBehaviour
    {
        private ITrap m_trap;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            m_trap.TriggerTrap();
        }
    }
}