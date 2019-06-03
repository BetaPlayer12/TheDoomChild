using DChild.Gameplay;
using DChild.Gameplay.Pathfinding;
using DChild.Gameplay.Systems.WorldComponents;
using Pathfinding;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Toolkit
{
    public class ObjectCreationToolkit
    {
        [MenuItem("Tools/Kit/Create/Isolated Object")]
        private static void CreateIsolatedObject()
        {
            var objectGO = Commands.CreateGameObject("Isolated Object", null, Vector3.zero);
            objectGO.AddComponent<Rigidbody2D>();
            objectGO.AddComponent<IsolatedObject>();
            objectGO.AddComponent<IsolatedObjectPhysics2D>();
            Selection.activeGameObject = objectGO;
        }

        [MenuItem("Tools/Kit/Create/Isolated Floating Object")]
        private static void CreateIsolatedFloatingObject()
        {
            var objectGO = Commands.CreateGameObject("Isolated Floating Object", null, Vector3.zero);
            objectGO.AddComponent<Rigidbody2D>();
            objectGO.AddComponent<IsolatedObject>();
            var physics = objectGO.AddComponent<IsolatedObjectPhysics2D>();
            physics.simulateGravity = false;
            Selection.activeGameObject = objectGO;
        }

        [MenuItem("Tools/Kit/Create/Isolated Character")]
        private static void CreateIsolatedCharacter()
        {
            var objectGO = Commands.CreateGameObject("IsolatedObject", null, Vector3.zero);
            objectGO.AddComponent<Rigidbody2D>();
            objectGO.AddComponent<IsolatedObject>();
            var physics = objectGO.AddComponent<IsolatedCharacterPhysics2D>();
            Selection.activeGameObject = objectGO;
        }

        [MenuItem("Tools/Kit/Create/Isolated Flying Character")]
        private static void CreateIsolatedFlyingObject()
        {
            var objectGO = Commands.CreateGameObject("Isolated Flying Object", null, Vector3.zero);
            objectGO.AddComponent<Rigidbody2D>();
            objectGO.AddComponent<IsolatedObject>();
            var physics = objectGO.AddComponent<IsolatedObjectPhysics2D>();
            physics.simulateGravity = false;
            objectGO.AddComponent<Seeker>();
            objectGO.AddComponent<NavigationTracker>();
            Selection.activeGameObject = objectGO;
        }
    }
}