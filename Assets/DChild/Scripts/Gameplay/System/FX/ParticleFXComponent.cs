using UnityEngine;

namespace DChild.Gameplay
{
    public interface ParticleFXComponent
    {
       void Initialize();
       void SetActive(bool isActive);
       void Stop();
       void Reset();
    }
}