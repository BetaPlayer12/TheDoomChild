using DChild.Gameplay.Characters.AI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.Optimization.Modules
{
    [System.Serializable, InfoBox("Minions")]
    public sealed class MinionFunctionCullingModule : IFunctionCullingModule2D
    {
        [System.Serializable]
        private class SubModule
        {
            [System.Serializable]
            private class Info
            {
                [SerializeField, ReadOnly, TableColumnWidth(500, resizable: false)]
                protected ICombatAIBrain m_instance;
                [SerializeField, HideInInspector]
                private Transform m_centerMass;
                [SerializeField, HideInInspector]
                private SkeletonAnimation m_animation;
                private BoneFollower[] m_boneFollowers;
                [SerializeField, HideInInspector]
                protected ICullingVisibilityChecker m_culler;

                private Vector3 m_lossyScale;
                private Vector3 m_rotation;

                public void ExecuteOptimization(Bounds cameraBounds)
                {
                    var transformChanges = false;
                    var lossyScale = m_centerMass.lossyScale;
                    var rotation = m_centerMass.rotation.eulerAngles;
                    if (m_lossyScale != lossyScale)
                    {
                        m_lossyScale = lossyScale;
                        transformChanges = true;
                    }
                    else if (m_rotation != rotation)
                    {
                        m_rotation = rotation;
                        transformChanges = true;
                    }

                    if (transformChanges)
                    {
                        m_culler.UpdateRuntimeData(m_centerMass.position, m_centerMass);
                    }
                    var result = m_culler.IsVisible(m_centerMass.position, m_centerMass, cameraBounds);
                    m_animation.enabled = result;
                    for (int i = 0; i < m_boneFollowers.Length; i++)
                    {
                        m_boneFollowers[i].enabled = result;
                    }
                }

                public void Initialize()
                {
                    m_lossyScale = m_centerMass.lossyScale;
                    m_rotation = m_centerMass.rotation.eulerAngles;
                    m_culler.InitializeRuntimeData(m_centerMass.position, m_centerMass);
                    m_boneFollowers = m_animation.GetComponentsInChildren<BoneFollower>(true);
                }

                #region EDITOR ONLY
#if UNITY_EDITOR
                [SerializeField, PropertyOrder(-1), TableColumnWidth(10), LabelText("")]
                private bool m_gizmo = true;
                public ICombatAIBrain instance => m_instance;
                public bool showGizmo => m_gizmo && m_instance != null;

                public void CopyCuller(ICullingVisibilityChecker culling)
                {
                    if (culling == null)
                    {
                        m_culler = null;
                    }
                    else
                    {
                        m_culler = culling.CreateCopy();
                    }
                }

                public void SetReference(ICombatAIBrain instance)
                {
                    m_instance = instance;
                    var monoBehaviour = ((MonoBehaviour)instance);
                    m_centerMass = monoBehaviour.GetComponentsInParent<Character>(true)[0].centerMass;
                    m_animation = monoBehaviour.GetComponentsInChildren<SkeletonAnimation>(true)[0];
                }

                public void DrawGizmo(Color gizmoColor)
                {
                    if (showGizmo)
                    {
                        m_culler.DrawGizmos(m_centerMass.position, m_centerMass, gizmoColor);
                    }
                }
#endif 
                #endregion
            }

            [SerializeField, ValueDropdown("GetMinionType"), OnValueChanged("OnMinionTypeChange")]
            private Type m_minionType;
#if UNITY_EDITOR
            [SerializeField]
            private bool m_showGizmo = true;
#endif
            [SerializeField]
            private Color m_gizmoColor = new Color(1, 1, 0, 0.2f);
            [SerializeField, TabGroup("Configuration"), OnValueChanged("OnCullerChange", true)]
            private ICullingVisibilityChecker m_culler;
            [SerializeField, HideReferenceObjectPicker, TableList(AlwaysExpanded = true, IsReadOnly = true), ListDrawerSettings(DraggableItems = false), TabGroup("Configuration")]
            private Info[] m_infos = new Info[0];
            [SerializeField, ValueDropdown("GetInstances", IsUniqueList = true), TabGroup("Reference"), OnValueChanged("OnReferenceChange")]
            private List<ICombatAIBrain> m_referencer = new List<ICombatAIBrain>();

            public void ExecuteOptimization(Bounds cameraBounds)
            {
                for (int i = 0; i < m_infos.Length; i++)
                {
                    m_infos[i].ExecuteOptimization(cameraBounds);
                }
            }

            public void Initialize()
            {
                for (int i = 0; i < m_infos.Length; i++)
                {
                    m_infos[i].Initialize();
                }
            }

            #region EDITOR ONLY
#if UNITY_EDITOR
            public void Validate()
            {
                bool hasNull = false;
                for (int i = 0; i < m_referencer.Count; i++)
                {
                    if (m_referencer[i] == null)
                    {
                        hasNull = true;
                        break;
                    }
                }

                if (hasNull)
                {
                    m_referencer.RemoveAll(x => x == null);
                    OnReferenceChange();
                }
            }

            private IEnumerable GetMinionType()
            {
                var type = typeof(ICombatAIBrain);
                var list = type.Assembly.GetTypes().Where(x => !x.IsAbstract).Where(x => !x.IsGenericTypeDefinition).Where(x => type.IsAssignableFrom(x));
                return list;
            }

            private void OnMinionTypeChange()
            {
                m_referencer.Clear();
            }

            private void OnCullerChange()
            {
                for (int i = 0; i < m_infos.Length; i++)
                {
                    m_infos[i].CopyCuller(m_culler);
                }
            }

            private IEnumerable GetInstances()
            {
                if (m_minionType == null)
                {
                    return null;
                }
                else
                {
                    Func<Transform, string> getpath = null;
                    getpath = x => (x ? getpath(x.parent) + "/" + x.gameObject.name : "");
                    var allMinions = UnityEngine.Object.FindObjectsOfTypeAll(m_minionType).Where(x => ((MonoBehaviour)x).gameObject.scene.name != null);
                    return allMinions.Select(x => new ValueDropdownItem(getpath(((MonoBehaviour)x).transform), (ICombatAIBrain)x));
                }
            }

            private void OnReferenceChange()
            {
                List<Info> list = new List<Info>(m_infos);
                var newInfos = new Info[m_referencer.Count];
                for (int i = 0; i < m_referencer.Count; i++)
                {
                    var infoIndex = list.FindIndex(x => x.instance == m_referencer[i]);
                    if (infoIndex >= 0)
                    {
                        newInfos[i] = list[infoIndex];
                    }
                    else
                    {
                        newInfos[i] = new Info();
                        newInfos[i].SetReference(m_referencer[i]);
                        if (m_culler != null)
                        {
                            newInfos[i].CopyCuller(m_culler);
                        }
                    }
                }
                m_infos = newInfos;
            }

            public void DrawGizmos()
            {
                if (m_showGizmo)
                {
                    for (int i = 0; i < m_infos.Length; i++)
                    {
                        m_infos[i].DrawGizmo(m_gizmoColor);
                    }
                }
            }
#endif 
            #endregion
        }

        [OdinSerialize, ListDrawerSettings(DraggableItems = false, NumberOfItemsPerPage = 1), HideReferenceObjectPicker]
        private SubModule[] m_submodules = new SubModule[0];

        public void ExecuteOptimization2D(Vector3 reference, Bounds boundExtent)
        {
            for (int i = 0; i < m_submodules.Length; i++)
            {
                m_submodules[i].ExecuteOptimization(boundExtent);
            }
        }

        public void Initalize()
        {
            for (int i = 0; i < m_submodules.Length; i++)
            {
                m_submodules[i].Initialize();
            }
        }

        public void Validate()
        {
#if UNITY_EDITOR
            for (int i = 0; i < m_submodules.Length; i++)
            {
                m_submodules[i].Validate();
            }
#endif
        }

#if UNITY_EDITOR
        public void DrawGizmos()
        {
            for (int i = 0; i < m_submodules.Length; i++)
            {
                m_submodules[i].DrawGizmos();
            }
        }

        public void DrawHandles(UnityEngine.Object undoReference)
        {

        }
#endif
    }
}