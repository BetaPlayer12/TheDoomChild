using System;
using System.Collections.Generic;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace DChild.Gameplay.Trohpies
{
    public class TrophyHandleEventArgs : IEventActionArgs
    {
        public TrophyHandle instance { get; private set; }
        public void Initialize(TrophyHandle instance)
        {
            this.instance = instance;
        }
    }

    [System.Serializable]
    public class TrophyHandle
    {
        [SerializeField, ReadOnly]
        private int m_ID;
        private ITrophyModule[] m_modules;
        private List<ISerializableTrophyModule> m_serializableModules;
        private bool m_isInitialized;
        [SerializeField, ReadOnly]
        private bool m_isComplete;

        public int ID => m_ID;
        public bool hasSerializableModules => m_serializableModules != null;

        public event EventAction<TrophyHandleEventArgs> Complete;

        public TrophyHandle(int m_ID, ITrophyModule[] m_modules)
        {
            this.m_ID = m_ID;
            this.m_modules = m_modules;
            for (int i = 0; i < m_modules.Length; i++)
            {
                m_modules[i].Complete += OnComplete;
                if(m_modules[i] is ISerializableTrophyModule)
                {
                    if(m_serializableModules == null)
                    {
                        m_serializableModules = new List<ISerializableTrophyModule>();
                    }
                    m_serializableModules.Add((ISerializableTrophyModule)m_modules[i]);
                }
            }
        }


        public void Initialize()
        {
            if (m_isInitialized == false)
            {
                for (int i = 0; i < m_modules.Length; i++)
                {
                    m_modules[i].Initialize();
                }
                m_isInitialized = true;
            }
        }

        public void LoadProgress()
        {
            if (m_serializableModules != null)
            {
                for (int i = 0; i < m_serializableModules.Count; i++)
                {
                    //TODO: load from steam;
                }
            }
        }

        private void OnComplete(object sender, EventActionArgs eventArgs)
        {
            if (m_isComplete == false)
            {
                bool isComplete = true;
                for (int i = 0; i < m_modules.Length; i++)
                {
                    isComplete = m_modules[i].isComplete();
                    if (isComplete == false)
                    {
                        break;
                    }
                }
                if (isComplete)
                {
                    using (Cache<TrophyHandleEventArgs> cacheEventArgs = Cache<TrophyHandleEventArgs>.Claim())
                    {
                        cacheEventArgs.Value.Initialize(this);
                        Complete?.Invoke(this, cacheEventArgs.Value);
                        cacheEventArgs.Release();
                    }
                    m_isComplete = true;
                }
            }
        }

    }
}