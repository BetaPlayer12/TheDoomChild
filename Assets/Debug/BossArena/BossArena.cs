using Cinemachine;
using DChild.Gameplay;
using DChild.Gameplay.Characters.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Testing
{

    public class BossArena : MonoBehaviour
    {
        [SerializeField]
        private Transform m_playerStartPosition;
        [SerializeField]
        private CinemachineVirtualCamera m_camera;
        [SerializeField]
        private Boss m_boss;

        public void PrepareFight()
        {
            var player = GameplaySystem.playerManager.player;
            player.character.transform.position = m_playerStartPosition.position;
            GameplaySystem.gamplayUIHandle.MonitorBoss(m_boss);
            m_camera.gameObject.SetActive(true);
            m_boss.gameObject.SetActive(true);
            m_boss.SetTarget(player.damageableModule, player.character);
            m_boss.Disable();
        }

        public void StartFight()
        {
            var player = GameplaySystem.playerManager.player;
            m_boss.Enable();
        }

        public void StopFight()
        {
            m_boss.Disable();
            m_boss.gameObject.SetActive(false);
        }

        public void CleanUpFight()
        {
            m_camera.gameObject.SetActive(false);
        }

        private void Awake()
        {
            m_camera.gameObject.SetActive(false);
            m_boss.gameObject.SetActive(false);
        }

        private void OnValidate()
        {
            if (Application.isEditor == false || Application.isPlaying)
            {
                m_camera.gameObject.SetActive(false);
                m_boss.gameObject.SetActive(false);
            }
        }
    }

}