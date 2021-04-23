using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChildDebug
{
    [CreateAssetMenu(fileName = "SkeletonDataMix", menuName = "DChild/Debug/Skeleton Data Mix")]
    public class SkeletonDataMix : ScriptableObject, IHasSkeletonDataAsset
    {
        [System.Serializable]
        public class Info
        {
            [SerializeField, SpineAnimation]
            private string m_fromAnimation;
            [SerializeField, SpineAnimation]
            private string m_toAnimation;
            [SerializeField, MinValue(0)]
            private float m_duration;

            public Info(string fromAnimation, string toAnimation, float duration)
            {
                m_fromAnimation = fromAnimation;
                m_toAnimation = toAnimation;
                m_duration = duration;
            }

            public string fromAnimation => m_fromAnimation;
            public string toAnimation => m_toAnimation;
            public float duration => m_duration;
        }

        [ShowInInspector]
        private SkeletonDataAsset m_asset;
        [SerializeField, TableList]
        private List<Info> m_customDurations;

        public SkeletonDataAsset SkeletonDataAsset => m_asset;

        private void Something()
        {
            //       				if (fromAnimation.arraySize > 0) {
            //	using (new SpineInspectorUtility.IndentScope()) {
            //		EditorGUILayout.LabelField("Custom Mix Durations", EditorStyles.boldLabel);
            //	}

            //	for (int i = 0; i < fromAnimation.arraySize; i++) {
            //		SerializedProperty from = fromAnimation.GetArrayElementAtIndex(i);
            //		SerializedProperty to = toAnimation.GetArrayElementAtIndex(i);
            //		SerializedProperty durationProp = duration.GetArrayElementAtIndex(i);
            //		using (new EditorGUILayout.HorizontalScope()) {
            //			GUILayout.Space(16f); // Space instead of EditorGUIUtility.indentLevel. indentLevel will add the space on every field.
            //			EditorGUILayout.PropertyField(from, GUIContent.none);
            //			//EditorGUILayout.LabelField(">", EditorStyles.miniLabel, GUILayout.Width(9f));
            //			EditorGUILayout.PropertyField(to, GUIContent.none);
            //			//GUILayout.Space(5f);
            //			durationProp.floatValue = EditorGUILayout.FloatField(durationProp.floatValue, GUILayout.MinWidth(25f), GUILayout.MaxWidth(60f));
            //			if (GUILayout.Button("Delete", EditorStyles.miniButton)) {
            //				duration.DeleteArrayElementAtIndex(i);
            //				toAnimation.DeleteArrayElementAtIndex(i);
            //				fromAnimation.DeleteArrayElementAtIndex(i);
            //			}
            //		}
            //	}
            //}

            //using (new EditorGUILayout.HorizontalScope()) {
            //	EditorGUILayout.Space();
            //	if (GUILayout.Button("Add Custom Mix")) {
            //		duration.arraySize++;
            //		toAnimation.arraySize++;
            //		fromAnimation.arraySize++;
            //	}
            //	EditorGUILayout.Space();
            //}
        }
#if UNITY_EDITOR
        [ShowIf("m_asset"),Button,PropertyOrder(1)]
        private void CopyCustomDurations()
        {
            var length = m_asset.fromAnimation.Length;
            if (m_customDurations == null)
            {
                m_customDurations = new List<Info>();
            }
            else
            {
                m_customDurations.Clear();
            }
            for (int i = 0; i < length; i++)
            {
                m_customDurations.Add(new Info(m_asset.fromAnimation[i], m_asset.toAnimation[i], m_asset.duration[i]));
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        [ShowIf("m_asset"), Button, PropertyOrder(1)]
        private void PasteCustomDurations()
        {
            m_asset.fromAnimation = m_customDurations.Select(x => x.fromAnimation).ToArray();
            m_asset.toAnimation = m_customDurations.Select(x => x.toAnimation).ToArray();
            m_asset.duration = m_customDurations.Select(x => x.duration).ToArray();

            EditorUtility.SetDirty(m_asset);
            AssetDatabase.SaveAssets();
        }
#endif
    }

}