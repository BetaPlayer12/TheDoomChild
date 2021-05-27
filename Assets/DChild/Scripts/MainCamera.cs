using DChild.Gameplay;
using UnityEngine;

namespace DChild
{
    [AddComponentMenu("DChild/Misc/Main Camera")]
    public class MainCamera : MonoBehaviour
    {
        private void Awake()
        {
            var camera = GetComponent<Camera>();
            GameSystem.SetCamera(camera);
            GameplaySystem.cinema?.SetMainCamera(camera);
        }

        private void OnDestroy()
        {
            var camera = GetComponent<Camera>();
            if (GameSystem.mainCamera == camera)
            {
                GameSystem.SetCamera(null);
            }
            var cinema = GameplaySystem.cinema;
            if (cinema != null && cinema.mainCamera == camera)
            {
                cinema.SetMainCamera(null);
            }
        }
    }
}