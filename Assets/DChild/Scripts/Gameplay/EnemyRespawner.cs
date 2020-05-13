using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Holysoft.Collections;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public class EnemyRespawner : MonoBehaviour
    {
        [SerializeField]
        private RangeFloat m_respawnTime;
        [SerializeField]
        private GameObject m_respawnFX;
        [SerializeField]
        private Damageable[] m_damageableList;

        private Dictionary<Damageable, Vector3> m_damageableStartPosition;
        private Dictionary<Damageable, IResetableAIBrain> m_ressetableBrains;
        private Dictionary<DeathHandle, Damageable> m_deathHandlePair;
        private List<Damageable> m_toRessurrect;
        private List<float> m_ressurectTimers;

        private FXSpawnHandle<FX> m_fxSpawner;

        private void OnEnemyDestroyed(object sender, EventActionArgs eventArgs)
        {
            m_toRessurrect.Add((Damageable)sender);
            m_ressurectTimers.Add(m_respawnTime.GenerateRandomValue());
        }

        private void OnEnemyBodyDisposed(object sender, DeathHandle.DisposingEventArgs eventArgs)
        {
            if (eventArgs.isBodyDestroyed == false)
            {
                m_toRessurrect.Add(m_deathHandlePair[(DeathHandle)sender]);
                m_ressurectTimers.Add(m_respawnTime.GenerateRandomValue()); 
            }
        }

        private void Respawn(Damageable damageable)
        {
            damageable.gameObject.SetActive(true);
            damageable.SetHitboxActive(true);
            damageable.Heal(99999999);
            damageable.transform.position = m_damageableStartPosition[damageable];
            m_ressetableBrains[damageable]?.ResetAI();
            if (m_respawnFX)
            {
                m_fxSpawner.InstantiateFX(m_respawnFX, m_damageableStartPosition[damageable]);
            }
        }

        private void Awake()
        {
            m_toRessurrect = new List<Damageable>();
            var length = m_damageableList.Length;
            m_damageableStartPosition = new Dictionary<Damageable, Vector3>();
            m_ressetableBrains = new Dictionary<Damageable, IResetableAIBrain>();
            m_deathHandlePair = new Dictionary<DeathHandle, Damageable>();
            m_ressurectTimers = new List<float>();
            Damageable damageable = null;
            for (int i = 0; i < length; i++)
            {
                damageable = m_damageableList[i];
                m_damageableStartPosition.Add(damageable, damageable.transform.position);
                m_ressetableBrains.Add(damageable, damageable.GetComponent<IResetableAIBrain>());
                if (damageable.TryGetComponentInChildren(out DeathHandle deathHandle))
                {
                    m_deathHandlePair.Add(deathHandle, damageable);
                    deathHandle.BodyDestroyed += OnEnemyBodyDisposed;
                }
                else
                {
                    damageable.Destroyed += OnEnemyDestroyed;
                }
            }
        }



        private void LateUpdate()
        {
            var deltaTime = GameplaySystem.time.deltaTime;
            for (int i = m_ressurectTimers.Count - 1; i >= 0; i--)
            {
                m_ressurectTimers[i] -= deltaTime;
                if (m_ressurectTimers[i] <= 0)
                {
                    Respawn(m_toRessurrect[i]);
                    m_ressurectTimers.RemoveAt(i);
                    m_toRessurrect.RemoveAt(i);
                }
            }
        }
    }
}