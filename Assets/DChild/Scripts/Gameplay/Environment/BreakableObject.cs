
using DChild.Gameplay.Cinematics.Cameras;
using DChild.Gameplay.Combat;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DChild.Gameplay.Environment
{
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
        private bool m_forceShakeOnDestroy;
        [SerializeField, ShowIf("m_forceShakeOnDestroy"), Indent]
        private CameraShakeData m_onDestroyShake;

        [SerializeField]
        private bool m_createDebris;
        [SerializeField, ShowIf("m_createDebris"), Indent]
        private AssetReferenceGameObject m_debris;
        [SerializeField, ShowIf("m_createDebris"), Indent]
        private bool m_copySorting;
        [SerializeField, ShowIf("m_createDebris"), Indent]
        private bool m_applyDebrisColorChange;
        [SerializeField, ShowIf("m_applyDebrisColorChange"), Indent]
        private Color m_colorToApply = Color.white;

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
        private int m_sortingID;

        [SerializeField]
        private bool m_willRepairSelf;
        [SerializeField]
        private float m_selfRepairTime;
        private float m_selfRepairTimer;

        public Type type => m_type;

        public void SetObjectState(bool isDestroyed)
        {
            if (isDestroyed)
            {
                BreakObject();
                //m_object.Healed += ODamagaeableHeal;
            }
            else
            {
                RevertToFixState();
                //m_object.Healed -= ODamagaeableHeal;
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
                RevertToFixState();
            }
        }

        public void Initialize()
        {
            m_isDestroyed = false;
            RevertToFixState();
        }

        [Button, HideInEditorMode, HideIf("m_isDestroyed")]
        public void BreakObject()
        {
            m_isDestroyed = true;
            m_onDestroy?.Invoke();
            if (m_forceShakeOnDestroy)
            {
                GameplaySystem.cinema.ExecuteCameraShake(m_onDestroyShake);
            }
            if (m_createDebris)
            {
                InstantiateDebris(m_debris);
            }
        }


        [Button, HideInEditorMode, ShowIf("m_isDestroyed")]
        private void RevertToFixState()
        {
            m_isDestroyed = false;
            m_onFix?.Invoke();
            if (m_createDebris)
            {
                DestroyInstantiatedDebris();
            }
        }

        private void InstantiateDebris(GameObject debris)
        {
            var instance = Instantiate(debris, m_object.position, Quaternion.identity);
            InitializeDebris(instance);
        }

        private void InstantiateDebris(AssetReferenceGameObject debris)
        {
            Addressables.InstantiateAsync(debris).Completed += OnDebrisSpawn;
        }

        private void ODamagaeableHeal(object sender, EventActionArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void OnDebrisSpawn(AsyncOperationHandle<GameObject> obj)
        {
            InitializeDebris(obj.Result);
        }

        private void InitializeDebris(GameObject instance)
        {
            var instanceTransform = instance.transform;
            instanceTransform.parent = transform;
            instanceTransform.localPosition = Vector3.zero;
            instanceTransform.localScale = Vector3.one;
            instanceTransform.parent = null;
            m_instantiatedDebris = instance.GetComponent<Debris>();
            if (m_instantiatedDebris != null)
            {
                m_instantiatedDebris.SetInitialForceReference(m_forceDirection, m_force);
                m_leftOverDebris = m_instantiatedDebris.GetDetachables();
                if (m_applyDebrisColorChange)
                {
                    m_instantiatedDebris.GetComponent<RendererColorChangeHandle>()?.ApplyColor(m_colorToApply);
                }
            }
            else
            {
                //assuming the game object is particle effects rather than actual debris and effects shgould be going left to right
                if (m_forceDirection.x == -1)
                {

                    instanceTransform.localScale = new Vector3(m_forceDirection.x, instanceTransform.localScale.y, 1);

                }
                else
                {

                }
            }

            if (m_copySorting)
            {
                var renderers = instance.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].sortingLayerID = m_sortingID;
                }
            }
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


        private void Awake()
        {
            m_object.Destroyed += OnDestroyObject;

            if (TryGetComponent(out SortingHandle sortingHandle))
            {
                m_sortingID = sortingHandle.sortingLayerID;
            }
            else if (TryGetComponent(out SortingGroup sortingGroup))
            {
                m_sortingID = sortingGroup.sortingOrder;
            }
        }

        private void Update()
        {
            if (m_willRepairSelf)
            {
                if (m_isDestroyed)
                {
                    m_selfRepairTimer += GameplaySystem.time.deltaTime;
                    Debug.Log(m_selfRepairTimer);
                    if (m_selfRepairTimer > m_selfRepairTime)
                    {
                        m_selfRepairTimer = 0;
                        RevertToFixState();
                    }
                }
            }
        }


#if UNITY_EDITOR
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
