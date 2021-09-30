using DChild.Gameplay.Combat.UI;
using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    public class CombatUIHandler
    {
        public struct UIInfo
        {
            public UIInfo(Vector3 position, TMP_ColorGradient configurations, int value, bool isCrit) : this()
            {
                this.position = position;
                this.configurations = configurations;
                this.value = value;
                this.isCrit = isCrit;
            }

            public Vector3 position { get; }
            public TMP_ColorGradient configurations { get; }
            public int value { get; }
            public bool isCrit { get; }
        }

        [SerializeField]
        private GameObject m_damageValueUI;
        [SerializeField, MinValue(0.1)]
        private float m_positionOffset;
        [SerializeField, MinValue(1)]
        private int m_maxInstanceSpawned;
        [SerializeField]
        private DamageUIConfigurations m_damageUIConfigurations;

        private List<UIInfo> m_uiInfoList;

        private UIObjectPool m_pool;
        private Scene m_scene;

        public void ShowDamageValues(Vector3 position, Damage damages, bool isCrit)
        {
            var info = new UIInfo(OffsetPosition(position), m_damageUIConfigurations.FindDamageConfiguration(damages.type), damages.value, isCrit);
            m_uiInfoList.Insert(0, info);
        }

        public void ShowHealValues(Vector3 position, int healValue, bool isCrit)
        {
            var info = new UIInfo(OffsetPosition(position), m_damageUIConfigurations.healConfiguration, healValue, isCrit);
            m_uiInfoList.Insert(0, info);
        }

        public void Initialize(Scene scene)
        {
            m_uiInfoList = new List<UIInfo>();
            m_pool = GameSystem.poolManager?.GetPool<UIObjectPool>() ?? null;
            m_scene = scene;
        }

        public void Update()
        {
            if (m_uiInfoList != null)
            {
                var count = m_uiInfoList.Count < m_maxInstanceSpawned ? m_uiInfoList.Count : m_maxInstanceSpawned;
                for (int i = 0; i < count; i++)
                {
                    ShowCombatUI(m_uiInfoList[0]);
                    m_uiInfoList.RemoveAt(0);
                }
            }
        }

        private Vector3 OffsetPosition(Vector3 position)
        {
            var xNormal = Random.Range(-1f, 1f);
            var yNormal = Random.Range(-1f, 1f);
            return position + (new Vector3(xNormal, yNormal, 0f) * m_positionOffset);
        }

        private GameObject GetOrCreateUIGameObject()
        {
            var uiObject = (m_pool.GetOrCreateItem(m_damageValueUI));
            if (uiObject == null)
            {
                return this.InstantiateToScene(m_damageValueUI, m_scene);
            }
            else
            {
                uiObject.transform.parent = null;
                return uiObject.gameObject;
            }
        }

        private void ShowCombatUI(UIInfo info)
        {
            var ui = GetOrCreateUIGameObject().GetComponent<IDamageUI>();
            ui.Load(info.value, info.configurations, info.isCrit);
            ui.SpawnAt(info.position);
        }



#if UNITY_EDITOR
        public void Initialize(GameObject damageUI, float positionOffset)
        {
            m_damageValueUI = damageUI;
            m_positionOffset = positionOffset;
        }
#endif
    }
}