using System;
using DChild.Gameplay.Combat;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace DChildEditor.Toolkit
{
    public class PlayerVisualConstructor
    {
        private static bool m_hasBasicFX;

        public static void Reset()
        {
            m_hasBasicFX = false;
        }

        public static void UpdateConstructor(PlayerConstructor window)
        {
            m_hasBasicFX = window.player.GetComponentInChildren<Hitbox>();
        }

        public static void DrawInspector(PlayerConstructor window)
        {
            var windowWidth = window.position.width;
            SirenixEditorGUI.BeginBox("Visual", false, GUILayout.Width(windowWidth));

            SirenixEditorGUI.EndBox();
        }
    }
}