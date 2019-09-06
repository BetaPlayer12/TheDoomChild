using DChild.Gameplay.Environment;
using Holysoft.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Systems.Serialization
{
    [CreateAssetMenu(fileName = "LocationData", menuName = "DChild/Gameplay/Location Data")]
    public class LocationData : ScriptableObject
    {
        public enum ExitDirection
        {
            Top,
            Bottom,
            Left,
            Right,
        }

        [SerializeField]
        private Location m_location;
        [SerializeField]
        private SceneInfo m_scene;
        [SerializeField]
        private Vector2 m_position;
        [SerializeField]
        private ExitDirection m_exitDirection;

        public Location location => m_location;
        public string scene => m_scene.sceneName;
        public Vector2 position => m_position;
        public ExitDirection exitDirection => m_exitDirection;


#if UNITY_EDITOR
        public void Set(Scene scene, Vector2 position)
        {
            m_scene.Set(scene);
            m_position = position;
        }
#endif
    }
}
