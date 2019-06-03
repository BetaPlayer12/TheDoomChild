using DChild.Gameplay.Environment.Obstacles;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Environment.Obstacles
{
    [CustomEditor(typeof(ExtendingObstacle))]
    public class ExtendingObstacle_Inspector : OdinEditor
    {
        protected void OnSceneGUI()
        {
            var extendingObstacle = target as ExtendingObstacle;

            int extractHandleID = 5;
            Handles.color = new Color(0.5f, 0.5f, 0, 1f);
            extendingObstacle.extendPosition = Handles.FreeMoveHandle(extractHandleID, extendingObstacle.extendPosition, Quaternion.identity, 0.5f, Vector3.one * 0.5f, Handles.CubeHandleCap);
            Handles.Label(extendingObstacle.extendPosition, "Extend");

            int retractHandleID = 10;
            Handles.color = new Color(0.25f, 0.25f, 0.5f, 1f);
            extendingObstacle.retractPosition = Handles.FreeMoveHandle(retractHandleID, extendingObstacle.retractPosition, Quaternion.identity, 0.5f, Vector3.one * 0.5f, Handles.CubeHandleCap);
            Handles.Label(extendingObstacle.retractPosition, "Retract");

            if (GUIUtility.hotControl == extractHandleID)
            {
                extendingObstacle.transform.position = extendingObstacle.extendPosition;
            }
            else if (GUIUtility.hotControl == retractHandleID)
            {
                extendingObstacle.transform.position = extendingObstacle.retractPosition;
            }


            Handles.color = new Color(0, 1, 0, 0.1f);
            Handles.DrawAAPolyLine(extendingObstacle.extendPosition, extendingObstacle.retractPosition);
        }
    }
}