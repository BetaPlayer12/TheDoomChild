/***************************************
 * 
 * This class is use to isolate Time to each object
 * 
 ***************************************/

using UnityEngine;

namespace DChild.Gameplay.Systems.WorldComponents
{
    public interface IIsolatedPhysicsTime
    {
        void CalculateActualVelocity();
        Vector2 GetRelativeForce(Vector2 force);
        float GetRelativeForce(float force);
    }
}
