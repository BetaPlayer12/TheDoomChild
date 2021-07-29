using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Environment
{
    public abstract class BaseLightIntensityAdjuster : MonoBehaviour
    {
        public abstract void SetIntensity(float intensitypercent);
    }
}
