using System;
using UnityEngine;

namespace DChildEditor.Toolkit
{
    public static class ConstructorCommands
    {
        public static void DrawButton(bool reference, string buttonLabel,GameObject player, Action<GameObject> action)
        {
            if(reference == false)
            {
                if (GUILayout.Button(buttonLabel))
                {
                    action(player);
                }
            }
        }
    }
}