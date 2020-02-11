using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Trohpies
{
    public class TrophyManager : MonoBehaviour
    {
        [SerializeField]
        private TrophyList m_dataList;
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private List<TrophyHandle> m_trophyHandles;
        private List<TrophyHandle> m_serializableTrophyHandles;
        private Dictionary<int, TrophyInfo> m_trophyInfos;
        private Dictionary<int, bool> m_trophyProgress;

        public bool IsLocked(int ID) => m_trophyProgress[ID] == false;
        public TrophyInfo GetInfo(int ID) => m_trophyInfos.ContainsKey(ID) ? m_trophyInfos[ID] : null;

        private void OnComplete(object sender, TrophyHandleEventArgs eventArgs)
        {
            var instance = eventArgs.instance;
            m_trophyProgress[instance.ID] = true;
            m_trophyHandles.Remove(instance);
            if (instance.hasSerializableModules)
            {
                m_serializableTrophyHandles.Remove(instance);
            }
            //Show Notification;
        }

        private void CreateHandles(int ID)
        {
            if (IsLocked(ID))
            {
                var cache = m_dataList.GetInfo(ID).CreateHandle();
                cache.Initialize();
                cache.Complete += OnComplete;
                m_trophyHandles.Add(cache);
                if (cache.hasSerializableModules)
                {
                    m_serializableTrophyHandles.Add(cache);
                    cache.LoadProgress();
                }
            }
        }

        private void DeserializeProgress(int IDs)
        {
            var isUnlocked = false;
            //Connect to Account Progress
            m_trophyProgress.Add(IDs, isUnlocked);
        }

        private void GenerateInfoList(int ID)
        {
            bool connectedToSteam = false;
            if (connectedToSteam)
            {
                //Get Info from steam directly
            }
            else
            {
                m_trophyInfos.Add(ID, m_dataList.GetInfo(ID).CreateInfo());
            }
        }

        private void Awake()
        {
            m_trophyInfos = new Dictionary<int, TrophyInfo>();
            m_trophyHandles = new List<TrophyHandle>();
            m_trophyProgress = new Dictionary<int, bool>();
            m_trophyInfos = new Dictionary<int, TrophyInfo>();

            var IDs = m_dataList.GetIDs();
            for (int i = 0; i < IDs.Length; i++)
            {
                var id = IDs[i];
                GenerateInfoList(id);
                DeserializeProgress(id);
                CreateHandles(id);
            }
        }
    }
}