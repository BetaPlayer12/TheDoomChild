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

                Bone bone;
                IkConstraint ikConstraint;
                for (int i = 0; i < updatables.Length; i++)
                {
                    var updatable = updatables[i];
                    if (updatable is Bone)
                    {
                        bone = (Bone)updatable;
                        skelBones.Value.Add(new SkeletonBone(bone));
                        bones.Add(bone);
                        instructions.Value.Add(new SkeletonUpdateInstruction(SkeletonUpdateInstruction.Type.Bone, bones.Count - 1));
                    }
                    else if (updatable is IkConstraint)
                    {
                        ikConstraint = (IkConstraint)updatable;
                        var boneCount = ikConstraint.bones.Count;
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
                    // LETS NOT DO THIS PLES
                    else if (updatable is PathConstraint)
                    {
                        Debug.Log("A Supported");
                    }
                    else if (updatable is TransformConstraint)
                    {
                        Debug.Log("B Supported");
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
        #endregion

}