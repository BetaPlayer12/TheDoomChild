using DChild.Menu.Bestiary;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public struct BestiaryUpdateEventArgs : IEventActionArgs
    {
        public BestiaryUpdateEventArgs(int iD) : this()
        {
            ID = iD;
        }

        public int ID { get; }
    }

    public class BestiaryProgress : MonoBehaviour
    {
        [SerializeField]
        private BestiaryList m_list;
        [ShowInInspector, HideInEditorMode]
        private Dictionary<int, bool> m_progress;
        public event EventAction<BestiaryUpdateEventArgs> NewUpdate;

        public BestiaryList list => m_list;

        public bool HasInfoOf(int ID) => m_progress.ContainsKey(ID) ? m_progress[ID] : false;
        public void SetProgress(int ID, bool value)
        {
            if (m_progress.ContainsKey(ID))
            {
                m_progress[ID] = value;
                NewUpdate?.Invoke(this, new BestiaryUpdateEventArgs(ID));
            }
        }

        public AcquisitionData SaveData()
        {
            List<AcquisitionData.SerializeData> serializedDatas = new List<AcquisitionData.SerializeData>();
            foreach (var key in m_progress.Keys)
            {
                serializedDatas.Add(new AcquisitionData.SerializeData(key, m_progress[key]));
            }
            return new AcquisitionData(serializedDatas.ToArray());
        }

        public void LoadData(AcquisitionData saveData)
        {
            if (m_progress == null)
            {
                m_progress = new Dictionary<int, bool>();
                var IDs = m_list.GetIDs();
                for (int i = 0; i < IDs.Length; i++)
                {
                    m_progress.Add(IDs[i], false);
                }
            }

            var size = saveData.count;
            for (int i = 0; i < size; i++)
            {
                var data = saveData.GetData(i);
                if (m_progress.ContainsKey(data.ID))
                {
                    if (data.hasData)
                    {
                        Debug.Log("HAHAHA");
                    }
                    m_progress[data.ID] = data.hasData;
                }
            }
        }

        private void Awake()
        {
            if (m_progress == null)
            {
                m_progress = new Dictionary<int, bool>();
                var IDs = m_list.GetIDs();
                for (int i = 0; i < IDs.Length; i++)
                {
                    m_progress.Add(IDs[i], false);
                }
            }
        }
    }
}