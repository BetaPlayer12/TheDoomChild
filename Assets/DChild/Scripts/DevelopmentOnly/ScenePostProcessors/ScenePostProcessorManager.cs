#if UNITY_EDITOR
using Cinemachine;
using DChild.Gameplay.Cinematics.Cameras;
using DChild.Serialization;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChildEditor
{
    public class ScenePostProcessorManager : SerializedMonoBehaviour
    {
        [SerializeField]
        private IScenePostProcessor[] m_processors;

        public void ProcessScene()
        {
            for (int i = 0; i < m_processors.Length; i++)
            {
                m_processors[i].Execute();
            }
        }

        [Button, ShowIf("@m_processors == null || m_processors.Length == 0")]
        private void InitializeProcessors()
        {
            List<IScenePostProcessor> processors = new List<IScenePostProcessor>();

            processors.Add(new ZoneDataPostProcessor(FindObjectOfType<ZoneDataHandle>(true)));
            processors.Add(new MainCameraPostProcessor(FindObjectOfType<CinemachineBrain>().GetComponent<Camera>()));
            processors.Add(new VirtualCameraPostProcessor(FindObjectsOfType<VirtualCamera>(true)));

            m_processors = processors.ToArray();
        }
    }

}
#endif