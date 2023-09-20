using UnityEngine;
#if UNITY_EDITOR
using DChild.Menu.Codex;
using Sirenix.OdinInspector;
#endif

namespace DChild.Menu.Bestiary
{
    public class BestiaryEntity : MonoBehaviour
    {
        [SerializeField]
        private BestiaryData m_data;

        public int bestiaryID => m_data.id;
        public void SetData(BestiaryData data)
        {
            m_data = data;
        }

#if UNITY_EDITOR
        [Button, HideIf("@m_data == null")]
        private void ForceBestiaryUpdateWithEntity()
        {
            var trackers = FindObjectsOfType<CodexProgressTracker<BestiaryList, BestiaryData>>();
            for (int i = 0; i < trackers.Length; i++)
            {
                trackers[i].SetProgress(m_data.id, true);
            }
        }
#endif
    }
}