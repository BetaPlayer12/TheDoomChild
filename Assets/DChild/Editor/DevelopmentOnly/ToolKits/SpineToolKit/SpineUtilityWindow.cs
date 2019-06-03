using DChild.Gameplay.Combat;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Spine;
using Spine.Unity;
using UnityEditor;
using UnityEngine;
using DChild;
using System;

namespace DChildEditor.Toolkit
{
    public class SpineUtilityWindow : OdinEditorWindow
    {
        private static SpineUtilityWindow m_instance;

        private SkeletonUtility m_convertSkeleton;
        private SkeletonAnimation m_skeletonAnimation;
        private BoneFollower[] m_boneFollowers;
        private Hitbox m_hitbox;
        private bool m_lockSpineReference;
        private bool m_hasHitbox;

        private bool m_usingHitboxTransfer;
        private SkeletonAnimation m_sourceSkeleton;
        private Hitbox m_sourceHitbox;


        public void InitializeWindow()
        {
            titleContent = new GUIContent("Spine Utility Window", EditorIcons.Male.Active);
            m_lockSpineReference = false;
            m_hasHitbox = false;
        }

        private void DrawMain()
        {
            DrawConversion();
            EditorGUILayout.Space();
            DrawReferenceField();
            EditorGUILayout.Space();
            DrawSkeletonSetupFunctions();
            EditorGUILayout.Space();
            DrawHitboxTransfer();
        }

        private void DrawConversion()
        {
            EditorGUILayout.BeginHorizontal();
            m_convertSkeleton = (SkeletonUtility)EditorGUILayout.ObjectField("Convert Skeleton: ", m_convertSkeleton, typeof(SkeletonUtility), true);
            if(m_convertSkeleton!= null)
            {
                if(GUILayout.Button("Convert To Bone Follower"))
                {
                    SpineUtility.ConvertToBoneFollower(m_convertSkeleton);
                    UnityEngine.Object.DestroyImmediate(m_convertSkeleton);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawHitboxTransfer()
        {
            m_usingHitboxTransfer = SirenixEditorGUI.Foldout(true, "Hitbox Transfer");
            if (m_usingHitboxTransfer)
            {
                EditorGUI.indentLevel++;
                m_lockSpineReference = m_usingHitboxTransfer;
                EditorGUI.BeginChangeCheck();
                m_sourceSkeleton = (SkeletonAnimation)EditorGUILayout.ObjectField("Skeleton: ", m_sourceSkeleton, typeof(SkeletonAnimation), true);
                if (EditorGUI.EndChangeCheck())
                {
                    m_sourceHitbox = m_sourceSkeleton.GetComponentInChildren<Hitbox>();
                }

                if (m_sourceHitbox != null)
                {
                    if (m_sourceSkeleton == m_skeletonAnimation || (m_sourceSkeleton?.skeletonDataAsset ?? null) != m_skeletonAnimation.skeletonDataAsset)
                    {
                        m_sourceSkeleton = null;
                    }
                    else
                    {
                        if (GUILayout.Button("Transfer Hitbox"))
                        {
                            SpawnHitboxes();
                            var referenceBones = m_hitbox.GetComponentsInChildren<BoneFollower>();
                            SpineUtility.TransferHitbox(m_sourceHitbox.GetComponentsInChildren<BoxCollider2D>(), referenceBones);
                            SpineUtility.TransferHitbox(m_sourceHitbox.GetComponentsInChildren<PolygonCollider2D>(), referenceBones);
                        }
                    }
                }
                EditorGUI.indentLevel--;
            }
            else
            {
                m_sourceSkeleton = null;
            }
        }


        private void DrawSkeletonSetupFunctions()
        {
            if (m_skeletonAnimation != null && m_skeletonAnimation.skeletonDataAsset != null)
            {
                SirenixEditorGUI.BeginBox("Setup Function");
                if (GUILayout.Button("Create Hierarchy"))
                {
                    SpineUtility.CreateHierarchyFor(m_skeletonAnimation);
                }
                if (GUILayout.Button("Create Hitbox"))
                {
                    SpawnHitboxes();
                }

                if (m_hasHitbox)
                {
                    if (GUILayout.Button("Clean up Hitboxes"))
                    {
                        SpineUtility.CleanHitboxHierarchy(m_skeletonAnimation);
                    }
                }
                SirenixEditorGUI.EndBox();
            }
        }

        private void SpawnHitboxes()
        {
            var hierarchy = SpineUtility.CreateHitboxes(m_skeletonAnimation);
            m_hitbox = hierarchy.GetComponentInChildren<Hitbox>();
            m_hasHitbox = true;
        }

        private void DrawReferenceField()
        {
            EditorGUILayout.BeginHorizontal();
            m_skeletonAnimation = (SkeletonAnimation)EditorGUILayout.ObjectField("Skeleton: ", m_skeletonAnimation, typeof(SkeletonAnimation), true);
            if (m_lockSpineReference)
            {
                if (SirenixEditorGUI.IconButton(EditorIcons.LockLocked, tooltip: "Locks Reference"))
                {
                    m_lockSpineReference = false;
                }
            }
            else
            {
                m_skeletonAnimation = Selection.activeGameObject?.GetComponentInParent<SkeletonAnimation>() ?? null;
                if (m_skeletonAnimation)
                {
                    m_hitbox = m_skeletonAnimation.GetComponentInChildren<Hitbox>();
                    m_hasHitbox = m_hitbox;
                }
                if (SirenixEditorGUI.IconButton(EditorIcons.LockUnlocked, tooltip: "Gets Skeleton of Current Object or its parent"))
                {
                    m_lockSpineReference = true;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        protected override void OnGUI()
        {
            DrawMain();
        }

        protected override object GetTarget()
        {
            return m_instance;
        }
    }
}