using UnityEngine;

namespace DChild.Gameplay
{
    public class SystemController : MonoBehaviour
    {
        public void EnableCameraShake(bool shakeCamera)
        {
            GameplaySystem.cinema.EnableCameraShake(shakeCamera);
        }
    }
}