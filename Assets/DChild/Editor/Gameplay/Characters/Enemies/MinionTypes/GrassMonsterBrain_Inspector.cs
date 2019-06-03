using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GrassMonsterBrain))]
[CanEditMultipleObjects]
public class GrassMonsterBrain_Inspector : OdinEditor {

    private void OnSceneGUI()
    {
        var brain = target as GrassMonsterBrain;
        var startPoint = brain.gameObject.GetComponentInChildren<BasicAggroSensor>().transform.position;
        var forward = Vector3.right * brain.transform.localScale.x;
        var attackRangeEnd = startPoint + (forward * brain.attackRange);

        Handles.color = Color.red;
        if (Application.isPlaying)
        {
            Handles.DrawAAPolyLine(10f, new Vector3[] { startPoint, attackRangeEnd });
        }
        else
        {
            Handles.DrawAAPolyLine(10f, new Vector3[] { startPoint, attackRangeEnd });
        }
        
        
    }
}
