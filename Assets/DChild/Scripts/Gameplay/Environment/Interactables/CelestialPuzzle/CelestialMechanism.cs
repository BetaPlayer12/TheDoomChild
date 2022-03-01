using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class CelestialMechanism : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public class SaveData : ISaveData
        {
            private SerializeID[] m_slotIDs;
            private bool[] m_slotStates;

            public int m_numberOfActivatedSlots;

            public SaveData(IEnumerable<SerializeID> slotIDs, IEnumerable<bool> slotStates, int numberOfActivatedSlots)
            {
                m_slotIDs = slotIDs.ToArray();
                m_slotStates = slotStates.ToArray();
                m_numberOfActivatedSlots = numberOfActivatedSlots;
            }

            public int slotCount => m_slotIDs.Length;
            public int numberOfActivatedSlots => m_numberOfActivatedSlots;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_slotIDs,m_slotStates,m_numberOfActivatedSlots);

            public SerializeID GetID(int index) => m_slotIDs[index];
            public bool GetState(int index) => m_slotStates[index];
        }

        [TabGroup("Start", "Reference")]
        [BoxGroup("Start/Reference/Box")]
        [SerializeField, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, DraggableItems = false, Expanded = true), HorizontalGroup("Start/Reference/Box/Split")]
        private List<CelestialSlot> m_slots = new List<CelestialSlot>(new CelestialSlot[1]);

        [SerializeField, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, Expanded = true), HorizontalGroup("Start/Reference/Box/Split")]
        private List<CelestialIndicator> m_activationIndicators = new List<CelestialIndicator>(new CelestialIndicator[1]);

        [TabGroup("Start", "Events")]
        [SerializeField, TabGroup("Start/Events", "Completed")]
        private UnityEvent m_completedEvent;
        [SerializeField, TabGroup("Start/Events", "Transistion")]
        private UnityEvent m_transistionToCompleteEvent;
        [SerializeField, TabGroup("Start/Events", "Incomplete")]
        private UnityEvent m_incompleteEvent;

        private bool m_readyActivate;
        private int m_activatedSlots;
        

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(-1), MinValue(1), OnValueChanged("OnSizeChange")]
        public int m_requiredSlots = 1;

        private void OnSizeChange()
        {
            if (m_slots.Count > m_requiredSlots)
            {
                
                for (int i = m_slots.Count - 1; i >= m_requiredSlots; i--)
                {
                    m_slots.RemoveAt(i);
                    m_activationIndicators.RemoveAt(i);
                }
            }
            else
            {
                var missingCount = m_requiredSlots - m_slots.Count;
                for (int i = 0; i < missingCount; i++)
                {
                    m_slots.Add(null);
                    m_activationIndicators.Add(null);
                }
            }
        }
#endif

        public void Load(ISaveData data)
        {
            var saveData = (SaveData)data;

            for (int i = 0; i < saveData.slotCount; i++)
            {
                var serializeID = saveData.GetID(i);
                for (int j = 0; j < m_slots.Count; j++)
                {
                    if (m_slots[j].ID == serializeID)
                    {
                        //Change Slot State;
                    }
                }
            }

            for (int i = 0; i < m_activationIndicators.Count; i++)
            {
                m_activationIndicators[i].SetState(i < saveData.numberOfActivatedSlots);
            }

            if (m_slots.Count == saveData.numberOfActivatedSlots)
            {
                m_completedEvent?.Invoke();
            }
            else
            {
                m_incompleteEvent?.Invoke();
            }
        }

        public ISaveData Save() => new SaveData(m_slots.Select(x => x.ID), m_slots.Select(x => x.isOccupied), m_activatedSlots);
        public void Initialize()
        {
            for (int i = 0; i < m_activationIndicators.Count; i++)
            {
                m_activationIndicators[i].SetState(false);
            }
            m_incompleteEvent?.Invoke();
        }
        private void OnSlotStateChange(object sender, EventActionArgs eventArgs)
        {
            var slot = (CelestialSlot)sender;
            m_activatedSlots += slot.isOccupied ? 1 : -1;
            if (m_activatedSlots == m_slots.Count)
            {
                m_readyActivate = true;
                for (int i = 0; i < m_slots.Count; i++)
                {
                    m_slots[i].SetLockDown(true);
                }
                m_transistionToCompleteEvent?.Invoke();
            }
            else
            {
                for (int i = 0; i < m_slots.Count; i++)
                {
                    m_slots[i].SetLockDown(false);
                }
            }
            for (int i = 0; i < m_activationIndicators.Count; i++)
            {
                m_activationIndicators[i].SetState(i < m_activatedSlots);
            }
        }

        private void Awake()
        {
            for (int i = 0; i < m_slots.Count; i++)
            {
                m_slots[i].StateChange += OnSlotStateChange;
            }
        }
    }
}
