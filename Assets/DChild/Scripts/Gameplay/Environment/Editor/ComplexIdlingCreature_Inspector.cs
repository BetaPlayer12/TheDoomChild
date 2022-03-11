using DChild.Gameplay.Environment;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Environment
{
    [CustomEditor(typeof(ComplexIdlingCreature))]
    public class ComplexIdlingCreature_Inspector : OdinEditor
    {
        private struct Info
        {
            public Vector3 destination;
            public int index;
            public string animation;
        }

        private static List<Info> movementList;

        protected override void DrawTree()
        {
            base.DrawTree();

            var gameObject = (target as ComplexIdlingCreature).gameObject;
            var currentPosition = gameObject.transform.position;
            var idlingProp = Tree.GetPropertyAtUnityPath("m_idlingBehaviour");
            var behaviours = (ComplexIdlingCreature.IBehaviour[])idlingProp.ValueEntry.WeakSmartValue;
            for (int i = 0; i < behaviours.Length; i++)
            {
                if (behaviours[i] is ComplexIdlingCreature.MovingBehaviour)
                {
                    var behaviour = (ComplexIdlingCreature.MovingBehaviour)behaviours[i];
                    behaviour.m_relativeDestination = behaviour.destination - currentPosition;
                }
            }

            Tree.ApplyChanges();
        }

        private void OnSceneGUI()
        {
            if (movementList == null)
            {
                movementList = new List<Info>();
            }
            else
            {
                movementList.Clear();
            }

            var gameObject = (target as ComplexIdlingCreature).gameObject;
            var objectName = gameObject.name;
            var currentPosition = gameObject.transform.position;
            movementList.Add(new Info() { destination = currentPosition });
            AddIdlingBehaviourMovement(currentPosition);
            DrawIdlingMovement(objectName);
        }

        private void AddIdlingBehaviourMovement(Vector3 currentPosition)
        {
            var idlingProp = Tree.GetPropertyAtUnityPath("m_idlingBehaviour");
            var idlingBehaviours = (ComplexIdlingCreature.IBehaviour[])idlingProp.ValueEntry.WeakSmartValue;


            for (int i = 0; i < idlingBehaviours.Length; i++)
            {
                if (idlingBehaviours[i] is ComplexIdlingCreature.MovingBehaviour)
                {
                    var behaviour = (ComplexIdlingCreature.MovingBehaviour)idlingBehaviours[i];
                    if (Event.current.shift == false)
                    {
                        behaviour.destination = behaviour.m_relativeDestination + currentPosition;
                    }
                    behaviour.destination = Handles.DoPositionHandle(behaviour.destination, Quaternion.identity);
                    var info = new Info()
                    {
                        destination = behaviour.destination,
                        index = i,
                        animation = behaviour.animation
                    };
                    movementList.Add(info);
                    behaviour.m_relativeDestination = behaviour.destination - currentPosition;
                }
            }
        }

        private static void DrawIdlingMovement(string objectName)
        {
            if (movementList.Count >= 2)
            {
                Handles.DrawAAPolyLine(10f, movementList.Select(x => x.destination).ToArray());
                for (int i = 1; i < movementList.Count; i++)
                {
                    var info = movementList[i];
                    Handles.Label(info.destination, $"{objectName}<{info.index}-{info.animation}>");
                }
                Handles.Label(movementList[0].destination, $"Hold Shift to move this seperately");
            }
        }

        private void AddReactingBehaviourMovement(Vector3 currentPosition)
        {
            var reactProp = Tree.GetPropertyAtUnityPath("m_reactBehaviour");
            var reactBehaviours = (ComplexIdlingCreature.IBehaviour[])reactProp.ValueEntry.WeakSmartValue;


            for (int i = 0; i < reactBehaviours.Length; i++)
            {
                var reactBehaviour = reactBehaviours[i];
                if (reactBehaviour is ComplexIdlingCreature.MovingBehaviour)
                {
                    var behaviour = (ComplexIdlingCreature.MovingBehaviour)reactBehaviour;
                    if (Event.current.shift == false)
                    {
                        behaviour.destination = behaviour.m_relativeDestination + currentPosition;
                    }
                    behaviour.destination = Handles.DoPositionHandle(behaviour.destination, Quaternion.identity);
                    var info = new Info()
                    {
                        destination = behaviour.destination,
                        index = i,
                        animation = behaviour.animation
                    };
                    movementList.Add(info);
                    behaviour.m_relativeDestination = behaviour.destination - currentPosition;
                }
                else
                {

                }
            }
        }
    }

}