using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DChild.Gameplay.Optimization.Modules
{

    [System.Serializable]
    public abstract class FunctionCullingModule<U> where U : Component
    {
        [System.Serializable]
        public abstract class BaseInfo
        {
            [SerializeField, ReadOnly, TableColumnWidth(200, resizable: false)]
            protected U m_instance;
            [SerializeField, OnValueChanged("InitializeCuller",false), TableColumnWidth(350,resizable: false)]
            protected ICullingVisibilityChecker m_culler;
#if UNITY_EDITOR
            [SerializeField, PropertyOrder(-1), TableColumnWidth(10), LabelText("")]
            private bool m_gizmo;
            public U instance => m_instance;
            public bool showGizmo => m_gizmo && m_instance != null;
#endif
            protected abstract float m_defaulCullSize { get; }

            protected virtual Vector3 position => m_instance.transform.position;
            protected virtual Vector3 scale => m_instance.transform.lossyScale;

            protected BaseInfo()
            {
#if UNITY_EDITOR
                m_gizmo = true;
#endif
                m_instance = null;
            }

            protected abstract void SetFunctionality(bool enabled);

            public virtual void ExecuteOptimization(Bounds cameraBounds)
            {
                SetFunctionality(m_culler.IsVisible(position, m_instance.transform, cameraBounds));
            }

            public virtual void Initialize()
            {
                m_culler.InitializeRuntimeData(position, m_instance.transform);
            }

            public virtual void SetReference(U instance)
            {
                m_instance = instance;
            }

#if UNITY_EDITOR
            public virtual void DrawGizmos(Color gizmoColor)
            {
                if (showGizmo)
                {
                    m_culler.DrawGizmos(position, m_instance.transform, gizmoColor); 
                }
            }

            public virtual void DrawHandles(Color gizmoColor, UnityEngine.Object undoReference)
            {
                if (showGizmo)
                {
                    m_culler.DrawHandles(position, m_instance.transform, gizmoColor,undoReference);
                }
            }

            protected void InitializeCuller()
            {
                if (m_culler != null)
                {
                    m_culler.InitializeConfiguration(m_defaulCullSize);
                }
            }
#endif
        }
        public abstract void Validate();
#if UNITY_EDITOR
        public abstract void DrawGizmos();
        public abstract void DrawHandles(UnityEngine.Object undoReference);
#endif

    }

    [System.Serializable]
    public abstract class FunctionCullingModule<T, U> : FunctionCullingModule<U> where T : FunctionCullingModule<U>.BaseInfo, new() where U : Component
    {
        [SerializeField, HideReferenceObjectPicker, TableList(AlwaysExpanded = true, IsReadOnly = true), ListDrawerSettings(DraggableItems = false), TabGroup("Configuration")]
        protected T[] m_infos = new T[0];

#if UNITY_EDITOR
        public override void DrawGizmos()
        {
            for (int i = 0; i < m_infos.Length; i++)
            {
                m_infos[i].DrawGizmos(m_gizmoColor);
            }
        }

        public override void DrawHandles(UnityEngine.Object undoReference)
        {
            for (int i = 0; i < m_infos.Length; i++)
            {
                m_infos[i].DrawHandles(Color.white,undoReference);
            }
        }
#endif

        public virtual void Initalize()
        {
            for (int i = 0; i < m_infos.Length; i++)
            {
                m_infos[i].Initialize();
            }
        }

        public override void Validate()
        {
#if UNITY_EDITOR
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
#endif
        }

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(-1), TabGroup("Configuration")]
        protected Color m_gizmoColor = new Color(0, 1, 1, 0.05f);
        [SerializeField, HideReferenceObjectPicker, ListDrawerSettings(DraggableItems = false), ValueDropdown("GetInstancesToImport", IsUniqueList = true), TabGroup("Reference"), OnValueChanged("OnReferenceChange", true)]
        private List<U> m_referencer = new List<U>();

        private IEnumerable GetInstancesToImport()
        {
            Func<Transform, string> getpath = null;
            getpath = x => (x ? getpath(x.parent) + "/" + x.gameObject.name : "");
            return UnityEngine.Object.FindObjectsOfType<U>().Select(x => new ValueDropdownItem(getpath(x.transform), x));
        }

        private void OnReferenceChange()
        {
            List<T> list = new List<T>(m_infos);
            var newInfos = new T[m_referencer.Count];
            for (int i = 0; i < m_referencer.Count; i++)
            {
                var infoIndex = list.FindIndex(x => x.instance == m_referencer[i]);
                if (infoIndex >= 0)
                {
                    newInfos[i] = list[infoIndex];
                }
                else
                {
                    newInfos[i] = new T();
                    newInfos[i].SetReference(m_referencer[i]);
                }
            }
            m_infos = newInfos;
        }
#endif
    }
}