using DChild.Serialization;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace DChild.Menu.Codex
{
    public abstract class CodexProgressTracker : MonoBehaviour
    {
        [ShowInInspector, HideInEditorMode]
        protected Dictionary<int, bool> m_progress;

        public bool HasInfoOf(int ID) => m_progress.ContainsKey(ID) ? m_progress[ID] : false;
        public void SetProgress(int ID, bool value)
        {
            if (m_progress.ContainsKey(ID))
            {
                m_progress[ID] = value;
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
                var IDs = GetCodexEntryIDs();
                for (int i = 0; i < IDs.Length; i++)
                {
                    m_progress.Add(IDs[i], false);
                }
            }

            if (saveData == null)
                return;

            var size = saveData.count;
            for (int i = 0; i < size; i++)
            {
                var data = saveData.GetData(i);
                if (m_progress.ContainsKey(data.ID))
                {
                    m_progress[data.ID] = data.hasData;
                }
            }
        }

        protected abstract int[] GetCodexEntryIDs();
    }

    public abstract class CodexProgressTracker<DatabaseAssetListType, DatabaseAssetType> : CodexProgressTracker where DatabaseAssetListType : DatabaseAssetList<DatabaseAssetType>
                                                                                                               where DatabaseAssetType : DatabaseAsset
    {
        [SerializeField]
        private DatabaseAssetListType m_list;

        protected override int[] GetCodexEntryIDs() => m_list.GetIDs();
    }
}