using UnityEngine;

namespace DChild
{
    [AddComponentMenu("DChild/Misc/Main Camera")]
    public class MainCamera : MonoBehaviour
    {
        private void Awake()
        {
            GameSystem.SetCamera(GetComponent<Camera>());
        }

        private void OnDestroy()
        {
            if (GameSystem.mainCamera == GetComponent<Camera>())
            {
                GameSystem.SetCamera(null);
            }
        }
    }
}