using DChild;
using DChild.Gameplay;
using Doozy.Engine;
using Holysoft.Event;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChildDebug.Testing
{
    public class BossArenaSystem : MonoBehaviour
    {
        private BossArena m_arena;
        [SerializeField]
        private GameplaySignalHandle m_signalHandle;
        [SerializeField]
        private float m_gameOverTime;

        public void Set(BossArena arena)
        {
            m_arena = arena;
        }

        public void StartFight()
        {
            m_arena.StartFight();
        }
        public void StopFight()
        {
            m_arena.StopFight();
        }
        public void CleanUpFight()
        {
            m_arena.CleanUpFight();
            SceneManager.LoadScene(gameObject.scene.name);
        }

        public void BlockInput()
        {
            m_signalHandle.OverridePlayerControl();
            enabled = true;
        }
        private void OnPlayeDeath(object sender, EventActionArgs eventArgs)
        {
            StartCoroutine(GameOverTimeRoutine());
        }

        private IEnumerator GameOverTimeRoutine()
        {
            yield return new WaitForSeconds(m_gameOverTime);
            SceneManager.LoadScene(gameObject.scene.name);
        }

        private void Start()
        {
            GameplaySystem.playerManager.player.damageableModule.Destroyed += OnPlayeDeath;
            enabled = false;
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                m_signalHandle.StopPlayerControlOverride();
                GameEventMessage.SendEvent("Release Controls");
                enabled = false;
            }
        }
    }

}