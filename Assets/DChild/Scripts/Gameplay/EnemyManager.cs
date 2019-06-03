using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.SoulEssence;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    [System.Serializable]
    public class EnemyManager
    {
        [SerializeField]
        private DeathSenseHandler m_deathSenseHandler;
        private bool m_showEnemyHealth;

        private Dictionary<int, Enemy> m_enemyList;

        public EnemyManager()
        {
            m_enemyList = new Dictionary<int, Enemy>();
        }

        public void Register(Enemy enemy)
        {
            m_enemyList.Add(enemy.GetInstanceID(), enemy);
            if (m_showEnemyHealth)
            {
                m_deathSenseHandler.TrackHealth(enemy);
            }
            enemy.Death += OnEnemyDeath;
        }

        public void Unregister(Enemy enemy)
        {
            m_enemyList.Remove(enemy.GetInstanceID());
            if (m_showEnemyHealth)
            {
                m_deathSenseHandler.RemoveTracker(enemy);
            }
        }

        public void Initialize()
        {
            m_deathSenseHandler.Initialize();
            m_showEnemyHealth = GameSystem.settings?.gameplay.showEnemyHealth ?? true;
        }

        private void OnEnemyDeath(object sender, EnemyInfoEventArgs eventArgs)
        {
            //Drop Essence
            Unregister(m_enemyList[eventArgs.instanceID]);
        }
    }
}