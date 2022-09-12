using UnityEngine;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using DarkTonic.MasterAudio;
using System.Collections.Generic;
using System.Linq;

namespace DChild.Gameplay.Audio
{
    [CreateAssetMenu(fileName = "CompositeAmbientSoundData", menuName = "DChild/Audio/Composite Ambient Data")]
    public class CompositeAmbientSoundData : ScriptableObject
    {
        [System.Serializable]
        private class MultiAudioInfo
        {
            [SerializeField]
            private int m_sourceAmountThreshold;
            [SerializeField, SoundGroup]
            private string m_soundGroupToReplace;

            public int sourceAmountThreshold => m_sourceAmountThreshold;
            public string soundGroupToReplace => m_soundGroupToReplace;
        }
        [SerializeField, SoundGroup]
        private string m_soundGroup;
        [SerializeField, OnValueChanged("OnInfosChange", true), ListDrawerSettings(DraggableItems = false)]
        private MultiAudioInfo[] m_multiAudioInfos;

        public string soundGroup => m_soundGroup;

        public string GetRelevantSoundGroup(int audioSourceAmount)
        {
            for (int i = 0; i < m_multiAudioInfos.Length; i++)
            {
                var info = m_multiAudioInfos[i];
                if (info.sourceAmountThreshold > audioSourceAmount)
                {
                    return m_multiAudioInfos[i - 1].soundGroupToReplace;
                }
            }
            return m_multiAudioInfos[m_multiAudioInfos.Length - 1].soundGroupToReplace;
        }

        private void OnInfosChange()
        {
            var list = new List<MultiAudioInfo>(m_multiAudioInfos);
            list.OrderBy(x => x.sourceAmountThreshold);
            m_multiAudioInfos = list.ToArray();

        }
    }

}