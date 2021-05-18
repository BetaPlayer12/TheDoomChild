using DChild.Gameplay.Characters.Players.Modules;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class IntervalIllusionPlatformHandle : MonoBehaviour
    {
        [SerializeField]
        private IllusionPlatform[] m_list;
        private int m_currentSequenceIndex;

        private void OnPlayerJumpExecution(object sender, EventActionArgs eventArgs)
        {
            var newIndex = (int)Mathf.Repeat(m_currentSequenceIndex + 1, m_list.Length);
            RevealPlatformsAtConfiguration(newIndex);
            m_currentSequenceIndex = newIndex;
        }

        private void RevealPlatformsAtConfiguration(int index)
        {
            m_list[m_currentSequenceIndex]?.Disappear(false);
            m_list[index]?.Appear(false);
        }

        private void Reset()
        {
            m_currentSequenceIndex = 0;
            m_list[0].Appear(true);
            for (int i = 1; i < m_list.Length; i++)
            {
                m_list[i]?.Disappear(true);
            }
        }

        private void Start()
        {
            var character = GameplaySystem.playerManager.player.character;
            character.GetComponentInChildren<WallJump>().ExecuteModule += OnPlayerJumpExecution;
            character.GetComponentInChildren<GroundJump>().ExecuteModule += OnPlayerJumpExecution;
            character.GetComponentInChildren<ExtraJump>().ExecuteModule += OnPlayerJumpExecution;
            Reset();
        }

        private void OnDestroy()
        {
            var character = GameplaySystem.playerManager.player.character;
            character.GetComponentInChildren<WallJump>().ExecuteModule -= OnPlayerJumpExecution;
            character.GetComponentInChildren<GroundJump>().ExecuteModule -= OnPlayerJumpExecution;
            character.GetComponentInChildren<ExtraJump>().ExecuteModule -= OnPlayerJumpExecution;
        }
    }
}
