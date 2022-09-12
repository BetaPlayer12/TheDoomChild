using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    [System.Serializable]
    public class BodyPartFXCreation : IStatusEffectModule
    {
        [SerializeField]
        private BodyReference.BodyPart m_attachTo;
        [SerializeField]
        private GameObject m_fx;

        private Dictionary<int, GameObject> m_fxTracker;
        private FXSpawnHandle<FX> m_fXSpawnHandle;

        public IStatusEffectModule GetInstance() => this;

        public void Start(Character character)
        {
            var bodypart = character.GetBodyPart(m_attachTo);
            var instance = m_fXSpawnHandle.InstantiateFX(m_fx, bodypart.position).gameObject;
            instance.transform.parent = bodypart;
            if (m_fxTracker == null)
            {
                m_fxTracker = new Dictionary<int, GameObject>();
            }
            character.InstanceDestroyed += OnInstanceDestroyed;
            m_fxTracker.Add(character.GetInstanceID(), instance);
        }

        public void Stop(Character character)
        {
            var instanceID = character.GetInstanceID();
            character.InstanceDestroyed -= OnInstanceDestroyed;
            RemoveFXForInstance(instanceID);
        }

        private void RemoveFXForInstance(int instanceID)
        {
            if (m_fxTracker.ContainsKey(instanceID))
            {
                UnityEngine.Object.Destroy(m_fxTracker[instanceID]);
                m_fxTracker.Remove(instanceID);
            }
        }

        private void OnInstanceDestroyed(object sender, ObjectIDEventArgs eventArgs)
        {
            RemoveFXForInstance(eventArgs.ID);
        }

       
    }
}