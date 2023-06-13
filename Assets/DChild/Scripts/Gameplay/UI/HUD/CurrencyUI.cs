﻿using DChild.Gameplay.Systems;
using DChild.Temp;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class CurrencyUI : SerializedMonoBehaviour
    {
        [SerializeField]
        private UIContainer m_addedCurrencyContainer;
        [SerializeField]
        private ICurrency m_currency;
        [SerializeField]
        private TextMeshProUGUI m_currentAmountText;
        [SerializeField]
        private TextMeshProUGUI m_addedAmountText;

        [SerializeField]
        private float m_addAmountDelay;
        [SerializeField, MinValue(0)]
        private float m_updateRateFactor;
        [SerializeField, MinValue(0)]
        private float m_increaseUpdateRateInterval = 1;
        [SerializeField, MinValue(0)]
        private float m_maxUpdateIncreaseStack = 1;


        private int m_currentAmount;
        private int m_addedAmount;
        private bool m_addedAmountIsNegative;
        private float m_delayTimer;
        private bool m_delayAddingOfAmount;
        private int m_updateRateStack;
        private int m_currentUpdateFactor;
        private float m_updateRateIntervalTimer;


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
            m_currentAmount = eventArgs.amount;
        }

        private void OnAmountAdded(object sender, CurrencyUpdateEventArgs eventArgs)
        {
            addedAmount += eventArgs.amount;
            m_delayTimer = m_addAmountDelay;
            m_delayAddingOfAmount = true;
            m_addedCurrencyContainer.Show(true);
            enabled = true;
            m_updateRateStack = 0;
            m_currentUpdateFactor = 1;
            m_updateRateIntervalTimer = m_increaseUpdateRateInterval;
        }

        private void CapCurrentUpdateFactor()
        {
            var simulatedAddedAmount = addedAmount;
            if (m_addedAmountIsNegative)
            {
                simulatedAddedAmount += m_currentUpdateFactor;
                if (simulatedAddedAmount > 0)
                {
                    m_currentUpdateFactor -= simulatedAddedAmount;
                }
            }
            else
            {
                simulatedAddedAmount -= m_currentUpdateFactor;
                if (simulatedAddedAmount < 0)
                {
                    m_currentUpdateFactor += simulatedAddedAmount;
                }
            }
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
            if (GameplaySystem.isGamePaused == false)
            {
                var deltaTime = Time.unscaledDeltaTime;
                if (m_delayAddingOfAmount)
                {
                    m_delayTimer -= deltaTime;
                    if (m_delayTimer <= 0)
                    {
                        m_delayAddingOfAmount = false;
                    }
                }
                else
                {
                    bool stopUpdate = false;
                    CapCurrentUpdateFactor();
                    if (m_addedAmountIsNegative)
                    {
                        addedAmount += m_currentUpdateFactor;
                        currentAmount -= m_currentUpdateFactor;
                        stopUpdate = addedAmount >= 0;
                    }
                    else
                    {
                        addedAmount -= m_currentUpdateFactor;
                        currentAmount += m_currentUpdateFactor;
                        stopUpdate = addedAmount <= 0;
                    }

                    if (stopUpdate)
                    {
                        enabled = false;
                        m_addedCurrencyContainer.Hide();
                    }
                }

                if (m_updateRateStack < m_maxUpdateIncreaseStack)
                {
                    m_updateRateIntervalTimer -= deltaTime;
                    if (m_updateRateIntervalTimer < 0)
                    {
                        m_currentUpdateFactor += Mathf.CeilToInt(Mathf.Pow(m_updateRateFactor, m_updateRateStack));
                        m_updateRateStack++;
                        m_updateRateIntervalTimer = m_increaseUpdateRateInterval;
                    }
                }
            }
        }
    }
}