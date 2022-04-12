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
            public bool drawLabel;
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

            var creature = (target as ComplexIdlingCreature);
            var gameObject = creature.gameObject;
            var objectName = gameObject.name;
            var startPosition = Application.isPlaying ? creature.startPosition : gameObject.transform.position;

            movementList.Add(new Info() { destination = startPosition });
            AddIdlingBehaviourMovement(startPosition);
            DrawMovement($"Idle-{objectName}");

            movementList.Clear();

            Handles.color = Color.cyan;
            movementList.Add(new Info() { destination = startPosition, drawLabel = false });
            AddReactingBehaviourMovement(startPosition);
            DrawMovement($"React-{objectName}");
        }

        private void AddIdlingBehaviourMovement(Vector3 currentPosition)
        {
            var idlingProp = Tree.GetPropertyAtUnityPath("m_idlingBehaviour");
            var idlingBehaviours = (ComplexIdlingCreature.IBehaviour[])idlingProp.ValueEntry.WeakSmartValue;


            for (int i = 0; i < idlingBehaviours.Length; i++)
            {
                if (idlingBehaviours[i] is ComplexIdlingCreature.MovingBehaviour)
                {
                    RecordMovementSegment(currentPosition, idlingBehaviours, i);
                }
            }
        }

        private void AddReactingBehaviourMovement(Vector3 currentPosition)
        {
            var reactProp = Tree.GetPropertyAtUnityPath("m_reactBehaviour");
            var reactBehaviours = (ComplexIdlingCreature.IBehaviour[])reactProp.ValueEntry.WeakSmartValue;
            AddReactingBehaviourMovement(currentPosition, reactBehaviours);
        }

        private static void AddReactingBehaviourMovement(Vector3 currentPosition, ComplexIdlingCreature.IBehaviour[] reactBehaviours)
        {
            for (int i = 0; i < reactBehaviours.Length; i++)
            {
                var reactBehaviour = reactBehaviours[i];
                if (reactBehaviour is ComplexIdlingCreature.MovingBehaviour)
                {
                    RecordMovementSegment(currentPosition, reactBehaviours, i);
                }
                else if (reactBehaviour is ComplexIdlingCreature.BehaviourList)
                {
                    AddReactingBehaviourMovement(currentPosition, ((ComplexIdlingCreature.BehaviourList)reactBehaviour).behaviours);
                }
            }
        }

        private static void RecordMovementSegment(Vector3 currentPosition, ComplexIdlingCreature.IBehaviour[] idlingBehaviours, int i)
        {
            var behaviour = (ComplexIdlingCreature.MovingBehaviour)idlingBehaviours[i];
            if (Application.isPlaying == false && Event.current.shift == false)
            {
                behaviour.destination = behaviour.m_relativeDestination + currentPosition;
            }
            behaviour.destination = Handles.DoPositionHandle(behaviour.destination, Quaternion.identity);
            if (behaviour.useSlerp)
            {
                var origin = movementList[movementList.Count - 1].destination;
                for (float k = 0.1f; k <= 1; k += 0.1f)
                {
                    var segmentInfo = new Info()
                    {
                        destination = behaviour.EvaluateLerp(origin, behaviour.destination, k),
                        index = i,
                        animation = behaviour.animation,
                        drawLabel = false
                    };
                    movementList.Add(segmentInfo);
                }

                var info = new Info()
                {
                    destination = behaviour.destination,
                    index = i,
                    animation = behaviour.animation,
                    drawLabel = true
                };
                movementList.Add(info);
            }
            else
            {
                var info = new Info()
                {
                    destination = behaviour.destination,
                    index = i,
                    animation = behaviour.animation,
                    drawLabel = true
                };
                movementList.Add(info);
            }
            behaviour.m_relativeDestination = behaviour.destination - currentPosition;
        }

        private static void DrawMovement(string objectName)
        {
            if (movementList.Count >= 2)
            {
                Handles.DrawAAPolyLine(10f, movementList.Select(x => x.destination).ToArray());
                for (int i = 1; i < movementList.Count; i++)
                {
                    var info = movementList[i];
                    if (info.drawLabel)
                    {
                        Handles.Label(info.destination, $"{objectName}<{info.index}-{info.animation}>");
                    }
                }
                Handles.Label(movementList[0].destination, $"Hold Shift to move this seperately");
            }
        }


    }

}