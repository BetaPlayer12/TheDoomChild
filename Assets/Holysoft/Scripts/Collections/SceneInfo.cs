using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Holysoft.Collections
{
    [System.Serializable]
    public class SceneInfo
    {
        [SerializeField]
        private string m_sceneName;
        [SerializeField]
        private string m_scenePath;
        [SerializeField]
        private bool m_isAddressables;

        public string sceneName
        {
            get => m_sceneName;
#if UNITY_EDITOR
            set => m_sceneName = value;
#endif
        }

        public string scenePath
        {
            get => m_scenePath;

#if UNITY_EDITOR
            set => m_scenePath = value;
#endif
        }

        public bool isAddressables
        {
            get => m_isAddressables;

#if UNITY_EDITOR
            set => m_isAddressables = value;
#endif
        }

        public SceneInfo(Scene scene)
        {
            m_sceneName = scene.name;
            m_scenePath = scene.path;
        }

        public void Set(Scene scene)
        {
            m_sceneName = scene.name;
            m_scenePath = scene.path;
        }
    }
}
