using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using Holysoft.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Menu.Codex
{
    public class CodexProgressSerializer : MonoBehaviour, ISerializable<CodexSaveData>
    {
        [SerializeField, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, DraggableItems = false, OnBeginListElementGUI = "OnBeginTrackerListGUI")]
        private CodexProgressTracker[] m_trackers = new CodexProgressTracker[(int)CodexSubtab._COUNT];

        public CodexSaveData SaveData()
        {
            var data = new AcquisitionData[(int)CodexSubtab._COUNT];

            for (int i = 0; i < m_trackers.Length; i++)
            {
                var tracker = m_trackers[i];
                if (tracker == null)
                {
                    data[i] = null;
                }
                else
                {
                    data[i] = m_trackers[i].SaveData();
                }
            }

            return new CodexSaveData(data);
        }

        public void LoadData(CodexSaveData saveData)
        {
            if (saveData == null)
            {
                for (int i = 0; i < m_trackers.Length; i++)
                {
                    var tracker = m_trackers[i];
                    if (tracker != null)
                    {
                        m_trackers[i].LoadData(null);
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_trackers.Length; i++)
                {
                    var tracker = m_trackers[i];
                    if (tracker != null)
                    {
                        m_trackers[i].LoadData(saveData.GetData((CodexSubtab)i));
                    }
                }
            }
        }

        private void OnBeginTrackerListGUI(int index)
        {
#if UNITY_EDITOR
            EditorGUILayout.LabelField(((CodexSubtab)index).ToString());
#endif
        }

#if UNITY_EDITOR
        [Button, PropertyOrder(-1)]
        private void UpdateSubtabs()
        {
            var tracker = new CodexProgressTracker[(int)CodexSubtab._COUNT];
            var sizeTransfer = Mathf.Min(tracker.Length, m_trackers.Length);
            for (int i = 0; i < sizeTransfer; i++)
            {
                tracker[i] = m_trackers[i];
            }
            m_trackers = tracker;
        }
#endif
    }
}