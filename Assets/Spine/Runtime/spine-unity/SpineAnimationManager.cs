using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Spine;
using System;
using Sirenix.Utilities;

namespace DChild
{
    public class SpineAnimationManager : MonoBehaviour
    {
        public class BoneTracker
        {
            private Dictionary<SkeletonAnimation, Cache<List<Bone>>> m_pair;

            public BoneTracker()
            {
                m_pair = new Dictionary<SkeletonAnimation, Cache<List<Bone>>>();
            }

            public void AddToTracker(SkeletonAnimation animation)
            {
                if (m_pair.ContainsKey(animation) == false)
                {
                    var list = Cache<List<Bone>>.Claim();
                    list.Value.Clear();
                    m_pair.Add(animation, list);
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
                    m_pair[animation].Release();
                    m_pair.Remove(animation);
                }
            }

            public void Clear()
            {
                foreach (var item in m_pair.Keys)
                {
                    m_pair[item].Release();
                }
                m_pair.Clear();
            }
        }

        public static SpineAnimationManager m_instance;
        private static List<SkeletonAnimation> m_animations;
        private static BoneTracker m_boneTracker;
        private static List<int> m_updateAfterWorldUpdateIndexList;
        private static int m_animationCount;
        private SkeletonAnimation m_cacheAnimation;

        private NativeArray<SkeletonTime> m_skeletonTimesArray;
        private JobHandle m_deltaTimeUpdateHandle;

        private List<NativeArray<SkeletonUpdateInstruction>> m_skelInstructionList;
        private List<NativeArray<SkeletonBone>> m_skelBoneList;
        private List<NativeArray<SkeletonIKConstraint>> m_skelIkList;
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
            m_cacheAnimation = m_animations[animationIndex];
            m_boneTracker.AddToTracker(m_cacheAnimation);
            UpdateWorldTransformJob.CreateNativeArrays(m_cacheAnimation, m_boneTracker.GetBones(m_cacheAnimation), out NativeArray<SkeletonUpdateInstruction> instruction, out NativeArray<SkeletonBone> skelBone, out NativeArray<SkeletonIKConstraint> ik);
            m_skelInstructionList.Add(instruction);
            m_skelBoneList.Add(skelBone);
            m_skelIkList.Add(ik);
            var job = new UpdateWorldTransformJob(instruction, skelBone, ik);
            job.Initialize();
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
            }
            m_skelInstructionList.Clear();
            m_skelBoneList.Clear();
            m_skelIkList.Clear();
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
                m_boneTracker = new BoneTracker();
                m_updateAfterWorldUpdateIndexList = new List<int>();
            }
        }

        private void Update()
        {
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
        }

        #region Jobs
        private struct SkeletonTime
        {
            public SkeletonTime(SkeletonAnimation animation)
            {
                timeScale = animation.timeScale;
                time = animation.skeleton.Time;
                deltaTime = 0;
            }

            public float timeScale;
            public float time;
            public float deltaTime;
        }

        private struct DeltaTimeUpdateJob : IJobParallelFor
        {
            private NativeArray<SkeletonTime> m_skeletonTime;
            private float deltaTime;

            public DeltaTimeUpdateJob(NativeArray<SkeletonTime> skeletonTime, float deltaTime)
            {
                m_skeletonTime = skeletonTime;
                this.deltaTime = deltaTime;
            }

            public void Execute(int index)
            {
                var skelTime = m_skeletonTime[index];
                skelTime.deltaTime = deltaTime * skelTime.timeScale;
                skelTime.time += skelTime.deltaTime;
                m_skeletonTime[index] = skelTime;
            }
        }


        public struct SkeletonBone
        {
            #region Variables
            public SkeletonBone(Bone bone)
            {
                if (bone != null)
                {
                    hashCode = bone.GetHashCode();
                    transformMode = bone.data.transformMode;

                    ax = bone.x;
                    ay = bone.y;
                    arotation = bone.rotation;
                    ascaleX = bone.scaleX;
                    ascaleY = bone.scaleY;
                    ashearX = bone.shearX;
                    ashearY = bone.shearY;
                    appliedValid = false;

                    skeletonScaleX = bone.skeleton.ScaleX;
                    skeletonScaleY = bone.skeleton.ScaleY;
                    skeletonX = bone.skeleton.x;
                    skeletonY = bone.skeleton.y;

                    hasParent = bone.parent != null;
                    parentHashCode = hasParent ? bone.parent.GetHashCode() : 0;
                    dataLength = bone.data.length;

                    x = bone.x;
                    y = bone.y;
                    rotation = bone.rotation;
                    scaleX = bone.scaleX;
                    scaleY = bone.scaleY;
                    shearX = bone.shearX;
                    shearY = bone.shearY;

                    a = 0;
                    b = 0;
                    c = 0;
                    d = 0;
                    worldX = 0;
                    worldY = 0;
                }
                else
                {
                    hashCode = 0;
                    transformMode = TransformMode.Normal;

                    ax = 0;
                    ay = 0;
                    arotation = 0;
                    ascaleX = 0;
                    ascaleY = 0;
                    ashearX = 0;
                    ashearY = 0;
                    appliedValid = false;

                    skeletonScaleX = 0;
                    skeletonScaleY = 0;
                    skeletonX = 0;
                    skeletonY = 0;

                    hasParent = false;
                    parentHashCode = 0;
                    dataLength = 0f;

                    x = 0;
                    y = 0;
                    rotation = 0;
                    scaleX = 0;
                    scaleY = 0;
                    shearX = 0;
                    shearY = 0;

                    a = 0;
                    b = 0;
                    c = 0;
                    d = 0;
                    worldX = 0;
                    worldY = 0;
                }
                parentIndex = 0;
            }

            public int hashCode { get; }
            public bool hasParent { get; }
            public int parentHashCode { get; }

            public int parentIndex;

            public float dataLength { get; }

            public float skeletonScaleX { get; }
            public float skeletonScaleY { get; }
            public float skeletonX { get; }
            public float skeletonY { get; }


            public float ax { get; private set; }
            public float ay { get; private set; }
            public float arotation { get; private set; }
            public float ascaleX { get; private set; }
            public float ascaleY { get; private set; }

            public float ashearX { get; private set; }
            public float ashearY { get; private set; }
            public bool appliedValid { get; private set; }

            public TransformMode transformMode { get; }

            public float x, y, rotation, scaleX, scaleY, shearX, shearY;
            public float a, b, c, d;
            public float worldX, worldY;
            #endregion

            public void UpdateWorldTransform(SkeletonBone parent)
            {
                UpdateWorldTransform(x, y, rotation, scaleX, scaleY, shearX, shearY, parent);
            }

            public void UpdateWorldTransform(float x, float y, float rotation, float scaleX, float scaleY, float shearX, float shearY, SkeletonBone parent)
            {
                ax = x;
                ay = y;
                arotation = rotation;
                ascaleX = scaleX;
                ascaleY = scaleY;
                ashearX = shearX;
                ashearY = shearY;
                appliedValid = true;

                if (hasParent)
                { // Root bone.

                    float pa = parent.a, pb = parent.b, pc = parent.c, pd = parent.d;
                    worldX = pa * x + pb * y + parent.worldX;
                    worldY = pc * x + pd * y + parent.worldY;

                    switch (transformMode)
                    {
                        case TransformMode.Normal:
                            {
                                float rotationY = rotation + 90 + shearY;
                                float la = MathUtils.CosDeg(rotation + shearX) * scaleX;
                                float lb = MathUtils.CosDeg(rotationY) * scaleY;
                                float lc = MathUtils.SinDeg(rotation + shearX) * scaleX;
                                float ld = MathUtils.SinDeg(rotationY) * scaleY;
                                a = pa * la + pb * lc;
                                b = pa * lb + pb * ld;
                                c = pc * la + pd * lc;
                                d = pc * lb + pd * ld;
                                return;
                            }
                        case TransformMode.OnlyTranslation:
                            {
                                float rotationY = rotation + 90 + shearY;
                                a = MathUtils.CosDeg(rotation + shearX) * scaleX;
                                b = MathUtils.CosDeg(rotationY) * scaleY;
                                c = MathUtils.SinDeg(rotation + shearX) * scaleX;
                                d = MathUtils.SinDeg(rotationY) * scaleY;
                                break;
                            }
                        case TransformMode.NoRotationOrReflection:
                            {
                                float s = pa * pa + pc * pc, prx;
                                if (s > 0.0001f)
                                {
                                    s = Math.Abs(pa * pd - pb * pc) / s;
                                    pb = pc * s;
                                    pd = pa * s;
                                    prx = MathUtils.Atan2(pc, pa) * MathUtils.RadDeg;
                                }
                                else
                                {
                                    pa = 0;
                                    pc = 0;
                                    prx = 90 - MathUtils.Atan2(pd, pb) * MathUtils.RadDeg;
                                }
                                float rx = rotation + shearX - prx;
                                float ry = rotation + shearY - prx + 90;
                                float la = MathUtils.CosDeg(rx) * scaleX;
                                float lb = MathUtils.CosDeg(ry) * scaleY;
                                float lc = MathUtils.SinDeg(rx) * scaleX;
                                float ld = MathUtils.SinDeg(ry) * scaleY;
                                a = pa * la - pb * lc;
                                b = pa * lb - pb * ld;
                                c = pc * la + pd * lc;
                                d = pc * lb + pd * ld;
                                break;
                            }
                        case TransformMode.NoScale:
                        case TransformMode.NoScaleOrReflection:
                            {
                                float cos = MathUtils.CosDeg(rotation), sin = MathUtils.SinDeg(rotation);
                                float za = (pa * cos + pb * sin) / skeletonScaleX;
                                float zc = (pc * cos + pd * sin) / skeletonScaleY;
                                float s = (float)Math.Sqrt(za * za + zc * zc);
                                if (s > 0.00001f) s = 1 / s;
                                za *= s;
                                zc *= s;
                                s = (float)Math.Sqrt(za * za + zc * zc);
                                if (transformMode == TransformMode.NoScale
                                    && (pa * pd - pb * pc < 0) != (skeletonScaleX < 0 != skeletonScaleY < 0)) s = -s;

                                float r = MathUtils.PI / 2 + MathUtils.Atan2(zc, za);
                                float zb = MathUtils.Cos(r) * s;
                                float zd = MathUtils.Sin(r) * s;
                                float la = MathUtils.CosDeg(shearX) * scaleX;
                                float lb = MathUtils.CosDeg(90 + shearY) * scaleY;
                                float lc = MathUtils.SinDeg(shearX) * scaleX;
                                float ld = MathUtils.SinDeg(90 + shearY) * scaleY;
                                a = za * la + zb * lc;
                                b = za * lb + zb * ld;
                                c = zc * la + zd * lc;
                                d = zc * lb + zd * ld;
                                break;
                            }
                    }

                    a *= skeletonScaleX;
                    b *= skeletonScaleX;
                    c *= skeletonScaleY;
                    d *= skeletonScaleY;
                }
                else
                {
                    float rotationY = rotation + 90 + shearY;
                    a = MathUtils.CosDeg(rotation + shearX) * scaleX * skeletonScaleX;
                    b = MathUtils.CosDeg(rotationY) * scaleY * skeletonScaleX;
                    c = MathUtils.SinDeg(rotation + shearX) * scaleX * skeletonScaleY;
                    d = MathUtils.SinDeg(rotationY) * scaleY * skeletonScaleY;
                    worldX = x * skeletonScaleX + skeletonX;
                    worldY = y * skeletonScaleY + skeletonY;
                }


            }

            public void UpdateAppliedTransform(SkeletonBone parent)
            {
                appliedValid = true;
                if (hasParent)
                {
                    float pa = parent.a, pb = parent.b, pc = parent.c, pd = parent.d;
                    float pid = 1 / (pa * pd - pb * pc);
                    float dx = worldX - parent.worldX, dy = worldY - parent.worldY;
                    ax = (dx * pd * pid - dy * pb * pid);
                    ay = (dy * pa * pid - dx * pc * pid);
                    float ia = pid * pd;
                    float id = pid * pa;
                    float ib = pid * pb;
                    float ic = pid * pc;
                    float ra = ia * a - ib * c;
                    float rb = ia * b - ib * d;
                    float rc = id * c - ic * a;
                    float rd = id * d - ic * b;
                    ashearX = 0;
                    ascaleX = (float)Math.Sqrt(ra * ra + rc * rc);
                    if (ascaleX > 0.0001f)
                    {
                        float det = ra * rd - rb * rc;
                        ascaleY = det / ascaleX;
                        ashearY = MathUtils.Atan2(ra * rb + rc * rd, det) * MathUtils.RadDeg;
                        arotation = MathUtils.Atan2(rc, ra) * MathUtils.RadDeg;
                    }
                    else
                    {
                        ascaleX = 0;
                        ascaleY = (float)Math.Sqrt(rb * rb + rd * rd);
                        ashearY = 0;
                        arotation = 90 - MathUtils.Atan2(rd, rb) * MathUtils.RadDeg;
                    }
                }
                else
                {
                    ax = worldX;
                    ay = worldY;
                    arotation = MathUtils.Atan2(c, a) * MathUtils.RadDeg;
                    ascaleX = (float)Math.Sqrt(a * a + c * c);
                    ascaleY = (float)Math.Sqrt(b * b + d * d);
                    ashearX = 0;
                    ashearY = MathUtils.Atan2(a * b + c * d, a * d - b * c) * MathUtils.RadDeg;
                }
            }
        }


        public struct SkeletonIKConstraint
        {
            public SkeletonIKConstraint(IkConstraint constraint)
            {
                targetHashCode = constraint.target.GetHashCode();
                boneCount = constraint.bones.Count;
                if (boneCount > 0)
                {
                    boneHashCode1 = constraint.bones.Items[0].GetHashCode();
                    if (boneCount == 2)
                    {
                        boneHashCode2 = constraint.bones.Items[1].GetHashCode();
                    }
                    else
                    {
                        boneHashCode2 = 0;
                    }
                }
                else
                {
                    boneHashCode1 = 0;
                    boneHashCode2 = 0;
                }

                compress = constraint.compress;
                stretch = constraint.stretch;
                uniform = constraint.data.uniform;
                alpha = constraint.mix;
                bendDir = constraint.bendDirection;
                softness = constraint.softness;

                targetIndex = 0;
                boneIndex1 = 0;
                boneIndex2 = 0;
            }

            public int targetHashCode { get; }
            public int targetIndex;

            public int boneCount { get; }
            public int boneHashCode1 { get; }
            public int boneHashCode2 { get; }

            public int boneIndex1;

            public int boneIndex2;

            public bool compress { get; }

            public bool stretch { get; }

            public bool uniform { get; }

            public float alpha { get; }

            public int bendDir { get; }

            public float softness { get; private set; }

            public void Apply(SkeletonBone targetBone, ref SkeletonBone childBone, SkeletonBone childBoneParent)
            {
                Apply(ref childBone, targetBone.worldX, targetBone.worldY, compress, stretch, uniform, alpha, childBoneParent);
            }

            public void Apply(ref SkeletonBone bone, float targetX, float targetY, bool compress, bool stretch, bool uniform,
                                float alpha, SkeletonBone parent)
            {
                if (!bone.appliedValid) bone.UpdateAppliedTransform(parent);

                float pa = parent.a, pb = parent.b, pc = parent.c, pd = parent.d;
                float rotationIK = -bone.ashearX - bone.arotation;
                float tx = 0, ty = 0;

                switch (bone.transformMode)
                {
                    case TransformMode.OnlyTranslation:
                        tx = targetX - bone.worldX;
                        ty = targetY - bone.worldY;
                        break;
                    case TransformMode.NoRotationOrReflection:
                        {
                            rotationIK += (float)Math.Atan2(pc, pa) * MathUtils.RadDeg;
                            float ps = Math.Abs(pa * pd - pb * pc) / (pa * pa + pc * pc);
                            pb = -pc * ps;
                            pd = pa * ps;
                            float x = targetX - parent.worldX, y = targetY - parent.worldY;
                            float d = pa * pd - pb * pc;
                            tx = (x * pd - y * pb) / d - bone.ax;
                            ty = (y * pa - x * pc) / d - bone.ay;
                            break;
                        }
                    default:
                        {
                            float x = targetX - parent.worldX, y = targetY - parent.worldY;
                            float d = pa * pd - pb * pc;
                            tx = (x * pd - y * pb) / d - bone.ax;
                            ty = (y * pa - x * pc) / d - bone.ay;
                            break;
                        }
                }

                rotationIK += (float)Math.Atan2(ty, tx) * MathUtils.RadDeg;
                if (bone.ascaleX < 0) rotationIK += 180;
                if (rotationIK > 180)
                    rotationIK -= 360;
                else if (rotationIK < -180) //
                    rotationIK += 360;

                float sx = bone.ascaleX, sy = bone.ascaleY;
                if (compress || stretch)
                {
                    switch (bone.transformMode)
                    {
                        case TransformMode.NoScale:
                            tx = targetX - bone.worldX;
                            ty = targetY - bone.worldY;
                            break;
                        case TransformMode.NoScaleOrReflection:
                            tx = targetX - bone.worldX;
                            ty = targetY - bone.worldY;
                            break;
                    }
                    float b = bone.dataLength * sx, dd = (float)Math.Sqrt(tx * tx + ty * ty);
                    if ((compress && dd < b) || (stretch && dd > b) && b > 0.0001f)
                    {
                        float s = (dd / b - 1) * alpha + 1;
                        sx *= s;
                        if (uniform) sy *= s;
                    }
                }
                bone.UpdateWorldTransform(bone.ax, bone.ay, bone.arotation + rotationIK * alpha, sx, sy, bone.ashearX, bone.ashearY, parent);
            }

            public void Apply(SkeletonBone targetBone, ref SkeletonBone parentBone, SkeletonBone parentBoneParent, ref SkeletonBone childBone, SkeletonBone childBoneParent)
            {
                if (alpha == 0)
                {
                    childBone.UpdateWorldTransform(childBoneParent);
                    return;
                }
                if (!parentBone.appliedValid) parentBone.UpdateAppliedTransform(parentBoneParent);
                if (!childBone.appliedValid) childBone.UpdateAppliedTransform(childBoneParent);
                float px = parentBone.ax, py = parentBone.ay, psx = parentBone.ascaleX, sx = psx, psy = parentBone.ascaleY, csx = childBone.ascaleX;
                int os1, os2, s2;
                if (psx < 0)
                {
                    psx = -psx;
                    os1 = 180;
                    s2 = -1;
                }
                else
                {
                    os1 = 0;
                    s2 = 1;
                }
                if (psy < 0)
                {
                    psy = -psy;
                    s2 = -s2;
                }
                if (csx < 0)
                {
                    csx = -csx;
                    os2 = 180;
                }
                else
                    os2 = 0;
                float cx = childBone.ax, cy, cwx, cwy, a = parentBone.a, b = parentBone.b, c = parentBone.c, d = parentBone.d;
                bool u = Math.Abs(psx - psy) <= 0.0001f;
                if (!u)
                {
                    cy = 0;
                    cwx = a * cx + parentBone.worldX;
                    cwy = c * cx + parentBone.worldY;
                }
                else
                {
                    cy = childBone.ay;
                    cwx = a * cx + b * cy + parentBone.worldX;
                    cwy = c * cx + d * cy + parentBone.worldY;
                }
                a = parentBoneParent.a;
                b = parentBoneParent.b;
                c = parentBoneParent.c;
                d = parentBoneParent.d;
                float id = 1 / (a * d - b * c), x = cwx - parentBoneParent.worldX, y = cwy - parentBoneParent.worldY;
                float dx = (x * d - y * b) * id - px, dy = (y * a - x * c) * id - py;
                float l1 = (float)Math.Sqrt(dx * dx + dy * dy), l2 = childBone.dataLength * csx, a1, a2;
                if (l1 < 0.0001f)
                {

                    Apply(ref parentBone, targetBone.worldX, targetBone.worldY, false, stretch, false, alpha, parentBoneParent);
                    childBone.UpdateWorldTransform(cx, cy, 0, childBone.ascaleX, childBone.ascaleY, childBone.ashearX, childBone.ashearY, childBoneParent);
                    return;
                }
                x = targetBone.worldX - parentBoneParent.worldX;
                y = targetBone.worldY - parentBoneParent.worldY;
                float tx = (x * d - y * b) * id - px, ty = (y * a - x * c) * id - py;
                float dd = tx * tx + ty * ty;
                if (softness != 0)
                {
                    softness *= psx * (csx + 1) / 2;
                    float td = (float)Math.Sqrt(dd), sd = td - l1 - l2 * psx + softness;
                    if (sd > 0)
                    {
                        float p = Math.Min(1, sd / (softness * 2)) - 1;
                        p = (sd - softness * (1 - p * p)) / td;
                        tx -= p * tx;
                        ty -= p * ty;
                        dd = tx * tx + ty * ty;
                    }
                }
                if (u)
                {
                    l2 *= psx;
                    float cos = (dd - l1 * l1 - l2 * l2) / (2 * l1 * l2);
                    if (cos < -1)
                        cos = -1;
                    else if (cos > 1)
                    {
                        cos = 1;
                        if (stretch) sx *= ((float)Math.Sqrt(dd) / (l1 + l2) - 1) * alpha + 1;
                    }
                    a2 = (float)Math.Acos(cos) * bendDir;
                    a = l1 + l2 * cos;
                    b = l2 * (float)Math.Sin(a2);
                    a1 = (float)Math.Atan2(ty * a - tx * b, tx * a + ty * b);
                }
                else
                {
                    a = psx * l2;
                    b = psy * l2;
                    float aa = a * a, bb = b * b, ta = (float)Math.Atan2(ty, tx);
                    c = bb * l1 * l1 + aa * dd - aa * bb;
                    float c1 = -2 * bb * l1, c2 = bb - aa;
                    d = c1 * c1 - 4 * c2 * c;
                    if (d >= 0)
                    {
                        float q = (float)Math.Sqrt(d);
                        if (c1 < 0) q = -q;
                        q = -(c1 + q) / 2;
                        float r0 = q / c2, r1 = c / q;
                        float r = Math.Abs(r0) < Math.Abs(r1) ? r0 : r1;
                        if (r * r <= dd)
                        {
                            y = (float)Math.Sqrt(dd - r * r) * bendDir;
                            a1 = ta - (float)Math.Atan2(y, r);
                            a2 = (float)Math.Atan2(y / psy, (r - l1) / psx);
                            goto break_outer; // break outer;
                        }
                    }
                    float minAngle = MathUtils.PI, minX = l1 - a, minDist = minX * minX, minY = 0;
                    float maxAngle = 0, maxX = l1 + a, maxDist = maxX * maxX, maxY = 0;
                    c = -a * l1 / (aa - bb);
                    if (c >= -1 && c <= 1)
                    {
                        c = (float)Math.Acos(c);
                        x = a * (float)Math.Cos(c) + l1;
                        y = b * (float)Math.Sin(c);
                        d = x * x + y * y;
                        if (d < minDist)
                        {
                            minAngle = c;
                            minDist = d;
                            minX = x;
                            minY = y;
                        }
                        if (d > maxDist)
                        {
                            maxAngle = c;
                            maxDist = d;
                            maxX = x;
                            maxY = y;
                        }
                    }
                    if (dd <= (minDist + maxDist) / 2)
                    {
                        a1 = ta - (float)Math.Atan2(minY * bendDir, minX);
                        a2 = minAngle * bendDir;
                    }
                    else
                    {
                        a1 = ta - (float)Math.Atan2(maxY * bendDir, maxX);
                        a2 = maxAngle * bendDir;
                    }
                }
            break_outer:
                float os = (float)Math.Atan2(cy, cx) * s2;
                float rotation = parentBone.arotation;
                a1 = (a1 - os) * MathUtils.RadDeg + os1 - rotation;
                if (a1 > 180)
                    a1 -= 360;
                else if (a1 < -180) a1 += 360;
                parentBone.UpdateWorldTransform(px, py, rotation + a1 * alpha, sx, parentBone.ascaleY, 0, 0, parentBoneParent);
                rotation = childBone.arotation;
                a2 = ((a2 + os) * MathUtils.RadDeg - childBone.ashearX) * s2 + os2 - rotation;
                if (a2 > 180)
                    a2 -= 360;
                else if (a2 < -180) a2 += 360;
                childBone.UpdateWorldTransform(cx, cy, rotation + a2 * alpha, childBone.ascaleX, childBone.ascaleY, childBone.ashearX, childBone.ashearY, childBoneParent);
            }
        }

        public struct SkeletonUpdateInstruction
        {

            public enum Type
            {
                Bone,
                IKConstraint,
            }
            public SkeletonUpdateInstruction(Type type, int index) : this()
            {
                this.type = type;
                this.index = index;
            }

            public Type type { get; }
            public int index { get; }
        }

        public struct UpdateWorldTransformJob : IJob
        {
            public NativeArray<SkeletonUpdateInstruction> instructionsArray;
            public NativeArray<SkeletonBone> bonesArray;
            public NativeArray<SkeletonIKConstraint> iKConstraintArray;

            public static void CreateNativeArrays(SkeletonAnimation animation, List<Bone> bones, out NativeArray<SkeletonUpdateInstruction> instructionsArray, out NativeArray<SkeletonBone> bonesArray, out NativeArray<SkeletonIKConstraint> iKConstraintArray)
            {
                var updatables = animation.skeleton.updateCache.Items;

                bones.Clear();
                Cache<List<SkeletonUpdateInstruction>> instructions = Cache<List<SkeletonUpdateInstruction>>.Claim();
                instructions.Value.Clear();
                Cache<List<SkeletonBone>> skelBones = Cache<List<SkeletonBone>>.Claim();
                skelBones.Value.Clear();
                Cache<List<SkeletonIKConstraint>> iKConstraints = Cache<List<SkeletonIKConstraint>>.Claim();
                iKConstraints.Value.Clear();

                for (int i = 0; i < updatables.Length; i++)
                {
                    var updatable = updatables[i];
                    if (updatable is Bone)
                    {
                        var bone = (Bone)updatable;
                        skelBones.Value.Add(new SkeletonBone(bone));
                        bones.Add(bone);
                        instructions.Value.Add(new SkeletonUpdateInstruction(SkeletonUpdateInstruction.Type.Bone, bones.Count - 1));
                    }
                    else if (updatable is IkConstraint)
                    {
                        var ikConstraint = (IkConstraint)updatable;
                        var boneCount = ikConstraint.bones.Count;
                        Bone bone;
                        for (int j = 0; j < boneCount; j++)
                        {
                            bone = ikConstraint.target;
                            if (bones.Contains(bone) == false)
                            {
                                skelBones.Value.Add(new SkeletonBone(bone));
                                bones.Add(bone);
                            }

                            bone = ikConstraint.bones.Items[j];
                            if (bones.Contains(bone) == false)
                            {
                                skelBones.Value.Add(new SkeletonBone(bone));
                                bones.Add(bone);
                            }
                        }
                        iKConstraints.Value.Add(new SkeletonIKConstraint(ikConstraint));
                        instructions.Value.Add(new SkeletonUpdateInstruction(SkeletonUpdateInstruction.Type.IKConstraint, iKConstraints.Value.Count - 1));
                    }
                    //LETS NOT DO THIS PLES
                    //else if (updatable is PathConstraint)
                    //{
                    //    PathConstraintCount++;
                    //}
                    //else if (updatable is TransformConstraint)
                    //{

                    //}
                }

                instructionsArray = new NativeArray<SkeletonUpdateInstruction>(instructions.Value.Count, Allocator.TempJob);
                for (int i = 0; i < instructionsArray.Length; i++)
                {
                    instructionsArray[i] = instructions.Value[i];
                }
                instructions.Release();
                bonesArray = new NativeArray<SkeletonBone>(skelBones.Value.Count, Allocator.TempJob);
                for (int i = 0; i < bonesArray.Length; i++)
                {
                    bonesArray[i] = skelBones.Value[i];
                }
                skelBones.Release();
                iKConstraintArray = new NativeArray<SkeletonIKConstraint>(iKConstraints.Value.Count, Allocator.TempJob);
                for (int i = 0; i < iKConstraintArray.Length; i++)
                {
                    iKConstraintArray[i] = iKConstraints.Value[i];
                }
                iKConstraints.Release();
            }

            public UpdateWorldTransformJob(NativeArray<SkeletonUpdateInstruction> instructionsArray, NativeArray<SkeletonBone> bonesArray, NativeArray<SkeletonIKConstraint> iKConstraintArray)
            {
                this.instructionsArray = instructionsArray;
                this.bonesArray = bonesArray;
                this.iKConstraintArray = iKConstraintArray;
            }

            public void Initialize()
            {
                for (int i = 0; i < bonesArray.Length; i++)
                {
                    if (bonesArray[i].hasParent)
                    {
                        var bone = bonesArray[i];
                        bone.parentIndex = GetBoneIndex(bone.parentHashCode);
                        bonesArray[i] = bone;
                    }
                }

                for (int i = 0; i < iKConstraintArray.Length; i++)
                {
                    var ik = iKConstraintArray[i];
                    if (ik.boneCount > 0)
                    {
                        ik.boneIndex1 = GetBoneIndex(ik.boneHashCode1);
                        if (ik.boneCount == 2)
                        {
                            ik.boneIndex2 = GetBoneIndex(ik.boneHashCode2);
                        }
                    }
                    ik.targetIndex = GetBoneIndex(ik.targetHashCode);
                    iKConstraintArray[i] = ik;
                }
            }

            public void Execute()
            {
                SkeletonBone nullBone = new SkeletonBone(null);
                SkeletonUpdateInstruction cacheInstruction;
                for (int i = 0; i < instructionsArray.Length; i++)
                {
                    cacheInstruction = instructionsArray[i];
                    switch (cacheInstruction.type)
                    {
                        case SkeletonUpdateInstruction.Type.Bone:
                            var bone = bonesArray[cacheInstruction.index];
                            bone.UpdateWorldTransform(GetParent(bone, nullBone));
                            bonesArray[cacheInstruction.index] = bone;
                            break;

                        case SkeletonUpdateInstruction.Type.IKConstraint:
                            var ik = iKConstraintArray[cacheInstruction.index];
                            switch (ik.boneCount)
                            {
                                case 1:
                                    var childBone1 = bonesArray[ik.boneIndex1];
                                    ik.Apply(bonesArray[ik.targetIndex], ref childBone1, GetParent(childBone1, nullBone));
                                    bonesArray[ik.boneIndex1] = childBone1;
                                    break;

                                case 2:
                                    var parentBone = bonesArray[ik.boneIndex1];
                                    var childBone2 = bonesArray[ik.boneIndex2];
                                    ik.Apply(bonesArray[ik.targetIndex], ref parentBone, GetParent(parentBone, nullBone), ref childBone2, GetParent(childBone2, nullBone));
                                    bonesArray[ik.boneIndex1] = childBone2;
                                    break;
                            }
                            iKConstraintArray[cacheInstruction.index] = ik;
                            break;
                    }
                }
            }

            public SkeletonBone GetSkeletonBone(int index) => bonesArray[index];

            private SkeletonBone GetParent(SkeletonBone bone, SkeletonBone nullBone) => bone.hasParent ? bonesArray[bone.parentIndex] : nullBone;
            private int GetBoneIndex(int hashCode)
            {
                for (int i = 0; i < bonesArray.Length; i++)
                {
                    if (hashCode == bonesArray[i].hashCode)
                    {
                        return i;
                    }
                }

                return -1;
            }
        }
    }
    #endregion

}