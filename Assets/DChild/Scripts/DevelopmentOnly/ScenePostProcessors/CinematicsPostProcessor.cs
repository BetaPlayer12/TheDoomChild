#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace DChildEditor
{
    [System.Serializable]
    public class CinematicsPostProcessor : IScenePostProcessor
    {
        [SerializeField, ValueDropdown("GetAllPlayableDirector", IsUniqueList = true)]
        private PlayableDirector[] m_cinematics;

        public void Execute()
        {
            for (int i = 0; i < m_cinematics.Length; i++)
            {
                m_cinematics[i].playOnAwake = false;
            }
        }

        private IEnumerable GetAllPlayableDirector()
        {
            return Object.FindObjectsOfType<PlayableDirector>();
        }
    }

}
#endif