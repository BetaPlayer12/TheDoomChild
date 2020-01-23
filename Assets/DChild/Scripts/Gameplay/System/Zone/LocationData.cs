using DChild.Gameplay.Environment;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Systems.Serialization
{
    public class CharacterEventArgs : IEventActionArgs
    {
        private Character m_character;

        public Character character => m_character;

        public void SetCharacter(Character character)
        {
            m_character = character;
        }
    }

    [CreateAssetMenu(fileName = "LocationData", menuName = "DChild/Gameplay/Location Data")]
    public class LocationData : ScriptableObject
    {
        [SerializeField]
        private Location m_location;
        [SerializeField]
        private SceneInfo m_scene;
        [SerializeField]
        private Vector2 m_position;

        public Location location => m_location;
        public string scene => m_scene.sceneName;
        public Vector2 position => m_position;


#if UNITY_EDITOR
        public void Set(Scene scene, Vector2 position)
        {
            m_scene.Set(scene);
            m_position = position;
        }
#endif

        public event EventAction<CharacterEventArgs> OnArrival;

        public void CallArriveEvent(Character character)
        {
            using (Cache<CharacterEventArgs> cacheEventArgs = Cache<CharacterEventArgs>.Claim())
            {
                cacheEventArgs.Value.SetCharacter(character);
                OnArrival?.Invoke(this, cacheEventArgs);
                cacheEventArgs.Release();
            }
        }
    }
}
