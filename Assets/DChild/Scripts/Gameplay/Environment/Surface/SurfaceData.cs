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
        private Dictionary<FXType, GameObject> m_fXlist = new Dictionary<FXType, GameObject>();
        [SerializeField, HideReferenceObjectPicker]
        private Dictionary<FXType, AudioClip> m_audiolist = new Dictionary<FXType, AudioClip>();

        public GameObject GetFX(FXType type) => m_fXlist[type];
        public AudioClip GetAudio(FXType type) => m_audiolist[type];

        [Button, PropertyOrder(-1)]
        private void ResetValues()
        {
            m_fXlist.Clear();
            m_audiolist.Clear();
            var size = (int)FXType._COUNT;
            for (int i = 0; i < size; i++)
            {
                m_fXlist.Add((FXType)i, null);
                m_audiolist.Add((FXType)i, null);
            }
        }
    }
}
