using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment.Interractables;
using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class SoulEssenceOffering : MonoBehaviour, IButtonToInteract, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private int m_currentAmount;

            public SaveData(int currentAmount)
            {
                m_currentAmount = currentAmount;
            }

            public int currentAmount => m_currentAmount;
        }

        [InfoBox("When interacted, it will take soul essence when it has not reached the amount, it will give soul essence whn it has reached the amount")]
        [SerializeField, MinValue(1)]
        private int m_amountRequired;
        [SerializeField, MinValue(0),MaxValue("$m_amountRequired")]
        private int m_currentAmount;
        [SerializeField]
        private Transform m_promptPosition;

        [TabGroup("Main", "StartAs")]
        [SerializeField, TabGroup("Main/StartAs","Complete")]
        private UnityEvent m_startAsComplete;
        [SerializeField, TabGroup("Main/StartAs", "Incomplete")]
        private UnityEvent m_startAsIncomplete;

        [TabGroup("Main", "Transistion")]
        [SerializeField, TabGroup("Main/Transistion","Complete")]
        private UnityEvent m_onComplete;
        [SerializeField, TabGroup("Main/Transistion", "Incomplete")]
        private UnityEvent m_onIncomplete;

        public bool showPrompt => true;

        public Vector3 promptPosition => m_promptPosition.position;

        public void Interact(Character character)
        {
            var inventory = character.GetComponent<PlayerControlledObject>().owner.inventory;
            if (m_currentAmount == m_amountRequired)
            {
                var getAmount = Mathf.Min(inventory.soulEssence, m_amountRequired - m_currentAmount);
                m_currentAmount += getAmount;
                inventory.AddSoulEssence(-getAmount);
                if (m_currentAmount == m_amountRequired)
                {
                    m_onComplete?.Invoke();
                }
                else
                {
                    m_onIncomplete?.Invoke();
                }
            }
            else
            {
                inventory.AddSoulEssence(m_currentAmount);
                m_currentAmount = 0;
                m_onIncomplete?.Invoke();
            }
        }

        public void Load(ISaveData data)
        {
            m_currentAmount = ((SaveData)data).currentAmount;
            if (m_currentAmount == m_amountRequired)
            {
                m_startAsComplete?.Invoke();
            }
            else
            {
                m_startAsIncomplete?.Invoke();
            }
        }

        public ISaveData Save()
        {
            return new SaveData(m_currentAmount);
        }
    }
}