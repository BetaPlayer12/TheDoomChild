using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat.UI;
using Holysoft.Gameplay.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Systems
{
    [System.Serializable]
    public class DeathSenseHandler
    {
        [SerializeField]
        private GameObject m_healthUI;

        private Dictionary<int, CappedStatUI> m_enemyList;

        public void Initialize()
        {
            m_enemyList = new Dictionary<int, CappedStatUI>();
        }

        public void TrackHealth(Enemy enemy)
        {
            m_enemyList.Add(enemy.GetInstanceID(), null);
            var instance = CreateUI(enemy.gameObject.scene);
            instance.MonitorInfoOf(enemy.health);
            m_enemyList.Add(enemy.GetInstanceID(), instance);
        }

        public void RemoveTracker(Enemy enemy)
        {
            var id = enemy.GetInstanceID();
            if (m_enemyList.ContainsKey(id))
            {
                var instance = m_enemyList[id];
                instance.MonitorInfoOf(null);
                Object.Destroy(instance.gameObject);
                m_enemyList.Remove(id);
            }
        }

        private CappedStatUI CreateUI(Scene scene)
        {
            var ui = this.InstantiateToScene(m_healthUI, scene);
            return ui.GetComponent<CappedStatUI>();
        }
    }
}