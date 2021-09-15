#if UNITY_EDITOR
using System;
using UnityEngine;

namespace DChildDebug
{
    public class BuildIntegrityReport
    {
        public string gameObjectName { get; }
        public string componentName { get; }
        public string sceneName { get; }
        public string message { get; }

        public Exception exception { get; }

        public const string FILTERTAG = "[INTEGRITY REPORT]";

        public BuildIntegrityReport(Component component, GameObject gameObject, string message)
        {
            componentName = component.GetType().Name;
            gameObjectName = gameObject.name;
            sceneName = gameObject.scene.name;
            this.message = $"{FILTERTAG} ({sceneName}):: {gameObjectName} <{componentName}> \n" + message;
        }

        public BuildIntegrityReport(Component component, GameObject gameObject, string message, Exception exception)
        {
            componentName = component.GetType().Name;
            gameObjectName = gameObject.name;
            sceneName = gameObject.scene.name;
            this.message = $"{FILTERTAG} ({sceneName}):: {gameObjectName} <{componentName}> \n" + message + " " + exception.Message;
            this.exception = exception;
        }
    }
}
#endif