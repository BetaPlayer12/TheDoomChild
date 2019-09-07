using UnityEngine;

namespace DChild
{
    public class MainCamera : MonoBehaviour
    {
        private void Awake()
        {
            GameSystem.SetCamera(GetComponent<Camera>());
        }
    }
}