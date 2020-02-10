using DChild.Gameplay.Databases;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SurfaceData", menuName = "DChild/Database/Surface Data")]
    [ShowOdinSerializedPropertiesInInspector]
    public class SurfaceData : ScriptableDatabase, ISerializationCallbackReceiver
    {
        [System.Serializable]
        public class FXGroup : EnumElement<SurfaceType>
        {
            [SerializeField, ValidateInput("ValidateFX")]
            private GameObject m_jumpFX;
            [SerializeField, ValidateInput("ValidateFX")]
            private GameObject m_landFX;
            [SerializeField, ValidateInput("ValidateFX")]
            private GameObject m_footStepFX;

            public GameObject jumpFX => m_jumpFX;
            public GameObject landFX => m_landFX;
            public GameObject footStepFX => m_footStepFX;

#if UNITY_EDITOR
            private bool ValidateFX(GameObject fx) => fx?.GetComponent<FX>() ?? true;
#endif
        }

        [SerializeField, ReadOnly, PropertyOrder(1)]
        private FXGroup[] m_fxGroups;

        public FXGroup GetFXGroup(SurfaceType type) => m_fxGroups[(int)type];

#if UNITY_EDITOR
        [NonSerialized, OdinSerialize, PropertyOrder(0), HideInPlayMode, HideReferenceObjectPicker, HideLabel]
        private EnumList<SurfaceType, FXGroup> m_surfaceInfoList = new EnumList<SurfaceType, FXGroup>();
        [SerializeField, HideInInspector]
        private SerializationData serializationData;

        private void OnDisable() => m_fxGroups = m_surfaceInfoList.ToArray();
#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData);
#endif
        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData);
#endif
        }
    }
}