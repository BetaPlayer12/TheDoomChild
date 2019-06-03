/*************************
 * 
 * This is to seperate Character Physics to Object Physics completely
 * 
 *************************/

namespace DChild.Gameplay
{
    public abstract class ObjectPhysics2D : IsolatedPhysics2D
    {
        public bool IsSleeping() => m_rigidbody2D.IsSleeping();
    }
}
