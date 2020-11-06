using Sirenix.OdinInspector;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.UI
{
    public abstract class StylizedPlayerUI : MonoBehaviour
    {

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(1), HideInEditorMode]
        private PlayerUIStyle m_changeInto;

        [Button, PropertyOrder(1), HideInEditorMode]
        public void Change()
        {
            ChangeTo(m_changeInto);
        }
#endif

        public abstract void ChangeTo(PlayerUIStyle type);
    }

    public abstract class StylizedPlayerUI<T> : StylizedPlayerUI
    {
        protected abstract T[] list { get; set; }

        private void OnValidate()
        {
            var count = Enum.GetNames(typeof(PlayerUIStyle)).Length;
            if (list.Length != count)
            {
                list = new T[count];
            }
        }

#if UNITY_EDITOR
        private void OnAnimationBeginElement(int index)
        {
            EditorGUILayout.LabelField($"{(PlayerUIStyle)index}");
        }
#endif
    }
}