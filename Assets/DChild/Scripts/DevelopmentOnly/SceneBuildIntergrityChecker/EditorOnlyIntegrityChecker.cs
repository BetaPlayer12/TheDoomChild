#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChildDebug
{
    public class EditorOnlyIntegrityChecker : IProcessSceneWithReport
    {
        public int callbackOrder => 2;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            var editorOnly = GameObject.Find("EditorOnly");
            if (editorOnly != null && editorOnly.CompareTag("EditorOnly") == false)
            {
                BuildIntegrityChecker.RecordReport(new BuildIntegrityReport(editorOnly.transform, editorOnly.gameObject, "Editor Only is not set to tag [Editor Only]"));
            }
        }
    }
}
#endif