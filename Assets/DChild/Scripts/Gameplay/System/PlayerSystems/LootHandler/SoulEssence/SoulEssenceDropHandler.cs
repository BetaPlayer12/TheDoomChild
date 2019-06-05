using DChild.Gameplay.Pooling;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.SoulEssence
{
    [System.Serializable]
    public class SoulEssenceDropHandler : MonoBehaviour
    {
        private struct SpliceInfo
        {
            public SpliceInfo(SoulEssenceDropInfo info, Vector2 position) : this()
            {
                this.dropInfo = info;
                this.position = position;
            }

            public SoulEssenceDropInfo dropInfo { get; }
            public Vector2 position { get; }
        }

        [SerializeField]
        private GameObject m_soulEssence;
        [SerializeField]
        private int m_maxDropInACall;

        private List<SpliceInfo> m_spliceDrop;

        public void Drop(SoulEssenceDropInfo info, Vector2 position)
        {
            var essenceCount = info.essenceCount;
            while (essenceCount > m_maxDropInACall)
            {
                essenceCount -= m_maxDropInACall;
                m_spliceDrop.Add(new SpliceInfo(new SoulEssenceDropInfo(info.valuePerEssence, m_maxDropInACall), position));
            }
            m_spliceDrop.Add(new SpliceInfo(new SoulEssenceDropInfo(info.valuePerEssence, essenceCount), position));
            enabled = true;
        }

        private void Drop(SpliceInfo splice)
        {
            for (int i = 0; i < splice.dropInfo.essenceCount; i++)
            {
                SoulEssenceLoot essence = (SoulEssenceLoot)GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_soulEssence);
                essence.value = splice.dropInfo.valuePerEssence;
                essence.SpawnAt(splice.position, Quaternion.identity);
            }
        }

        private void Awake()
        {
            m_spliceDrop = new List<SpliceInfo>();
            enabled = false;
        }

        private void Update()
        {
            Drop(m_spliceDrop[0]);
            m_spliceDrop.RemoveAt(0);
            enabled = m_spliceDrop.Count >= 1;
        }
    }
}