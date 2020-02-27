using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Spine;
using Sirenix.Utilities;
using UnityEngine.Profiling;

namespace DChild
{
    public class SpineAnimationManager : MonoBehaviour
    {
        public class BoneTracker
        {
            private Dictionary<SkeletonAnimation, Cache<List<Bone>>> m_pair;
            private List<Cache<List<Bone>>> m_cacheList;

            public BoneTracker()
            {
                m_pair = new Dictionary<SkeletonAnimation, Cache<List<Bone>>>();
                m_cacheList = new List<Cache<List<Bone>>>();
            }

            public void AddToTracker(SkeletonAnimation animation)
            {
                if (m_pair.ContainsKey(animation) == false)
                {
                    var list = Cache<List<Bone>>.Claim();
                    list.Value.Clear();
                    m_pair.Add(animation, list);
                    m_cacheList.Add(list);
                }
            }

            public List<Bone> GetBones(SkeletonAnimation animation)
            {
                if (m_pair.ContainsKey(animation))
                {
                    return m_pair[animation];
                }
                return null;
            }

            public void RemoveFromTracker(SkeletonAnimation animation)
            {
                if (m_pair.ContainsKey(animation))
                {
                    var list = m_pair[animation];
                    list.Release();
                    m_pair.Remove(animation);
                    m_cacheList.Remove(list);
                }
            }

            public void Clear()
            {
                m_pair.Clear();
                for (int i = 0; i < m_cacheList.Count; i++)
                {
                    m_cacheList[i].Release();
                }
                m_cacheList.Clear();
            }
        }

        public class UpdateTracker
        {
            private class Info
            {
                public SkeletonAnimation m_source { get; private set; }
                public float targetDeltaTime { get; private set; }
                public float deltaTime;
                public bool canBeUpdated;

                private int currentFPS;

                public void Initialize(SkeletonAnimation animation, bool canBeUpdated)
                {
                    m_source = animation;
                    targetDeltaTime = (1f / 60);
                    deltaTime = 0;
                    this.canBeUpdated = canBeUpdated;
                    currentFPS = 60;
                }

                public void SetFPS(int fps)
                {
                    if (currentFPS != fps)
                    {
                        targetDeltaTime = (1f / fps);
                        EvaluateUpdateStatus();
                        currentFPS = fps;
                    }
                }

                public void Update(float deltaTime)
                {
                    this.deltaTime += deltaTime;
                    EvaluateUpdateStatus();
                }

                private void EvaluateUpdateStatus()
                {
                    if (canBeUpdated == false)
                    {
                        canBeUpdated = this.deltaTime > targetDeltaTime;
                        if (canBeUpdated)
                        {
                            this.deltaTime -= targetDeltaTime;
                        }
                    }
                }
            }

            private Dictionary<SkeletonAnimation, Cache<Info>> m_pair;
            private List<Cache<Info>> m_infoList;

            public UpdateTracker()
            {
                m_pair = new Dictionary<SkeletonAnimation, Cache<Info>>();
                m_infoList = new List<Cache<Info>>();
            }

            public void AddToTracker(SkeletonAnimation animation)
            {
                if (m_pair.ContainsKey(animation) == false)
                {
                    var cache = Cache<Info>.Claim();
                    cache.Value.Initialize(animation, true);
                    m_pair.Add(animation, cache);
                    m_infoList.Add(cache);
                }
            }

            public void RemoveFromTracker(SkeletonAnimation animation)
            {
                if (m_pair.ContainsKey(animation))
                {
                    var cache = m_pair[animation];
                    cache.Release();
                    m_pair.Remove(animation);
                    m_infoList.Remove(cache);
                }
            }

            public void Update(int index, float deltaTime)
            {
                var info = m_infoList[index];
                var fps = info.Value.m_source.isVisible ? 60 : 30;
                info.Value.SetFPS(fps);
                info.Value.Update(deltaTime);
            }

            public bool CanBeUpdated(SkeletonAnimation animation)
            {
                if (m_pair.ContainsKey(animation))
                {
                    return m_pair[animation].Value.canBeUpdated;
                }
                else
                {
                    return false;
                }
            }
            public float GetTargetDeltaTime(SkeletonAnimation animation)
            {
                if (m_pair.ContainsKey(animation))
                {
                    return m_pair[animation].Value.targetDeltaTime;
                }
                else
                {
                    return 0f;
                }
            }

            public void ClearFlags()
            {
                for (int i = 0; i < m_infoList.Count; i++)
                {
                    m_infoList[i].Value.canBeUpdated = false;
                }
            }
        }

        public static SpineAnimationManager m_instance;
        private static List<SkeletonAnimation> m_animations;
        private static BoneTracker m_boneTracker;
        private static UpdateTracker m_updateTracker;
        private static List<int> m_updateAfterWorldUpdateIndexList;
        private static int m_animationCount;
        private SkeletonAnimation m_cacheAnimation;

        private NativeArray<SkeletonTime> m_skeletonTimesArray;
        private JobHandle m_deltaTimeUpdateHandle;

        private List<NativeArray<SkeletonUpdateInstruction>> m_skelInstructionList;
        private List<NativeArray<SkeletonBone>> m_skelBoneList;
        private List<NativeArray<SkeletonIKConstraint>> m_skelIkList;
        private List<NativeArray<SkeletonTransformConstraint>> m_skelTransformList;
        private List<NativeArray<SkeletonBoneIndex>> m_skelTransformBonesList;
        private NativeArray<JobHandle> m_updateInfoHandleArray;
        public static void Register(SkeletonAnimation animation)
        {

            if (m_instance)
            {
                if (m_animations.Contains(animation) == false)
                {
                    animation.enabled = false;
                    m_animationCount++;
                    m_animations.Add(animation);
                    m_updateTracker.AddToTracker(animation);
                }
            }
        }

        public static void Unregister(SkeletonAnimation animation)
        {
            if (m_instance)
            {
                if (m_animations.Contains(animation))
                {
                    animation.enabled = true;
                    m_animationCount--;
                    m_animations.Remove(animation);
                    m_updateTracker.RemoveFromTracker(animation);
                }
            }
        }
        private void ApplySkeletonUpdate(out List<Bone> cacheBone, int animationIndex, int jobIndex)
        {
            m_updateInfoHandleArray[jobIndex].Complete();
            m_cacheAnimation = m_animations[animationIndex];
            cacheBone = m_boneTracker.GetBones(m_cacheAnimation);
            var skelBones = m_skelBoneList[jobIndex];

            for (int j = 0; j < cacheBone.Count; j++)
            {
                var bone = cacheBone[j];
                var skelBone = skelBones[j];
                bone.ax = skelBone.ax;
                bone.ay = skelBone.ay;
                bone.arotation = skelBone.arotation;
                bone.ascaleX = skelBone.ascaleX;
                bone.ascaleY = skelBone.ascaleY;
                bone.ashearX = skelBone.ashearX;
                bone.ashearY = skelBone.ashearY;
                bone.appliedValid = skelBone.appliedValid;
                bone.x = skelBone.x;
                bone.y = skelBone.y;
                bone.rotation = skelBone.rotation;
                bone.scaleX = skelBone.scaleX;
                bone.scaleY = skelBone.scaleY;
                bone.shearX = skelBone.shearX;
                bone.shearY = skelBone.shearY;
                bone.a = skelBone.a;
                bone.b = skelBone.b;
                bone.c = skelBone.c;
                bone.d = skelBone.d;
                bone.worldX = skelBone.worldX;
                bone.worldY = skelBone.worldY;
            }
        }

        private void ScheduleSkeletonUpdateHandle(int animationIndex, int jobindex)
        {
            // THis is Heavy
            m_cacheAnimation = m_animations[animationIndex];
            m_boneTracker.AddToTracker(m_cacheAnimation);
            Profiler.BeginSample("Create Native Arrays");

            UpdateWorldTransformJob.CreateNativeArrays(m_cacheAnimation, m_boneTracker.GetBones(m_cacheAnimation), out NativeArray<SkeletonUpdateInstruction> instruction,
                                                        out NativeArray<SkeletonBone> skelBone, out NativeArray<SkeletonIKConstraint> ik, out NativeArray<SkeletonTransformConstraint> nTransform, out NativeArray<SkeletonBoneIndex> transformBone);
            Profiler.EndSample();
            m_skelInstructionList.Add(instruction);
            m_skelBoneList.Add(skelBone);
            m_skelIkList.Add(ik);
            m_skelTransformList.Add(nTransform);
            m_skelTransformBonesList.Add(transformBone);
            var job = new UpdateWorldTransformJob(instruction, skelBone, ik, nTransform, transformBone);
            m_updateInfoHandleArray[jobindex] = job.Schedule();

        }

        private void ScheduleDeltaUpdateHandle()
        {
            m_skeletonTimesArray = new NativeArray<SkeletonTime>(m_animationCount, Allocator.TempJob);
            for (int i = 0; i < m_animationCount; i++)
            {
                m_skeletonTimesArray[i] = new SkeletonTime(m_animations[i]);
            }
            m_deltaTimeUpdateHandle = new DeltaTimeUpdateJob(m_skeletonTimesArray, Time.deltaTime).Schedule(m_animationCount, 10);
        }

        private void ApplyDeltaUpdate()
        {
            m_deltaTimeUpdateHandle.Complete();
            for (int i = 0; i < m_animationCount; i++)
            {
                m_cacheAnimation = m_animations[i];
                if (m_cacheAnimation.valid)
                {
                    m_cacheAnimation.skeleton.Time = m_skeletonTimesArray[i].time;
                    m_cacheAnimation.state.Update(m_skeletonTimesArray[i].deltaTime);
                    m_cacheAnimation.state.Apply(m_cacheAnimation.skeleton);

                    m_cacheAnimation.CallUpdateLocal();
                }
            }
            m_skeletonTimesArray.Dispose();
        }

        private void ScheduleAllSkeletonUpdateHandle()
        {
            m_updateInfoHandleArray = new NativeArray<JobHandle>(m_animationCount, Allocator.TempJob);
            for (int i = 0; i < m_animationCount; i++)
            {
                ScheduleSkeletonUpdateHandle(i, i);
            }
        }
        private void ApplyAllSkeletonUpdate()
        {
            List<Bone> cacheBone;
            //Apply the Thingies
            for (int i = 0; i < m_animationCount; i++)
            {
                ApplySkeletonUpdate(out cacheBone, i, i);
            }
            DisposeSkeletonUpdate();
        }

        private void DisposeSkeletonUpdate()
        {
            m_updateInfoHandleArray.Dispose();
            for (int i = 0; i < m_skelInstructionList.Count; i++)
            {
                m_skelInstructionList[i].Dispose();
                m_skelBoneList[i].Dispose();
                m_skelIkList[i].Dispose();
                m_skelTransformList[i].Dispose();
                m_skelTransformBonesList[i].Dispose();
            }
            m_skelInstructionList.Clear();
            m_skelBoneList.Clear();
            m_skelIkList.Clear();
            m_skelTransformList.Clear();
            m_skelTransformBonesList.Clear();
            m_boneTracker.Clear();
        }

        private static void CallUpdateWorld()
        {
            m_updateAfterWorldUpdateIndexList.Clear();
            for (int i = 0; i < m_animationCount; i++)
            {
                if (m_animations[i].CallUpdateWorld())
                {
                    m_updateAfterWorldUpdateIndexList.Add(i);
                }
            }
        }

        private void ScheduleSKeletonUpdateAfterWorld()
        {
            var count = m_updateAfterWorldUpdateIndexList.Count;
            m_updateInfoHandleArray = new NativeArray<JobHandle>(count, Allocator.TempJob);
            for (int i = 0; i < count; i++)
            {
                ScheduleSkeletonUpdateHandle(m_updateAfterWorldUpdateIndexList[i], i);
            }
        }

        private void ApplySkeletonUpdateAfterWorld()
        {
            List<Bone> cacheBone;
            //Apply the Thingies
            for (int i = 0; i < m_updateAfterWorldUpdateIndexList.Count; i++)
            {
                ApplySkeletonUpdate(out cacheBone, m_updateAfterWorldUpdateIndexList[i], i);
            }
            m_updateAfterWorldUpdateIndexList.Clear();
            DisposeSkeletonUpdate();
        }

        private static void CallUpdateComplete()
        {
            for (int i = 0; i < m_animationCount; i++)
            {
                m_animations[i].CallUpdateComplete();
            }
        }


        private void Awake()
        {
            if (m_instance && m_instance != this)
            {
                Destroy(this);
            }
            else
            {
                m_instance = this;
                m_animations = new List<SkeletonAnimation>();
                m_skelInstructionList = new List<NativeArray<SkeletonUpdateInstruction>>();
                m_skelBoneList = new List<NativeArray<SkeletonBone>>();
                m_skelIkList = new List<NativeArray<SkeletonIKConstraint>>();
                m_skelTransformList = new List<NativeArray<SkeletonTransformConstraint>>();
                m_skelTransformBonesList = new List<NativeArray<SkeletonBoneIndex>>();
                m_boneTracker = new BoneTracker();
                m_updateTracker = new UpdateTracker();
                m_updateAfterWorldUpdateIndexList = new List<int>();
            }
        }


        private void Update()
        {
            //SkeletonAnimation cacheAnimation;
            //for (int i = 0; i < m_animationCount; i++)
            //{
            //    m_updateTracker.Update(i,Time.deltaTime);
            //    cacheAnimation = m_animations[i];
            //    if (m_updateTracker.CanBeUpdated(cacheAnimation))
            //    {
            //        cacheAnimation.Update(m_updateTracker.GetTargetDeltaTime(cacheAnimation));
            //    }
            //}

            //MultiThread;
            ScheduleDeltaUpdateHandle();

            //CallSomewhere Else
            ApplyDeltaUpdate();

            //Schedule World Transform Job
            ScheduleAllSkeletonUpdateHandle();
            //Call AI Manager To Update
            ApplyAllSkeletonUpdate();
            //Call Update World
            CallUpdateWorld();

            ScheduleSKeletonUpdateAfterWorld();

            ApplySkeletonUpdateAfterWorld();
            CallUpdateComplete();
        }

        private void LateUpdate()
        {
            for (int i = 0; i < m_animations.Count; i++)
            {
                m_animations[i].LateUpdate();
            }

            //SkeletonAnimation cacheAnimation;
            //for (int i = 0; i < m_animationCount; i++)
            //{
            //    cacheAnimation = m_animations[i];
            //    if (m_updateTracker.CanBeUpdated(cacheAnimation))
            //    {
            //        cacheAnimation.LateUpdate();
            //    }
            //}
            //m_updateTracker.ClearFlags();
        }
    }
}