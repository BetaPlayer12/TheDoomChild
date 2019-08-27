using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [CreateAssetMenu(fileName = "SurfaceData", menuName = "DChild/Gameplay/Surface Data")]
    public class SurfaceData : SerializedScriptableObject
    {
        public enum FXType
        {
            Jump,
            Land,
            FootSteps,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, HideReferenceObjectPicker]
        private Dictionary<FXType, GameObject> m_list = new Dictionary<FXType, GameObject>();

        public GameObject GetFX(FXType type) => m_list[type];

        [Button,PropertyOrder(-1)]
        private void ResetValues()
        {
            m_list.Clear();
            var size = (int)FXType._COUNT;
            for (int i = 0; i < size; i++)
            {
                m_list.Add((FXType)i, null);
            }
        }
    }
}
