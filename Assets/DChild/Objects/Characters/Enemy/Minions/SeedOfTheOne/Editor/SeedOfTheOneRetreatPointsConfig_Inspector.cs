using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Characters.AI
{
    [CustomEditor(typeof(SeedOfTheOneRetreatPointsConfiguration))]
    public class SeedOfTheOneRetreatPointsConfig_Inspector : OdinEditor
    {
        private void DrawLabels(Vector2[] retreatPoints)
        {
            for(int i = 0; i < retreatPoints.Length; i++)
            {
                Handles.Label(retreatPoints[i], "Retreat Point " + i.ToString());
            }
        }

        private void OnSceneGUI()
        {
            if (UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                var seedOfTheOneRetreatPointConfig = target as SeedOfTheOneRetreatPointsConfiguration;

                Handles.color = Color.yellow;

                var retreatPoints = seedOfTheOneRetreatPointConfig.retreatPoints;

                EditorGUI.BeginChangeCheck();
                for(int i = 0; i < seedOfTheOneRetreatPointConfig.retreatPoints.Length; i++)
                {
                    retreatPoints[i] = Handles.FreeMoveHandle(retreatPoints[i], Quaternion.identity,
                        3f, Vector3.one * 0.05f, Handles.ConeHandleCap);
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(seedOfTheOneRetreatPointConfig);
                }

                DrawLabels(retreatPoints);
            }
        }
    }

}
