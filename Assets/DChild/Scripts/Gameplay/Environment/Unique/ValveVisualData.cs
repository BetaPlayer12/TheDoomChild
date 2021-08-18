using System;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    [CreateAssetMenu(fileName = "ValveVisualData", menuName = "DChild/Gameplay/Environment/Valve Visual Data")]
    public class ValveVisualData : ScriptableObject
    {
        [System.Serializable]
        public class VisualInfo
        {
            [SerializeField]
            private Sprite m_valve;
            [SerializeField]
            private Sprite m_mechanism;

            public void LoadVisualsTo(SpriteRenderer valve, SpriteRenderer mechanism)
            {
                valve.sprite = m_valve;
                mechanism.sprite = m_mechanism;
            }
        }

        [SerializeField]
        private VisualInfo[] m_visualList;

        public int count => m_visualList.Length;

        public VisualInfo GetVisual(int index) => m_visualList[index];

#if UNITY_EDITOR
        [Button]
        private void SetThisDirty()
        {
            EditorUtility.SetDirty(this);
        }
#endif
    }
}