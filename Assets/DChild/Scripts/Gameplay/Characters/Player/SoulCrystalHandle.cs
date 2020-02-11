using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Serialization.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class SoulCrystalHandle : SerializedMonoBehaviour
    {
        private class ChallengeEventArgs : IEventActionArgs
        {
            public ChallengeHandle challengeHandle { get; private set; }

            public void Initialize(ChallengeHandle challengeHandle)
            {
                this.challengeHandle = challengeHandle;
            }
        }

        private class ChallengeHandle
        {
            private SoulCrystal m_crystal;
            private IChallenge[] m_challenges;
            private List<ITimeChallenge> m_timeChallenges;

            public event EventAction<ChallengeEventArgs> Completed;

            public bool isComplete
            {
                get
                {
                    bool isCOmplete = true;
                    for (int i = 0; i < m_challenges.Length; i++)
                    {
                        isCOmplete = m_challenges[i].IsComplete();
                        if (isCOmplete == false)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }

            public ChallengeHandle(SoulCrystal crystal)
            {
                m_crystal = crystal;
                m_challenges = crystal.CreateChallenges();
                for (int i = 0; i < m_challenges.Length; i++)
                {
                    m_challenges[i].Completed += OnChallengeComplete;
                    if (m_challenges[i] is ITimeChallenge)
                    {
                        if (m_timeChallenges == null)
                        {
                            m_timeChallenges = new List<ITimeChallenge>();
                        }
                        m_timeChallenges.Add((ITimeChallenge)m_challenges[i]);
                    }
                }
            }

            public void Update(float deltaTime)
            {
                for (int i = 0; i < m_timeChallenges.Count; i++)
                {
                    m_timeChallenges[i].Update(deltaTime);
                }
            }

            public SoulCrystal crystal => m_crystal;

            private void OnChallengeComplete(object sender, EventActionArgs eventArgs)
            {
                bool rewardSoulSkill = true;
                for (int i = 0; i < m_challenges.Length; i++)
                {
                    rewardSoulSkill = m_challenges[i].IsComplete();
                    if (rewardSoulSkill == false)
                    {
                        break;
                    }
                }

                if (rewardSoulSkill)
                {
                    using (Cache<ChallengeEventArgs> cacheEventArgs = Cache<ChallengeEventArgs>.Claim())
                    {
                        cacheEventArgs.Value.Initialize(this);
                        Completed?.Invoke(this, cacheEventArgs.Value);
                        cacheEventArgs.Release();
                    }
                }
            }
        }
        [SerializeField]
        private IItemContainer m_crystalContainer;
        [SerializeField]
        private SoulSkillAcquisitionList m_soulAcquisitionList;
        private List<ChallengeHandle> m_challengeList;

        public void InitializeHandles()
        {
            if (m_crystalContainer.Count > 0)
            {
                m_challengeList.Clear();
                for (int i = 0; i < m_crystalContainer.Count; i++)
                {
                    m_challengeList.Add(new ChallengeHandle((SoulCrystal)m_crystalContainer.GetSlot(i).item));
                }
            }
        }

        private void OnItemUpdate(object sender, ItemEventArgs eventArgs)
        {
            if (eventArgs.data is SoulCrystal)
            {
                var handle = new ChallengeHandle((SoulCrystal)eventArgs.data);
                if (handle.isComplete)
                {
                    m_soulAcquisitionList.SetAcquisition(handle.crystal.id, true);
                }
                else
                {
                    handle.Completed += OnChallengeComplete;
                    m_challengeList.Add(handle);
                    enabled = true;
                }
            }
        }

        private void OnChallengeComplete(object sender, ChallengeEventArgs eventArgs)
        {
            m_challengeList.Remove(eventArgs.challengeHandle);
            var crystal = eventArgs.challengeHandle.crystal;
            m_soulAcquisitionList.SetAcquisition(crystal.id, true);
            m_crystalContainer.SetItem(crystal, 0);
            if (m_challengeList.Count == 0)
            {
                enabled = false;
            }
        }

        private void Awake()
        {
            m_challengeList = new List<ChallengeHandle>();
            m_crystalContainer.ItemUpdate += OnItemUpdate;
            enabled = false;
        }


        private void Update()
        {
            for (int i = 0; i < m_challengeList.Count; i++)
            {
                m_challengeList[i].Update(Time.deltaTime);
            }
        }
    }
}