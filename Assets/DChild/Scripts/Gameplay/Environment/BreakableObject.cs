
using DChild.Gameplay.Combat;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{

    [RequireComponent(typeof(SortingHandle))]
    [AddComponentMenu("DChild/Gameplay/Environment/Breakable Object")]
    public class BreakableObject : MonoBehaviour, ISerializableComponent
    {
        public enum Type
        {
            Others,
            Floor,
            Wall
        }

        [System.Serializable]
        public struct SaveData : ISaveData
        {
            public SaveData(bool isDestroyed) : this()
            {
                this.m_isDestroyed = isDestroyed;
            }

            [SerializeField]
            private bool m_isDestroyed;

            public bool isDestroyed => m_isDestroyed;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_isDestroyed);
        }

        [SerializeField]
        private Damageable m_object;
        [SerializeField]
        private Type m_type;
        [ShowInInspector, OnValueChanged("SetObjectStateDebug")]
        private bool m_isDestroyed;
        [SerializeField]
        private bool m_createDebris;
        [SerializeField, ShowIf("m_createDebris"), Indent]
        private GameObject m_debris;
        [SerializeField, ShowIf("m_createDebris"), Indent]
        private bool m_copySorting;

        [SerializeField, TabGroup("On Destroy")]
        private UnityEvent m_onDestroy;
        [SerializeField, TabGroup("On Already Destroyed")]
        private UnityEvent m_onAlreadyDestroyed;
        [SerializeField, TabGroup("On Fix")]
        private UnityEvent m_onFix;

        private Vector2 m_forceDirection;
        private float m_force;
        private Debris m_instantiatedDebris;
        private Rigidbody2D[] m_leftOverDebris;
        private SortingHandle m_sortingHandle;

        public Type type => m_type;

        public void SetObjectState(bool isDestroyed)
        {
            m_isDestroyed = isDestroyed;
            if (m_isDestroyed == true)
            {
                m_onDestroy?.Invoke();
                if (m_createDebris)
                {
                    InstantiateDebris(m_debris);
                }
            }
            else
            {
                m_onFix?.Invoke();
                if (m_createDebris)
                {
                    DestroyInstantiatedDebris();
                }
            }
        }

        public void RecordForceReceived(Vector2 forceDirection, float force)
        {
            m_forceDirection = forceDirection;
            m_force = force;
        }

        public ISaveData Save() => new SaveData(m_isDestroyed);

        public void Load(ISaveData data)
        {
            var saveData = (SaveData)data;
            m_isDestroyed = saveData.isDestroyed;
            if (m_isDestroyed)
            {
                m_onAlreadyDestroyed?.Invoke();
            }
            else
            {
                m_onFix?.Invoke();
                if (m_createDebris)
                {
                    DestroyInstantiatedDebris();
                }
            }
        }

        private void InstantiateDebris(GameObject debris)
        {
            var instance = Instantiate(debris, m_object.position, Quaternion.identity);
            m_instantiatedDebris = instance.GetComponent<Debris>();
            m_instantiatedDebris.transform.parent = transform;
            m_instantiatedDebris.transform.localScale = transform.localScale;
            m_instantiatedDebris.transform.parent = null;
            m_instantiatedDebris.SetInitialForceReference(m_forceDirection, m_force);
            m_leftOverDebris = m_instantiatedDebris.GetDetachables();
            if (m_copySorting)
            {
                var renderers = instance.GetComponentsInChildren<SpriteRenderer>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].sortingLayerID = m_sortingHandle.sortingLayerID;
                }
            }
        }

        private void InstantiateDebris(AssetReferenceGameObject debris) => AddressableSpawner.Spawn(debris, m_object.position, 0, OnSpawn);

        private void OnSpawn(GameObject instance, int arg2)
        {
            m_instantiatedDebris = instance.GetComponent<Debris>();
            m_instantiatedDebris.transform.localScale = transform.localScale;
            m_instantiatedDebris.SetInitialForceReference(m_forceDirection, m_force);
            m_leftOverDebris = m_instantiatedDebris.GetDetachables();
        }

        private void DestroyInstantiatedDebris()
        {
            if (m_leftOverDebris != null)
            {
                for (int i = m_leftOverDebris.Length - 1; i >= 0; i--)
                {
                    Addressables.ReleaseInstance(m_leftOverDebris[i].gameObject);
                }
                m_leftOverDebris = null;
            }
            if (m_instantiatedDebris != null)
            {
                Addressables.ReleaseInstance(m_instantiatedDebris.gameObject);
            }
        }

        private void OnDestroyObject(object sender, EventActionArgs eventArgs)
        {
            SetObjectState(true);
        }

        // Start is called before the first frame update
        private void Awake()
        {
            m_object.Destroyed += OnDestroyObject;
            m_sortingHandle = GetComponent<SortingHandle>();
            //if (m_isDestroyed == true)
            //{
            //    m_onDestroy?.Invoke();
            //}
            //else
            //{
            //    m_onFix?.Invoke();
            //    if (m_createDebris)
            //    {
            //        DestroyInstantiatedDebris();
            //    }
            //}
        }

#if UNITY_EDITOR
        [Button, HideInEditorMode, HideIf("m_isDestroyed")]
        private void BreakObject()
        {
            m_isDestroyed = true;
            m_onDestroy?.Invoke();
            if (m_createDebris)
            {
                InstantiateDebris(m_debris);
            }
        }

        [Button, HideInEditorMode, ShowIf("m_isDestroyed")]
        private void FixObject()
        {
            m_isDestroyed = false;
            m_onFix?.Invoke();
            if (m_createDebris)
            {
                DestroyInstantiatedDebris();
            }
        }

        public void SetObjectStateDebug(bool isDestroyed)
        {
            m_isDestroyed = isDestroyed;
            if (m_isDestroyed == true)
            {
                m_onAlreadyDestroyed?.Invoke();
            }
            else
            {
                m_onFix?.Invoke();
                if (m_createDebris)
                {
                    DestroyInstantiatedDebris();
                }
            }
        }
#endif
    }
}
