using System;
using System.Collections;
using DChild.Gameplay.Characters.Enemies;
using DChild.Serialization;
using Holysoft.Event;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace DChild.Gameplay.Combat
{
    public class BossFightTrigger : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_isTriggered;

            public SaveData(bool isTriggered)
            {
                m_isTriggered = isTriggered;
            }


            public bool isTriggered => m_isTriggered;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_isTriggered);
        }

        private enum PreFight
        {
            None,
            Delay,
            Cinematic
        }

        [SerializeField]
        private Boss m_boss;
        [SerializeField]
        private PreFight m_prefight;
        [SerializeField, MinValue(0.1f), ShowIf("@m_prefight == PreFight.Delay")]
        private float m_startDelay;
        [SerializeField, ShowIf("@m_prefight == PreFight.Cinematic")]
        private PlayableDirector m_director;
        [SerializeField, ShowIf("@m_prefight == PreFight.Cinematic")]
        private PlayableAsset m_cinematic;
        [SerializeField, ShowIf("@m_prefight == PreFight.Cinematic")]
        private bool m_afterCinematicDialogue;
        [SerializeField, TabGroup("Upon Trigger")]
        private UnityEvent m_uponTrigger;
        [SerializeField, TabGroup("On Defeat")]
        private UnityEvent m_onDefeat;
        [SerializeField, TabGroup("Already Defeated")]
        private UnityEvent m_alreadyDefeated;
        private bool m_isTriggered;

        private (Damageable damageable, Character character) m_targetTuple;

        public void ForceFightWithPlayer()
        {
            var player = GameplaySystem.playerManager.player;
            m_targetTuple = ((Damageable)player.damageableModule, player.character);
            StartFight();
        }

        public void StartCombat()
        {
            //GameplaySystem.gamplayUIHandle.MonitorBoss(m_boss);
            GameplaySystem.gamplayUIHandle.ToggleBossHealth(true);
            m_boss.SetTarget(m_targetTuple.damageable, m_targetTuple.character);
            m_boss.Enable();
        }

        public void SetupBossUI()
        {
            GameplaySystem.gamplayUIHandle.MonitorBoss(m_boss);
        }

        public ISaveData Save() => new SaveData(m_isTriggered);

        public void Load(ISaveData data)
        {
            m_isTriggered = ((SaveData)data).isTriggered;
            if (m_isTriggered)
            {
                m_alreadyDefeated?.Invoke();
            }
        }
        public void Initialize()
        {
            m_isTriggered = false;
        }

        private void OnCinematicStop(PlayableDirector obj)
        {
            if (m_afterCinematicDialogue == false)
            {
                StartCombat();
            }

        }

        private void OnBossKilled(object sender, EventActionArgs eventArgs)
        {
            GameplaySystem.gamplayUIHandle.ToggleBossHealth(false);
            m_onDefeat?.Invoke();
        }

        private IEnumerator DelayedAwakeRoutine()
        {
            yield return new WaitForSeconds(m_startDelay);
            StartCombat();
        }

        private void StartFight()
        {

            switch (m_prefight)
            {
                case PreFight.None:
                    StartCombat();
                    break;
                case PreFight.Delay:
                    StartCoroutine(DelayedAwakeRoutine());
                    break;
                case PreFight.Cinematic:
                    m_director.extrapolationMode = DirectorWrapMode.None;
                    m_director.stopped += OnCinematicStop;
                    m_director.Play(m_cinematic);
                    break;
            }
            m_uponTrigger?.Invoke();
            m_isTriggered = true;
        }

        private void Awake()
        {
            if (m_boss != null)
            {
                m_boss.GetComponent<Damageable>().Destroyed += OnBossKilled;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_isTriggered == false)
            {
                if (collision.tag != "Sensor")
                {
                    var target = collision.GetComponentInParent<ITarget>();
                    if (target.CompareTag(Character.objectTag))
                    {
                        m_targetTuple = (collision.GetComponentInParent<Damageable>(), collision.GetComponentInParent<Character>());
                        StartFight();
                    }
                }
            }
        }

        public void UpdateDialogueSaveData()
        {
            GameplaySystem.campaignSerializer.UpdateDialogueSaveData();
        }
    }
}