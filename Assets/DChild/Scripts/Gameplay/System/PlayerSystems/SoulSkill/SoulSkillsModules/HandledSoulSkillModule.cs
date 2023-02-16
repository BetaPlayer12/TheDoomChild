using Sirenix.Utilities;
using Spine.Unity;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public abstract class HandledSoulSkillModule : ISoulSkillModule
    {
        protected struct ModuleReference
        {
            public ModuleReference(int soulSKillID, int moduleID, int playerInstanceID) : this()
            {
                this.soulSKillID = soulSKillID;
                this.moduleID = moduleID;
                this.playerInstanceID = playerInstanceID;
            }
            public int soulSKillID { get; }
            public int moduleID { get; }
            public int playerInstanceID { get; }
        }

        protected static class SoulSkillHandleManager
        {
            private static int m_currentID;
            private static Dictionary<int, BaseHandle> m_handles;
            private static bool m_isInitialize;

            public static int GenerateID()
            {
                m_currentID++;
                return m_currentID;
            }

            public static void AddHandle(ModuleReference reference, BaseHandle handle)
            {
                if (m_isInitialize == false)
                {
                    m_handles = new Dictionary<int, BaseHandle>();
                    m_isInitialize = true;
                }
                handle.Initialize();
                m_handles.Add(reference.moduleID, handle);
            }

            public static void RemoveHandle(ModuleReference reference)
            {
                m_handles[reference.moduleID].Dispose();
                m_handles.Remove(reference.moduleID);
            }
        }

        public abstract class BaseHandle
        {
            private int m_ID;
            protected IPlayer m_player;

            protected BaseHandle(IPlayer m_reference)
            {
                this.m_ID = SoulSkillHandleManager.GenerateID();
                this.m_player = m_reference;
            }

            public int ID => m_ID;

            public abstract void Initialize();
            public abstract void Dispose();
        }

        private Cache<List<ModuleReference>> m_referenceList;
        private bool m_initialized;

        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            var handle = CreateHandle(player);
            var reference = new ModuleReference(soulSkillInstanceID, handle.ID, player.GetInstanceID());
            SoulSkillHandleManager.AddHandle(reference, handle);
            Debug.Log("worked attach");
            if (m_initialized == false)
            {
                m_referenceList = Cache<List<ModuleReference>>.Claim();
                m_initialized = true;
            }
            m_referenceList.Value.Add(reference);
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            if (m_initialized)
            {
                var playerID = player.GetInstanceID();
                for (int i = 0; i < m_referenceList.Value.Count; i++)
                {
                    var reference = m_referenceList.Value[i];
                    if (reference.soulSKillID == soulSkillInstanceID && reference.playerInstanceID == playerID)
                    {
                        m_referenceList.Value.RemoveAt(i);
                        SoulSkillHandleManager.RemoveHandle(reference);
                        if (m_referenceList.Value.Count == 0)
                        {
                            m_referenceList.Release();
                            m_initialized = false;
                        }
                        break;
                    }
                }
            }
        }

        protected abstract BaseHandle CreateHandle(IPlayer player);
    }
}