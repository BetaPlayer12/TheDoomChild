using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.UI;
using Doozy.Runtime.Signals;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace DChild.Gameplay.UI
{
    public class KingPusUIHandle : MonoBehaviour
    {
        [SerializeField]
        private KingPusAI m_kingPusAI;
        [SerializeField]
        private BossCombatUI m_bossCombatUI;

        public event EventAction<EventActionArgs> HideHealthUI;

        void Start()
        {
            m_kingPusAI.PhaseChangeStart += OnPhaseChangeStart;
            m_kingPusAI.PhaseChangeDone += OnPhaseChangeDone;

            m_bossCombatUI = FindObjectOfType<BossCombatUI>();
        }

        private void OnPhaseChangeStart(object sender, EventActionArgs eventArgs)
        {
            m_bossCombatUI.HideBossHealth();
            HideHealthUI.Invoke(this, eventArgs);
        }

        private void OnPhaseChangeDone(object sender, EventActionArgs eventArgs)
        {
            m_bossCombatUI.ShowBossHealth();
        }
    }
}

