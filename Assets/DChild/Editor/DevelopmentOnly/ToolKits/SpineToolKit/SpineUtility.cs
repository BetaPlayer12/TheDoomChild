using DChild;
using DChild.Gameplay.Combat;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace DChildEditor.Toolkit
{
    public static class SpineUtility
    {
        public static void ConvertToBoneFollower(SkeletonUtility skeletonUtility)
        {
            var skeletonUtilityBones = skeletonUtility.GetComponentsInChildren<SkeletonUtilityBone>();
            for (int i = skeletonUtilityBones.Length - 1; i >= 0; i--)
            {
                var utilityBone = skeletonUtilityBones[i];
                var boneFollower = utilityBone.gameObject.AddComponent<BoneFollower>();
                boneFollower.bone = utilityBone.bone;
                boneFollower.boneName = utilityBone.boneName;
                Object.DestroyImmediate(utilityBone);
            }
        }

        public static void TransferHitbox<T>(T[] toTransferColliders, BoneFollower[] boneReferences) where T : Collider2D
        {
            if (toTransferColliders == null || toTransferColliders.Length == 0)
                return;

            if (toTransferColliders[0].GetComponentInParent<SkeletonUtility>())
            {
                for (int i = 0; i < toTransferColliders.Length; i++)
                {
                    var currentBone = toTransferColliders[i].GetComponent<SkeletonUtilityBone>();
                    if (currentBone != null)
                    {
                        for (int j = 0; j < boneReferences.Length; j++)
                        {
                            if (boneReferences[j].boneName == currentBone.boneName)
                            {
                                boneReferences[j].gameObject.CopyComponentAsNew(toTransferColliders[i]);
                                boneReferences[j].GetComponent<T>().usedByComposite = true;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < toTransferColliders.Length; i++)
                {
                    var currentBone = toTransferColliders[i].GetComponent<BoneFollower>();
                    if (currentBone != null)
                    {
                        for (int j = 0; j < boneReferences.Length; j++)
                        {
                            if (boneReferences[j].boneName == currentBone.boneName)
                            {
                                boneReferences[j].gameObject.CopyComponentAsNew(toTransferColliders[i]);
                                boneReferences[j].GetComponent<T>().usedByComposite = true;
                            }
                        }
                    }
                }
            }

        }

        public static void CleanHitboxHierarchy(SkeletonRenderer skeletonRenderer)
        {
            var hitbox = skeletonRenderer.GetComponentInChildren<Hitbox>();
            if (hitbox != null)
            {
                var colliders = hitbox.GetComponentsInChildren<Collider2D>();
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != hitbox.gameObject)
                    {
                        colliders[i].usedByComposite = true;
                        colliders[i].transform.parent = hitbox.transform;
                    }
                }

                var rootTransform = hitbox.transform.Find("root");

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != hitbox.gameObject)
                    {
                        var transform = colliders[i].transform;
                        for (int j = transform.childCount - 1; j >= 0; j--)
                        {
                            transform.GetChild(j).parent = rootTransform;
                        }
                    }
                }
                Object.DestroyImmediate(rootTransform.gameObject);
            }
        }

        public static GameObject CreateHitboxes(SkeletonRenderer skeletonRenderer)
        {
            var hierarchy = SpawnHierarchy(skeletonRenderer, SkeletonUtilityBone.Mode.Follow, true, true, true);
            hierarchy.name = "Hitboxes";
            var rigidbody = hierarchy.AddComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            var compositeCollider = hierarchy.AddComponent<CompositeCollider2D>();
            compositeCollider.isTrigger = true;
            hierarchy.AddComponent<Hitbox>();
            hierarchy.AddComponent<BaseColliderDamage>();
            return hierarchy;
        }

        #region Spawn Hierarchy
        public static GameObject CreateHierarchyFor(SkeletonRenderer skeletonRenderer)
        {
            return SpawnHierarchy(skeletonRenderer, SkeletonUtilityBone.Mode.Follow, true, true, true);
        }

        private static GameObject SpawnHierarchy(SkeletonRenderer skeletonRenderer, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
        {
            var root = Commands.CreateGameObject("SkeletonUtility-Root", skeletonRenderer.transform, Vector3.zero);
            Skeleton skeleton = skeletonRenderer.skeleton;
            GameObject go = SpawnBoneRecursively(skeleton.RootBone, root.transform, mode, pos, rot, sca);
            return root;
        }

        private static GameObject SpawnBoneRecursively(Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
        {
            GameObject go = SpawnBone(bone, parent, mode, pos, rot, sca);

            ExposedList<Bone> childrenBones = bone.Children;
            for (int i = 0, n = childrenBones.Count; i < n; i++)
            {
                Bone child = childrenBones.Items[i];
                SpawnBoneRecursively(child, go.transform, mode, pos, rot, sca);
            }

            return go;
        }

        private static GameObject SpawnBone(Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
        {
            GameObject go = new GameObject(bone.Data.Name);
            go.transform.parent = parent;

            BoneFollower b = go.AddComponent<BoneFollower>();
            b.bone = bone;
            b.boneName = bone.Data.Name;
            b.followLocalScale = true;
            b.Initialize();


            if (mode == SkeletonUtilityBone.Mode.Override)
            {
                if (rot)
                    go.transform.localRotation = Quaternion.Euler(0, 0, b.bone.AppliedRotation);

                if (pos)
                    go.transform.localPosition = new Vector3(b.bone.X, b.bone.Y, 0);

                go.transform.localScale = new Vector3(b.bone.ScaleX, b.bone.ScaleY, 0);
            }

            return go;
        }
        #endregion
    }
}