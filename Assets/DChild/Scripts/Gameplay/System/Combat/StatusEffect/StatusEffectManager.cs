using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    [System.Serializable]
    public class StatusEffectManager
    {
        [SerializeField, HideInEditorMode]
        private GameObject[] m_statusEffects;
        [OdinSerialize]
        private StatusEffectTracker m_tracker;
        [ShowInInspector, HideInEditorMode]
        private StatusEffectUpdateHandler m_updateHandler;
        [ShowInInspector, HideInEditorMode]
        private StatusEffectPool m_pool;

        public void InflictStatusTo(IStatusReciever statusReciever, params StatusInflictionInfo[] statusInflictionInfos)
        {
            if (statusReciever.statusResistance == null)
            {
                for (int i = 0; i < statusInflictionInfos.Length; i++)
                {
                    if (statusInflictionInfos[i].chance >= 100 || statusInflictionInfos[i].chance <= UnityEngine.Random.Range(0, 100))
                    {
                        InflictStatusTo(statusReciever, statusInflictionInfos[i].effect);
                    }
                }
            }
            else
            {
                for (int i = 0; i < statusInflictionInfos.Length; i++)
                {
                    var info = statusInflictionInfos[i];
                    if (statusReciever.statusResistance.GetResistance(info.effect) != StatusResistanceType.Immune)
                    {
                        if (info.chance == 100 || info.chance <= UnityEngine.Random.Range(0, 100))
                        {
                            InflictStatusTo(statusReciever, info.effect);
                        }
                    }
                }
            }
        }

        public void CureStatusOf(IStatusReciever statusReciever, StatusEffectType type)
        {
            if (statusReciever.statusEffectState.IsInflictedWith(type))
            {
                m_tracker.GetStatusEffectOf(statusReciever, type).StopEffect();
            }
        }

        public void InflictStatusTo(IStatusReciever statusReciever, StatusEffectType type)
        {
            var index = (int)type;
            var existingStatus = m_tracker.GetStatusEffectOf(statusReciever, type);
            if (existingStatus)
            {
                existingStatus.StartEffect();
            }
            else
            {
                StatusEffect statusEffect = m_pool.RetrieveFromPool(type);
                if (statusEffect == null)
                {
                    var statusEffectGO = UnityEngine.Object.Instantiate(m_statusEffects[index]);
                    statusEffect = statusEffectGO.GetComponent<StatusEffect>();
                }

                statusEffect.transform.parent = statusReciever.transform;
                statusEffect.SpawnAt(statusReciever.transform.position, Quaternion.identity);
                statusEffect.SetReciever(statusReciever);
                statusEffect.StartEffect();
                statusEffect.EffectEnd += OnEffectEnd;

                m_tracker.TrackEffect(statusReciever, type, statusEffect);
                m_updateHandler.Add(statusEffect);
            }
        }

        public void Initialize()
        {
            m_tracker = new StatusEffectTracker();
            m_updateHandler = new StatusEffectUpdateHandler();
            m_pool = GameSystem.poolManager.GetPool<StatusEffectPool>();
        }

        public void Update()
        {
            //m_updateHandler.Update();
        }

        private void OnEffectEnd(object sender, StatusEffectEventArgs eventArgs)
        {
            eventArgs.effect.EffectEnd -= OnEffectEnd;
            m_pool.AddToPool(eventArgs.effect);
        }

#if UNITY_EDITOR
        [NonSerialized, OdinSerialize, PropertyOrder(-1), HideInPlayMode]
        private EnumList<StatusEffectType, GameObject> m_statusEffectList = new EnumList<StatusEffectType, GameObject>();

        public void OnValidate()
        {
            m_statusEffects = m_statusEffectList.ToArray();
        }
#endif
    }
}