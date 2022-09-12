using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace DChild.Gameplay.Environment
{
    public class DynamicFlowFalls : MonoBehaviour
    {
        [System.Serializable]
        private class Something
        {
            [System.Serializable]
            public struct TransformInfo
            {
                [SerializeField]
                private Vector3 m_position;
                [SerializeField]
                private Vector3 m_scale;

                public TransformInfo(Vector3 position, Vector3 scale)
                {
                    m_position = position;
                    m_scale = scale;
                }

                public Vector3 position => m_position;
                public Vector3 scale => m_scale;
            }

            [SerializeField]
            private TransformInfo m_start;
            [SerializeField]
            private TransformInfo m_end;
            [SerializeField]
            private float m_duration;

            public Something(TransformInfo start, TransformInfo end, float duration)
            {
                m_start = start;
                m_end = end;
                m_duration = duration;
            }

            public TransformInfo start => m_start;
            public TransformInfo end => m_end;

            public float duration => m_duration;
        }

        [SerializeField]
        private Something[] m_list;

        public void ChangeInto(int index)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeShapeRoutine(m_list[index]));
        }

        private IEnumerator ChangeShapeRoutine(Something info)
        {
            var durationTimer = 0f;
            transform.position = info.start.position;
            transform.localScale = info.start.scale;
            yield return null;

            do
            {
                transform.position = Vector3.Lerp(info.start.position, info.end.position, durationTimer / info.duration);
                transform.localScale = Vector3.Lerp(info.start.scale, info.end.scale, durationTimer / info.duration);
                durationTimer += GameplaySystem.time.deltaTime;
                yield return null;
            } while (durationTimer < info.duration);

            transform.position = info.end.position;
            transform.localScale = info.end.scale;
        }

#if UNITY_EDITOR
        private enum InfoType
        {
            Start,
            End
        }

        [SerializeField, MaxValue("@m_list.Length"), PropertyOrder(-1)]
        private int m_indexToEdit;
        [SerializeField, PropertyOrder(-1)]
        private InfoType m_saveTo;

        [Button, PropertyOrder(-1)]
        private void CopyTransformInfoToIndex()
        {
            Undo.RecordObject(this, "Dynamic Falls Change");

            var transformInfo = new Something.TransformInfo(transform.position, transform.localScale);
            var formerInfo = m_list[m_indexToEdit];
            if (m_saveTo == InfoType.Start)
            {
                m_list[m_indexToEdit] = new Something(transformInfo, formerInfo.end, formerInfo.duration);
            }
            else
            {
                m_list[m_indexToEdit] = new Something(formerInfo.start, transformInfo, formerInfo.duration);
            }
        }
#endif
    }
}