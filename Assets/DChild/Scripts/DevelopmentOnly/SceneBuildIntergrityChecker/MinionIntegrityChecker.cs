#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChildDebug
{
    public class MinionIntegrityChecker : IProcessSceneWithReport
    {
        public int callbackOrder => 1;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            var enemies = GameObject.Find("Enemies");
            if (enemies == null)
            {
                enemies = GameObject.Find("Enemy");
            }

            if (enemies != null)
            {
                if (enemies.activeInHierarchy == false)
                {
                    BuildIntegrityChecker.RecordReport(new BuildIntegrityReport(enemies.transform, enemies.gameObject, "Enemies Are Disabled"));
                }
            }
        }
    }
}
#endif