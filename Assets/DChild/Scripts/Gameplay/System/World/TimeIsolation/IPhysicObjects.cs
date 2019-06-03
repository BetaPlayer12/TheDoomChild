using UnityEngine;

namespace DChild.Gameplay
{
    public interface IPhysicObjects
    {
        void CalculateActualVelocity(float timeScale);
        void AlignTime(float timeScale);
        void Revert();
    }

}