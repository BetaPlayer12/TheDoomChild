using UnityEngine;
using System.Collections.Generic;
using DChild.Serialization;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    public class SprocketMovementHandle : MonoBehaviour, ISerializableComponent
    {
        private class SaveData : ISaveData
        {
            private SerializedVector2[] m_positions;

            public SaveData(Transform[] transforms)
            {
                if (transforms != null)
                {
                    m_positions = new SerializedVector2[transforms.Length];
                    for (int i = 0; i < m_positions.Length; i++)
                    {
                        m_positions[i] = (Vector2)transforms[i].position;
                    }
                }
            }

            public Vector2 GetSavedPosition(int index) => m_positions[index];

            public ISaveData ProduceCopy()
            {
                throw new System.NotImplementedException();
            }
        }

        [SerializeField, TabGroup("Platforms"), HideLabel]
        private LinkedPlatformMovementHandle m_platformMovementHandle;
        [SerializeField, TabGroup("Mechanism"), HideLabel]
        private SprocketMechanismMovementHandle m_mechanismMovementHandle;

        public ISaveData Save()
        {
            return new SaveData(m_platformMovementHandle.GetPlatforms());
        }

        public void Load(ISaveData data)
        {
            var saveData = (SaveData)data;
            for (int i = 0; i < m_platformMovementHandle.platformCount; i++)
            {
                m_platformMovementHandle.SetPlatformPosition(i, saveData.GetSavedPosition(i));
            }
            m_platformMovementHandle.RecordTrackedPlatformPositions();
        }

        private void Awake()
        {
            m_platformMovementHandle.Initialize();
        }


        private void LateUpdate()
        {
            m_platformMovementHandle.HandleLinkedMovement();
            if (m_platformMovementHandle.hasMovedOnLastLinkedMovement)
            {
                var speed = m_platformMovementHandle.movementVelocityOnLastLinkedMovement.magnitude;
                if (m_platformMovementHandle.wasLastMovementOverallNegative)
                {
                    speed *= -1;
                }
                m_mechanismMovementHandle.MoveChains(speed);
            }
        }
    }
}