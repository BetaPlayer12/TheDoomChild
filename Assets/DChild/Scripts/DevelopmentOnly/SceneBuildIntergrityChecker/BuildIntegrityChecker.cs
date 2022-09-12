#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace DChildDebug
{

    public class BuildIntegrityChecker : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        private static List<BuildIntegrityReport> reports = new List<BuildIntegrityReport>();

        public static void RecordReport(BuildIntegrityReport report)
        {
            reports.Add(report);
        }

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            reports.Clear();
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            if (reports.Count > 0)
            {
                EditorUtility.DisplayDialog("Build Integrity Error", $"The Integrity of the Build is compromised Check the Error Console Log with the filter, {BuildIntegrityReport.FILTERTAG}", "Ok, I'll do it");
                reports = reports.OrderBy(x => x.sceneName).ThenBy(x => x.gameObjectName).ToList();
                Debug.Log("---------INTEGRITY REPORT START---------");
                for (int i = 0; i < reports.Count; i++)
                {
                    var cacheReport = reports[i];
                        Debug.Log(cacheReport.message);
                    if (cacheReport.exception != null)
                    {
                        Debug.LogException(cacheReport.exception);
                    }
                }
                Debug.Log("---------INTEGRITY REPORT END---------");
            }
        }
    }
}
#endif