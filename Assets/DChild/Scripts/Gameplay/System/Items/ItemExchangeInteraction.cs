using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment.Interractables;
using DChild.Gameplay.Systems;
using DChild.Serialization;
using DChild.Temp;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Items
{
    public class ItemExchangeInteraction : MonoBehaviour, IButtonToInteract, IInteractionRequirement
    {
        [InfoBox("When interacted, it will take soul essence when it has not reached the amount, it will give soul essence whn it has reached the amount")]
        [SerializeField, MinValue(1), OnValueChanged("OnAmountChanged")]
        private int m_amountRequired;
        [SerializeField, MinValue(0), MaxValue("$m_amountRequired"), OnValueChanged("OnAmountChanged")]
        private int m_currentAmount;
        [SerializeField]
        private Vector3 m_promptOffset;

       
        [TabGroup("Main", "Transistion")]
        [SerializeField]
        private UnityEvent m_onComplete;
       

        private Collider2D m_trigger;
        private static GameplayUIHandle m_gameplayUIHandle;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        public bool showPrompt => true;

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public string promptMessage => (m_currentAmount == m_amountRequired ? "Take" : "Give") + $" <sprite name=\"SoulEssenceIcon_TMP\">{m_amountRequired}";

        public string requirementMessage => $"Need <sprite name=\"SoulEssenceIcon_TMP\">{m_amountRequired}";

        public bool CanBeInteracted(Character character)
        {
            var inventory = character.GetComponent<PlayerControlledObject>().owner.inventory;
            return inventory.currency >= m_amountRequired;
        }

        public void Interact(Character character)
        {
            var inventory = character.GetComponent<PlayerControlledObject>().owner.inventory;
            if (m_currentAmount == m_amountRequired)
            {
                inventory.AddSoulEssence(m_currentAmount);
                m_currentAmount = 0;
                m_onComplete?.Invoke();
                StopAllCoroutines();
                StartCoroutine(DelayedReenableTrigger());
            }
            else
            {
                var getAmount = Mathf.Min(inventory.currency, m_amountRequired - m_currentAmount);
                m_currentAmount += getAmount;
                inventory.AddSoulEssence(-getAmount);
                if (m_currentAmount == m_amountRequired)
                {
                    m_onComplete?.Invoke();
                    StopAllCoroutines();
                    StartCoroutine(DelayedReenableTrigger());
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(DelayedReenableTrigger());
                }
            }
        }

       
        public void Initialize()
        {
            m_currentAmount = 0;
        }
        private IEnumerator DelayedReenableTrigger()
        {
            m_trigger.enabled = false;
            yield return new WaitForWorldSeconds(1);
            m_trigger.enabled = true;
        }

        private void Awake()
        {
            m_trigger = GetComponentInChildren<Collider2D>();
        }

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }

#if UNITY_EDITOR
        private void OnAmountChanged()
        {
            if (m_amountRequired <= m_currentAmount)
            {
                m_onComplete?.Invoke();
            }
           
        }

#endif
    }
}
