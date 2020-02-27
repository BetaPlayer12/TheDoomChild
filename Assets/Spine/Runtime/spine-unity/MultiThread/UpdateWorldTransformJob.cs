using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Spine;
using Sirenix.Utilities;

namespace DChild
{

    #region Jobs
    public struct UpdateWorldTransformJob : IJob
    {
        public NativeArray<SkeletonUpdateInstruction> instructionsArray;
        public NativeArray<SkeletonBone> bonesArray;
        public NativeArray<SkeletonIKConstraint> iKConstraintArray;
        public NativeArray<SkeletonTransformConstraint> transformConstraintArray;
        public NativeArray<SkeletonBoneIndex> transfromConstraintBonesArray;

        public UpdateWorldTransformJob(NativeArray<SkeletonUpdateInstruction> instructionsArray, NativeArray<SkeletonBone> bonesArray,
                                       NativeArray<SkeletonIKConstraint> iKConstraintArray, NativeArray<SkeletonTransformConstraint> transformConstraintArray,
                                       NativeArray<SkeletonBoneIndex> transfromConstraintBonesArray)
        {
            this.instructionsArray = instructionsArray;
            this.bonesArray = bonesArray;
            this.iKConstraintArray = iKConstraintArray;
            this.transformConstraintArray = transformConstraintArray;
            this.transfromConstraintBonesArray = transfromConstraintBonesArray;
        }

        public static void CreateNativeArrays(SkeletonAnimation animation, List<Bone> bones, out NativeArray<SkeletonUpdateInstruction> instructionsArray,
                                                out NativeArray<SkeletonBone> bonesArray, out NativeArray<SkeletonIKConstraint> iKConstraintArray,
                                                out NativeArray<SkeletonTransformConstraint> transformConstraintArray, out NativeArray<SkeletonBoneIndex> transfromConstraintBonesArray)
        {
            var updatables = animation.skeleton.updateCache.Items;

            bones.Clear();
            Cache<List<SkeletonUpdateInstruction>> instructions = Cache<List<SkeletonUpdateInstruction>>.Claim();
            instructions.Value.Clear();
            Cache<List<SkeletonBone>> skelBones = Cache<List<SkeletonBone>>.Claim();
            skelBones.Value.Clear();
            Cache<List<SkeletonIKConstraint>> iKConstraints = Cache<List<SkeletonIKConstraint>>.Claim();
            iKConstraints.Value.Clear();
            Cache<List<SkeletonTransformConstraint>> transformConstraints = Cache<List<SkeletonTransformConstraint>>.Claim();
            transformConstraints.Value.Clear();
            Cache<List<SkeletonBoneIndex>> transfromConstraintBones = Cache<List<SkeletonBoneIndex>>.Claim();
            transfromConstraintBones.Value.Clear();

            Bone bone;
            IkConstraint ikConstraint;
            TransformConstraint transformConstraint;
            Bone[] cacheTransfromContraintBones;
            for (int i = 0; i < updatables.Length; i++)
            {
                var updatable = updatables[i];
                if (updatable is Bone)
                {
                    bone = (Bone)updatable;
                    if (bones.Contains(bone))
                    {
                        var index = bones.FindIndex(x => x == bone);
                        instructions.Value.Add(new SkeletonUpdateInstruction(SkeletonUpdateInstruction.Type.Bone, index));
                    }
                    else
                    {
                        skelBones.Value.Add(new SkeletonBone(bone));
                        bones.Add(bone);
                        instructions.Value.Add(new SkeletonUpdateInstruction(SkeletonUpdateInstruction.Type.Bone, bones.Count - 1));
                    }

                    var parentBone = bone.parent;
                    if (parentBone != null)
                    {
                        if (bones.Contains(parentBone) == false)
                        {
                            skelBones.Value.Add(new SkeletonBone(parentBone));
                            bones.Add(parentBone);
                        }
                    }
                }
                else if (updatable is IkConstraint)
                {
                    ikConstraint = (IkConstraint)updatable;

                    bone = ikConstraint.target;
                    if (bones.Contains(bone) == false)
                    {
                        skelBones.Value.Add(new SkeletonBone(bone));
                        bones.Add(bone);
                    }

                    var boneCount = ikConstraint.bones.Count;
                    for (int j = 0; j < boneCount; j++)
                    {
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
                // LETS NOT DO THIS PLES
                else if (updatable is PathConstraint)
                {
                    Debug.Log("PathConstraint Supported");
                }
                else if (updatable is TransformConstraint)
                {
                    transformConstraint = (TransformConstraint)updatable;
                    var minBoneIndex = -1;
                    var validBoneCount = 0;
                    cacheTransfromContraintBones = transformConstraint.bones.Items;
                    if (cacheTransfromContraintBones.Length > 0)
                    {
                        minBoneIndex = transfromConstraintBones.Value.Count;
                        for (int j = 0; j < cacheTransfromContraintBones.Length; j++)
                        {
                            bone = cacheTransfromContraintBones[j];
                            if (bone != null)
                            {
                                if (bones.Contains(bone) == false)
                                {
                                    skelBones.Value.Add(new SkeletonBone(bone));
                                    bones.Add(bone);
                                }
                                transfromConstraintBones.Value.Add(new SkeletonBoneIndex(bone));
                                validBoneCount++;
                            }
                        }
                    }
                    var maxBoneIndex = minBoneIndex + validBoneCount;
                    transformConstraints.Value.Add(new SkeletonTransformConstraint(transformConstraint, minBoneIndex, maxBoneIndex));
                    instructions.Value.Add(new SkeletonUpdateInstruction(SkeletonUpdateInstruction.Type.TransformConstraint, transformConstraints.Value.Count - 1));
                }
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
            transformConstraintArray = new NativeArray<SkeletonTransformConstraint>(transformConstraints.Value.Count, Allocator.TempJob);
            for (int i = 0; i < transformConstraintArray.Length; i++)
            {
                transformConstraintArray[i] = transformConstraints.Value[i];
            }
            transformConstraints.Release();
            transfromConstraintBonesArray = new NativeArray<SkeletonBoneIndex>(transfromConstraintBones.Value.Count, Allocator.TempJob);
            for (int i = 0; i < transfromConstraintBonesArray.Length; i++)
            {
                transfromConstraintBonesArray[i] = transfromConstraintBones.Value[i];
            }
            transfromConstraintBones.Release();

            //Put SkeletonBone into a dictionary to easily get the index of each SkeletonBoneIndex
            Cache<Dictionary<int, int>> pairs = Cache<Dictionary<int, int>>.Claim();
            pairs.Value.Clear();
            for (int i = 0; i < bonesArray.Length; i++)
            {
                if (pairs.Value.ContainsKey(bonesArray[i].hashCode) == false)
                {
                    pairs.Value.Add(bonesArray[i].hashCode, i);
                }
            }

            for (int i = 0; i < bonesArray.Length; i++)
            {
                if (bonesArray[i].hasParent)
                {
                    var skelBone = bonesArray[i];
                    skelBone.parent.index = pairs.Value[skelBone.parent.hashcode];
                    bonesArray[i] = skelBone;
                }
            }

            for (int i = 0; i < iKConstraintArray.Length; i++)
            {
                var ik = iKConstraintArray[i];
                if (ik.boneCount > 0)
                {
                    ik.bone1.index = pairs.Value[ik.bone1.hashcode];
                    if (ik.boneCount == 2)
                    {
                        ik.bone2.index = pairs.Value[ik.bone2.hashcode];
                    }
                }
                ik.target.index = pairs.Value[ik.target.hashcode];
                iKConstraintArray[i] = ik;
            }

            for (int i = 0; i < transformConstraintArray.Length; i++)
            {
                var transform = transformConstraintArray[i];
                transform.target.index = pairs.Value[transform.target.hashcode];
                transformConstraintArray[i] = transform;
            }

            for (int i = 0; i < transfromConstraintBonesArray.Length; i++)
            {
                var transformBone = transfromConstraintBonesArray[i];
                transformBone.index = pairs.Value[transformBone.hashcode];
                transfromConstraintBonesArray[i] = transformBone;
            }


            pairs.Release();
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
                                var bone1Index = ik.bone1.index;
                                var childBone1 = bonesArray[bone1Index];
                                ik.Apply(bonesArray[ik.target.index], ref childBone1, GetParent(childBone1, nullBone));
                                bonesArray[bone1Index] = childBone1;
                                break;

                            case 2:
                                var parentBoneIndex = ik.bone1.index;
                                var parentBone = bonesArray[parentBoneIndex];
                                var childBoneIndex = ik.bone2.index;
                                var childBone2 = bonesArray[childBoneIndex];
                                ik.Apply(bonesArray[ik.target.index], ref parentBone, GetParent(parentBone, nullBone), ref childBone2, GetParent(childBone2, nullBone));
                                bonesArray[parentBoneIndex] = parentBone;
                                bonesArray[childBoneIndex] = childBone2;
                                break;
                        }
                        iKConstraintArray[cacheInstruction.index] = ik;
                        break;

                    case SkeletonUpdateInstruction.Type.TransformConstraint:
                        var transform = transformConstraintArray[cacheInstruction.index];
                        var targetBone = bonesArray[transform.target.index];
                        if (transform.dataLocal)
                        {
                            var data = transform.CreateLocalData();
                            if (transform.dataRelative)
                            {
                                for (int j = transform.minBoneIndex; j < transform.maxBoneIndex; j++)
                                {
                                    var transformBone = bonesArray[transfromConstraintBonesArray[j].index];
                                    transform.ApplyRelativeLocal(data, targetBone, ref transformBone, GetParent(transformBone, nullBone));
                                    bonesArray[transfromConstraintBonesArray[j].index] = transformBone;
                                }
                            }
                            else
                            {
                                for (int j = transform.minBoneIndex; j < transform.maxBoneIndex; j++)
                                {
                                    var transformBone = bonesArray[transfromConstraintBonesArray[j].index];
                                    transform.ApplyAbsoluteLocal(data, targetBone, ref transformBone, GetParent(transformBone, nullBone));
                                    bonesArray[transfromConstraintBonesArray[j].index] = transformBone;
                                }
                            }
                        }
                        else
                        {
                            var data = transform.CreateWorldData(targetBone);
                            if (transform.dataRelative)
                            {
                                for (int j = transform.minBoneIndex; j < transform.maxBoneIndex; j++)
                                {
                                    var transformBone = bonesArray[transfromConstraintBonesArray[j].index];
                                    transform.ApplyRelativeWorld(data, targetBone, ref transformBone);
                                    bonesArray[transfromConstraintBonesArray[j].index] = transformBone;
                                }
                            }
                            else
                            {
                                for (int j = transform.minBoneIndex; j < transform.maxBoneIndex; j++)
                                {
                                    var transformBone = bonesArray[transfromConstraintBonesArray[j].index];
                                    transform.ApplyAbsoluteWorld(data, targetBone, ref transformBone);
                                    bonesArray[transfromConstraintBonesArray[j].index] = transformBone;
                                }
                            }
                        }
                        break;
                }
            }
        }

        public SkeletonBone GetSkeletonBone(int index) => bonesArray[index];

        private SkeletonBone GetParent(SkeletonBone bone, SkeletonBone nullBone) => bone.hasParent ? bonesArray[bone.parent.index] : nullBone;
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
    #endregion

}