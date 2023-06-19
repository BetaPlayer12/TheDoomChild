using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DChildDebug.Spine.Tests
{
    public class MinionStressTestManager : MonoBehaviour
    {
        [SerializeField]
        private TextFileCreator m_textFileCreator;
        [SerializeField]
        private MinionStressTestInstantiator m_instantiator;
        //Fps Threshold
        [SerializeField]
        private float m_minPassingFPS;
        //MaxInstanceCount
        [SerializeField]
        private int m_maxInstanceCount;
        [SerializeField]
        private int m_passableInstanceCount;
        [SerializeField]
        private int m_instancesPerMonitoring = 1;
        [SerializeField]
        private float m_monitorPeriod = 1;
        [SerializeField]
        private float m_sanityTestPeriod = 600;
        //ListOfSkeletonData
        [SerializeField]
        private List<GameObject> m_minionPrefabs;

        private List<StressTestResult> m_results;

        private IEnumerator SpineStressTestRoutine(SkeletonAnimation skeletonData, string minionName)
        {
            if (skeletonData.SkeletonDataAsset == null)
            {
                Debug.LogError($"{minionName} Has No Skeleton Data");
            }
            else
            {
                var endofFrame = new WaitForEndOfFrame(); ;

                var fpsLog = new FPSLog();
                m_instantiator.DestroyAllInstances();
                yield return endofFrame;
                var totalInstanceCount = 0;
                var aveFPS = 0f;

                for (int i = 0; i < m_maxInstanceCount; i += m_instancesPerMonitoring)
                {
                    yield return InstantiateInstances(skeletonData.SkeletonDataAsset, i, endofFrame);
                    totalInstanceCount += m_instancesPerMonitoring;
                    yield return MonitorFPS(fpsLog, endofFrame, m_monitorPeriod);

                    yield return endofFrame;
                    aveFPS = fpsLog.aveFPS;
                    if (aveFPS > m_minPassingFPS)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                if (totalInstanceCount >= m_maxInstanceCount)
                {
                    yield return MonitorFPS(fpsLog, endofFrame, m_sanityTestPeriod);
                }

                var result = new StressTestResult(minionName, totalInstanceCount, fpsLog, GetResult(aveFPS, totalInstanceCount));

                m_results.Add(result);
                GenerateLog(minionName, totalInstanceCount, aveFPS);
                yield return null;
            }
        }

        private IEnumerator InstantiateInstances(SkeletonDataAsset skeletonDataAsset, int startingIndex, WaitForEndOfFrame endOfFrame)
        {
            for (int k = 0; k < m_instancesPerMonitoring; k++)
            {
                m_instantiator.InstantiateSpine(skeletonDataAsset, startingIndex + k);

                yield return endOfFrame;
            }
        }

        private IEnumerator MonitorFPS(FPSLog fpsLog, WaitForEndOfFrame endOfFrame, float duration)
        {
            var monitorDuration = 0f;
            fpsLog.ClearLog();
            do
            {
                fpsLog.Log(GetCurrentFPS());
                monitorDuration += Time.unscaledDeltaTime;
                yield return endOfFrame;
            } while (monitorDuration <= duration);
        }

        private IEnumerator TestRoutine()
        {
            Debug.Log("Test Starting in 5 Seconds...");
            yield return new WaitForSecondsRealtime(5f);
            Debug.Log("Test Start");
            m_results.Clear();
            for (int i = 0; i < m_minionPrefabs.Count; i++)
            {
                var minion = m_minionPrefabs[i];
                yield return SpineStressTestRoutine(minion.GetComponentInChildren<SkeletonAnimation>(), minion.name);
            }
            GenerateLogToFile();
        }

        private void GenerateLogToFile()
        {
            string currentDateAndTime = DateTime.Now.ToString().Replace("/", "_").Replace(":", "-");
            var failedResults = m_results.Where(x => x.result == "FAILED").ToList();
            failedResults.Sort((x, y) => x.instanceCount < y.instanceCount ? -1 : 1);
            var failedResultLog = "";
            for (int i = 0; i < failedResults.Count; i++)
            {
                failedResultLog += failedResults[i].ToString();
            }
            try
            {
                m_textFileCreator.CreateFile($"{currentDateAndTime}_FailedLog", failedResultLog);
            }
            catch (DirectoryNotFoundException ex)
            {
                Debug.LogError("OMG!!! Director not found!!! Cant make Fail Log!!!");
            }

            var passedResults = m_results.Where(x => x.result == "PASSING" || x.result == "STRESSFULY PASSING").ToList();
            failedResults.Sort((x, y) => x.instanceCount < y.instanceCount ? -1 : 1);
            var passedResultsLog = "";
            for (int i = 0; i < passedResults.Count; i++)
            {
                passedResultsLog += passedResults[i].ToString();
            }
            try
            {
                m_textFileCreator.CreateFile($"{currentDateAndTime}_PassedLog", passedResultsLog);
            }
            catch (DirectoryNotFoundException ex)
            {
                Debug.LogError("OMG!!! Director not found!!! Cant make Pass Log!!!");
            }
        }

        private string GetResult(float fps, int totalInstanceCount)
        {
            var hasPassedFPS = fps >= m_minPassingFPS;
            var hasPassedInstanceCount = totalInstanceCount >= m_passableInstanceCount;
            var hasreachedMaxInstanceCount = totalInstanceCount == m_maxInstanceCount;
            var result = "";

            if (hasPassedFPS)
            {
                if (hasreachedMaxInstanceCount)
                {
                    result = "PASSING";
                }
                else if (hasPassedInstanceCount)
                {
                    result = "ERROR 1";
                }
                else
                {
                    result = "ERROR 2";
                }
            }
            else
            {
                if (hasreachedMaxInstanceCount)
                {
                    result = "ERROR 3";
                }
                else if (hasPassedInstanceCount)
                {
                    result = "STRESSFULY PASSING";
                }
                else
                {
                    result = "FAILED";
                }
            }

            return result;
        }

        private void GenerateLog(string minionName, int totalInstanceCount, float lastFPS)
        {
            Debug.Log($"{GetResult(lastFPS, totalInstanceCount)}: ({totalInstanceCount}) {minionName} [{lastFPS}FPS]");
        }

        private float GetCurrentFPS()
        {
            return 1f / Time.unscaledDeltaTime;
        }

        public void Start()
        {
            m_results = new List<StressTestResult>();
            StartCoroutine(TestRoutine());
        }

    }
}