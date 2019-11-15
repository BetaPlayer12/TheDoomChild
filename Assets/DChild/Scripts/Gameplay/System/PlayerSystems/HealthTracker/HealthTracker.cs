using DChild.Gameplay.Combat;
using Holysoft.Gameplay.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Systems
{
    [System.Serializable]
    public class HealthTracker
    {
        [SerializeField]
        private GameObject m_healthUI;

        private Dictionary<int, CappedStatUI> m_damageableList;

        public void Initialize()
        {
            m_damageableList = new Dictionary<int, CappedStatUI>();
        }

        public void TrackHealth(Damageable damageable)
        {
            m_damageableList.Add(damageable.GetInstanceID(), null);
            var instance = CreateUI(damageable.gameObject.scene);
            instance.MonitorInfoOf(damageable.health);
            m_damageableList.Add(damageable.GetInstanceID(), instance);
        }

        public void RemoveTracker(Damageable damageable)
        {
            var id = damageable.GetInstanceID();
            if (m_damageableList.ContainsKey(id))
            {
                var instance = m_damageableList[id];
                instance.MonitorInfoOf(null);
                Object.Destroy(instance.gameObject);
                m_damageableList.Remove(id);
            }
        }

        private CappedStatUI CreateUI(Scene scene)
        {
            var ui = this.InstantiateToScene(m_healthUI, scene);
            return ui.GetComponent<CappedStatUI>();
        }
    }
}