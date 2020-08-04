using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using Holysoft.Gameplay;
using UnityEngine;
using Sirenix.OdinInspector;
using PlayerNew;
using System;

namespace DChild.Gameplay.Characters.Players
{
    public class MagicRegen : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private CountdownTimer m_regenInterval = new CountdownTimer(5);
        [SerializeField, MinValue(0)]
        private int m_passiveRegenRate;
        [SerializeField, MinValue(0)]
        private int m_onKillRegen;
        private ICappedStat m_magic;
        private bool m_isFull;
        private IIsolatedTime m_time;

        private PlayerNew.WallStick m_wallstickHandler;
        private StateManager m_collisionStateHandler;


        public void Initialize(ComplexCharacterInfo info)
        {
            var character = info.character;
            m_time = character.isolatedObject;
            m_magic = info.magic;
            m_isFull = m_magic.currentValue == m_magic.maxValue;
            m_magic.ValueChanged += OnValueChange;

            m_wallstickHandler = character.GetComponentInChildren<PlayerNew.WallStick>();
            m_collisionStateHandler = character.GetComponentInChildren<StateManager>();
            info.attacker.TargetDamaged += OnTargetDamaged;
        }

        private void OnTargetDamaged(object sender, CombatConclusionEventArgs eventArgs)
        {
            var target = eventArgs.target;
            if (target.isCharacter && target.instance.isAlive == false)
            {
                m_magic.AddCurrentValue(m_onKillRegen);
            }
        }

        public void SetPassiveRegenRate(int regenRate)
        {
            m_passiveRegenRate = regenRate;
        }

        private void OnValueChange(object sender, StatInfoEventArgs eventArgs)
        {
            m_isFull = m_magic.currentValue == m_magic.maxValue;
            if (m_isFull)
            {
                enabled = false;
                m_regenInterval.Reset();
            }
            else
            {
                enabled = true;
            }
        }

        private void OnIntervalEnd(object sender, EventActionArgs eventArgs)
        {
            m_regenInterval.Reset();
            m_magic.AddCurrentValue(m_passiveRegenRate);
            m_isFull = m_magic.currentValue == m_magic.maxValue;
            if (m_isFull)
            {
                enabled = false;
            }
        }

        private void Awake()
        {
            m_regenInterval.CountdownEnd += OnIntervalEnd;
            m_regenInterval.Reset();
            enabled = false;
        }

        private void Update()
        {
            if (m_collisionStateHandler.isGrounded || m_wallstickHandler.wallSticking)
            {
                m_regenInterval.Tick(m_time.deltaTime);
            }
        }

    }

}