using Holysoft.Collections;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    public class NavigationMapInstance : SerializedMonoBehaviour
    {
        [SerializeField, HideInPlayMode]
        private NavMapFogOfWarUI[] m_sceneFogOFWars;
        [SerializeField, HideInEditorMode]
        private Dictionary<string, NavMapFogOfWarSegment> m_fogOfWarSegments;

        public void UpdateFogOfWar()
        {
            foreach (var varName in m_fogOfWarSegments.Keys)
            {
                var state = DialogueLua.GetVariable(varName).asInt;
                SetFogOfwarState(varName, (Flag)state);
            }
        }

        public void SetFogOfwarState(string varName, Flag state)
        {
            m_fogOfWarSegments[varName].SetUIState(state);
        }

        public NavMapFogOfWarUI GetFogOfWarOfScene(int index) => m_sceneFogOFWars[index];

        private void OnValidate()
        {
#if UNITY_EDITOR
            m_fogOfWarSegments = new Dictionary<string, NavMapFogOfWarSegment>();
            for (int i = 0; i < m_sceneFogOFWars.Length; i++)
            {
                AddSegmentToDictionary(m_sceneFogOFWars[i].GetSegments());
            }

            void AddSegmentToDictionary(Dictionary<string, NavMapFogOfWarSegment> toAdd)
            {
                foreach (var key in toAdd.Keys)
                {
                    m_fogOfWarSegments.Add(key, toAdd[key]);
                }
            }
#endif
        }
    }
}