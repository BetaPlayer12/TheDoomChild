#if UNITY_EDITOR
using DChild.Gameplay.Optimization.Lights;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChildDebug
{
    public class SectionLightingIntegriyChecker : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        private void CheckIntegrity(SectionLighting sectionLighting)
        {
            if (sectionLighting.HasMissingLights())
            {
                BuildIntegrityChecker.RecordReport(new BuildIntegrityReport(sectionLighting, sectionLighting.gameObject, "Null Lights in List"));
            }
        }

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            var sectionLightings = GameObject.FindObjectsOfType<SectionLighting>(true);
            for (int i = 0; i < sectionLightings.Length; i++)
            {
                CheckIntegrity(sectionLightings[i]);
            }
        }
    }
}
#endif