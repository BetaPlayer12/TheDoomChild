using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DChild.Serialization;
using DChild.Gameplay.Environment.Interractables;
using DChild.Gameplay.Characters.Players;

namespace DChild.Gameplay.Environment
{
    public class KeystoneCollector : MonoBehaviour, IButtonToInteract, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField, MinValue(0)]
            private int m_currentKeyStones;

            public SaveData(int currentKeyStones)
            {
                m_currentKeyStones = currentKeyStones;
            }

            public int currentKeyStones => m_currentKeyStones;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_currentKeyStones);
        }

        [SerializeField]
        private Vector3 m_promptOffset;

        [SerializeField]
        private ItemRequirement m_itemRequirement;
        [SerializeField, MinValue(1), OnValueChanged("UpdateReactionCount")]
        private int m_keyStonesNeeded = 1;
        [SerializeField, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        private AbyssmalGateSlot[] m_reactions = new AbyssmalGateSlot[1];

        private int m_currentKeyStones;

        public bool showPrompt => true;

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public string promptMessage => "Insert Keystone";

        public ISaveData Save() => new SaveData(m_currentKeyStones);

        public void Load(ISaveData data)
        {
            for (int i = 0; i < m_reactions.Length; i++)
            {
                m_reactions[i].SetActive(i < m_currentKeyStones);
            }
        }

        public void Interact(Character character)
        {
            var inventory = character.GetComponent<PlayerControlledObject>().owner.inventory;
            if (m_itemRequirement.HasAllItems(inventory))
            {
                m_itemRequirement.ConsumeItems(inventory);
                m_reactions[m_currentKeyStones].SetActive(true);
                m_currentKeyStones++;
            }
        }

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }

#if UNITY_EDITOR
        private void UpdateReactionCount()
        {
            var list = new AbyssmalGateSlot[m_keyStonesNeeded];
            for (int i = 0; i < list.Length; i++)
            {
                try
                {
                    list[i] = m_reactions[i];
                }
                catch (IndexOutOfRangeException)
                {
                    break;
                }
            }
            m_reactions = list;
        }
#endif
    }

}