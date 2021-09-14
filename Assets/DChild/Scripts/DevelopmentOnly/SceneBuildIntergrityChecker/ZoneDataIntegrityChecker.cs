#if UNITY_EDITOR
using DChild.Serialization;
using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChildDebug
{
    public class ZoneDataIntegrityChecker : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        private void CheckZoneDataIntegrity(ZoneDataHandle zoneDataHandle)
        {
            if (zoneDataHandle == null)
                return;

            bool hasReportedNullComponentSerializers = false;
            var serializers = zoneDataHandle.GetComponentSerializers();
            foreach (var serializer in serializers)
            {
                if (serializer == null)
                {
                    if (hasReportedNullComponentSerializers == false)
                    {
                        BuildIntegrityChecker.RecordReport(new BuildIntegrityReport(zoneDataHandle, zoneDataHandle.gameObject, "Null ComponentSerializer"));
                        hasReportedNullComponentSerializers = true;
                    }
                }
                else
                {
                    try
                    {
                        serializer.SaveData();
                    }
                    catch (Exception e)
                    {
                        BuildIntegrityChecker.RecordReport(new BuildIntegrityReport(serializer, serializer.gameObject,"Error during simulated serialization:", e));
                    }
                }
            }
        }

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            CheckZoneDataIntegrity(GameObject.FindObjectOfType<ZoneDataHandle>());
        }
    }
}
#endif