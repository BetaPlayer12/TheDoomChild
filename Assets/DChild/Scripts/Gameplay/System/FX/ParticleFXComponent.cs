using UnityEngine;

namespace DChild.Gameplay
{
    public abstract class ParticleFXComponent : MonoBehaviour
    {
        public abstract void Initialize();
        public abstract void SetActive(bool isActive);
        public abstract void Stop();
        public abstract void Reset();
    }
}