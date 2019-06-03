using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.Collections
{
    [System.Serializable]
    public class SceneInfo
    {
        [SerializeField]
        private string m_sceneName;
        [SerializeField]
        private string m_scenePath;

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
    }
}
