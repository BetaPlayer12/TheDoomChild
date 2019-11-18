using DChild.Gameplay.Combat;
using Holysoft.Gameplay.UI;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Systems
{
    [System.Serializable]
    public class HealthTracker : MonoBehaviour, IHealthTracker, IGameplaySystemModule, IGameplayInitializable
    {
        [SerializeField]
        private RectTransform m_parentUI;
        [SerializeField]
        private GameObject m_healthUI;
        [SerializeField]
        private Vector3 m_offset;

        private Dictionary<int, CappedStatUI> m_damageableList;
        private List<ObjectFollower> m_objectFollowerList;

        public void Initialize()
        {
            m_damageableList = new Dictionary<int, CappedStatUI>();
            m_objectFollowerList = new List<ObjectFollower>();
            enabled = false;

        }

        public void TrackHealth(Damageable damageable)
        {
            var instance = CreateUI(damageable.gameObject.scene);
            instance.transform.SetParent(m_parentUI);
            instance.MonitorInfoOf(damageable.health);
            var ID = damageable.GetInstanceID();
            m_damageableList.Add(ID, instance);
            ObjectFollower objectFollower = new ObjectFollower();
            objectFollower.Initialize(ID, instance.gameObject);

            if (damageable.CompareTag(Character.objectTag))
            {
                objectFollower.SetTarget(damageable.GetComponent<Character>().GetBodyPart(BodyReference.BodyPart.OverHead));
            }
            else
            {
                objectFollower.SetTarget(damageable.transform);
            }
            m_objectFollowerList.Add(objectFollower);
            enabled = true;
        }

        public void RemoveTracker(Damageable damageable)
        {
            var id = damageable.GetInstanceID();
            if (m_damageableList.ContainsKey(id))
            {
                var instance = m_damageableList[id];
                instance.MonitorInfoOf(null);
                m_damageableList.Remove(id);
                for (int i = 0; i < m_objectFollowerList.Count; i++)
                {
                    if (m_objectFollowerList[i].referenceID == id)
                    {
                        m_objectFollowerList.RemoveAt(i);
                        break;
                    }
                }
                UnityEngine.Object.Destroy(instance.gameObject);
            }
            enabled = m_damageableList.Count > 0;
        }

        public void RemoveAllTrackers()
        {
            if (m_damageableList.Count > 0)
            {
                foreach (var ui in m_damageableList.Values)
                {
                    ui.MonitorInfoOf(null);
                    UnityEngine.Object.Destroy(ui.gameObject);
                }
                m_damageableList.Clear();
                m_objectFollowerList.Clear();
            }
            enabled = false;
        }

        private CappedStatUI CreateUI(Scene scene)
        {
            var ui = this.InstantiateToScene(m_healthUI, scene);
            return ui.GetComponent<CappedStatUI>();
        }

        private void LateUpdate()
        {
            for (int i = 0; i < m_objectFollowerList.Count; i++)
            {
                m_objectFollowerList[i].FollowTarget(m_offset);
            }
        }
    }
}