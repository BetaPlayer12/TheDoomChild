/*************************
 * 
 * This is to seperate Character Physics to Object Physics completely
 * 
 *************************/

using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay
{
    public class CollisionEventActionArgs : IEventActionArgs
    {
        private Collision2D m_collision;

        public Collision2D collision => m_collision;

        public void Set(Collision2D collision)
        {
            m_collision = collision;
        }
    }
}
