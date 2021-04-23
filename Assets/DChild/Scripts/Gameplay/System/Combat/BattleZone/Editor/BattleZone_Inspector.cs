using DChild.Gameplay.Combat;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DChild.Gameplay.Combat.BattleZoneComponents;
using UnityEditor.Experimental.SceneManagement;

namespace DChildEditor.Gameplay.Combat
{
    [CustomEditor(typeof(BattleZone))]
    public class BattleZone_Inspector : OdinEditor
    {
        private void OnSceneGUI()
        {
            if (PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                var wavesProp = Tree.GetPropertyAtUnityPath("m_waves");
                var waveEntry = (WaveInfo[])(IList<WaveInfo>)wavesProp.ValueEntry.WeakSmartValue;
                EditorGUI.BeginChangeCheck();
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.white;

                var battleZone = target as BattleZone;
                for (int i = 0; i < waveEntry.Length; i++)
                {
                    if (waveEntry[i].showGizmos)
                    {
                        var spawnInfo = waveEntry[i].spawnInfo;
                        for (int k = 0; k < spawnInfo.Length; k++)
                        {
                            var color = Color.HSVToRGB(14.4f * k / 360, 1, 1);
                            color.a = 1f;
                            Handles.color = color;
                            var datas = spawnInfo[k].datas;
                            for (int j = 0; j < datas.Length; j++)
                            {
                                var data = datas[j];
                                data.spawnLocation = Handles.FreeMoveHandle(data.spawnLocation, Quaternion.identity, 1f, Vector3.one * 0.05f, Handles.CubeHandleCap);
                                Handles.Label(data.spawnLocation + Vector2.left * 10f, $"W{i}S{k}P{j} - {data.spawnDelay}s", style);
                                datas[j] = data;
                            }
                        }
                    }
                }
                if (EditorGUI.EndChangeCheck())
                {
                    //Record Undo
                }

                EditorUtility.SetDirty(target);
            }
        }
    }
}