using DChild.Gameplay.Systems;
using Doozy.Engine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class CurrencyUI : SerializedMonoBehaviour
    {
        [SerializeField]
        private ICurrency m_currency;
        [SerializeField]
        private TextMeshProUGUI m_currentAmountText;
        [SerializeField]
        private TextMeshProUGUI m_addedAmountText;

        [SerializeField]
        private float m_addAmountDelay;

        private int m_currentAmount;
        private int m_addedAmount;
        private bool m_addedAmountIsNegative;
        private float m_delayTimer;
        private bool m_delayAddingOfAmount;
        

        private int currentAmount
        {
            get => m_currentAmount;
            set
            {
                m_currentAmount = value;
                m_currentAmountText.text = m_currentAmount.ToString();
            }
        }

        private int addedAmount
        {
            get => m_addedAmount;
            set
            {
                m_addedAmount = value;
                m_addedAmountIsNegative = addedAmount < 0;
                if (m_addedAmountIsNegative)
                {
                    m_addedAmountText.text = m_addedAmount.ToString();
                }
                else
                {
                    m_addedAmountText.text = "+" + m_addedAmount.ToString();
                }

            }
        }

        public void Monitor(ICurrency currency)
        {
            if (m_currency != null)
            {
                m_currency.OnAmountAdded -= OnAmountAdded;
                m_currency.OnAmountSet -= OnAmountSet;
            }
            m_currency = currency;
            currentAmount = m_currency.amount;
            m_currency.OnAmountAdded += OnAmountAdded;
            m_currency.OnAmountSet += OnAmountSet;
        }

        private void OnAmountSet(object sender, CurrencyUpdateEventArgs eventArgs)
        {
            m_currentAmountText.text = eventArgs.amount.ToString();
            // GameEventMessage.SendEvent("Soul Essence Notify");
        }

        private void OnAmountAdded(object sender, CurrencyUpdateEventArgs eventArgs)
        {
            addedAmount += eventArgs.amount;
            m_delayTimer = m_addAmountDelay;
            m_delayAddingOfAmount = true;
            GameplaySystem.gamplayUIHandle.ShowPromptSoulEssenceChangeNotify();
            enabled = true;
        }

        private void Awake()
        {
            if (m_currency != null)
            {
                Monitor(m_currency);
            }
            enabled = false;
        }

        private void LateUpdate()
        {
            if (m_delayAddingOfAmount)
            {
                m_delayTimer -= Time.unscaledDeltaTime;
                if (m_delayTimer <= 0)
                {
                    m_delayAddingOfAmount = false;
                }
            }
            else
            {
                if (m_addedAmountIsNegative)
                {
                    addedAmount += 1;
                    currentAmount -= 1;
                }
                else
                {
                    addedAmount -= 1;
                    currentAmount += 1;
                }

                if (addedAmount == 0)
                {
                    enabled = false;
                    GameplaySystem.gamplayUIHandle.ShowSoulEssenceNotify(false);
                }
            }
        }
    }
}