using DChild.Serialization;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace DChild.Menu.Codex
{
    [System.Serializable]
    public class CodexSaveData
    {
        [SerializeField]
        private AcquisitionData[] m_codexDatas;

        public CodexSaveData()
        {
            m_codexDatas = new AcquisitionData[(int)CodexSubtab._COUNT];
        }

        public CodexSaveData(AcquisitionData[] codexDatas)
        {
            m_codexDatas = codexDatas;
        }

        public CodexSaveData(CodexSaveData codexData)
        {
            var subtabCount = (int)CodexSubtab._COUNT;
            m_codexDatas = new AcquisitionData[subtabCount];
            if (codexData == null)
            {
                for (int i = 0; i < subtabCount; i++)
                {
                    m_codexDatas[i] = new AcquisitionData();
                }
            }
            else
            {
                for (int i = 0; i < subtabCount; i++)
                {
                    m_codexDatas[i] = codexData.GetData((CodexSubtab)i);
                }
            }
        }

        public AcquisitionData GetData(CodexSubtab codexSubtab)
        {
            return m_codexDatas[(int)codexSubtab];
        }
    }
}