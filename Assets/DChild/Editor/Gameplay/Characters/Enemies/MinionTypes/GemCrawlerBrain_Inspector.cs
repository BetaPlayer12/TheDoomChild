using DChild.Gameplay.Characters.Enemies;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Enemies
{
    [CustomEditor(typeof(GemCrawlerBrain))]
    public class GemCrawlerBrain_Inspector : OdinEditor
    {
        private Color m_chaseRangeColor;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_chaseRangeColor = new Color(1, 1, 0, 0.05f);
        }

        private void OnSceneGUI()
        {
            var brain = target as GemCrawlerBrain;
            Handles.color = m_chaseRangeColor;
            Handles.DrawSolidDisc(brain.transform.position, Vector3.forward, brain.chaseRange);
            var forward = Vector3.right * brain.transform.localScale.x;
            var startPoint = brain.transform.position;
            var attackDistanceEndPoint = startPoint + (forward * brain.attackRange);
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(30f, new Vector3[] { startPoint, attackDistanceEndPoint });
        }

    }

}
