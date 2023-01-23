#if UNITY_EDITOR
using Cinemachine;
using DChild.Gameplay.Cinematics.Cameras;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChildEditor
{
    [System.Serializable]
    public class VirtualCameraPostProcessor : IScenePostProcessor
    {
        [SerializeField, ValueDropdown("GetAllVirtualCameras", IsUniqueList = true)]
        private VirtualCamera[] virtualCameras;

        public VirtualCameraPostProcessor(VirtualCamera[] virtualCameras)
        {
            this.virtualCameras = virtualCameras;
        }

        public void Execute()
        {
            for (int i = 0; i < virtualCameras.Length; i++)
            {
                virtualCameras[i].GetComponentInChildren<CinemachineVirtualCamera>(true).enabled = false;
            }
        }

        private IEnumerable GetAllVirtualCameras()
        {
            ValueDropdownList<VirtualCamera> valueDropdowns = new ValueDropdownList<VirtualCamera>();

            var references = Object.FindObjectsOfType<VirtualCamera>(true);
            for (int i = 0; i < references.Length; i++)
            {
                var reference = references[i];
                valueDropdowns.Add("All/" + reference.gameObject.name, reference);
            }
            return valueDropdowns;
        }
    }
}
#endif