using DChild.Gameplay.Environment;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Environment
{
    [CustomEditor(typeof(DamagingExplosion))]
    public class DamagingExplosion_Inspector : OdinEditor
    {
        protected void OnSceneGUI()
        {
            var explosion = target as DamagingExplosion;
            Handles.color = new Color(0, 1, 1, 0.1f);
            Handles.DrawSolidDisc(explosion.transform.position, Vector3.forward, explosion.radius);
            Handles.color = new Color(1, 0, 0, 0.1f);
            Handles.DrawSolidDisc(explosion.transform.position, Vector3.forward, explosion.damageRadius);
        }
    }

}