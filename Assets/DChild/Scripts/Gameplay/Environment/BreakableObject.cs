using DChild.Gameplay.Combat;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{

    [AddComponentMenu("DChild/Gameplay/Environment/Breakable Object")]
    public class BreakableObject : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            public SaveData(bool isDestroyed) : this()
            {
                this.isDestroyed = isDestroyed;
            }

            [ShowInInspector,]
            public bool isDestroyed { get; private set; }
        }

        [SerializeField]
        private Damageable m_object;
        [ShowInInspector, OnValueChanged("SetObjectStateDebug")]
        private bool m_isDestroyed;
        [SerializeField]
        private bool m_createDebris;
        [SerializeField, ShowIf("m_createDebris"), Indent]
        private GameObject m_debris;

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
                    Destroy(m_leftOverDebris[i].gameObject);
                }
                m_leftOverDebris = null;
                Destroy(m_instantiatedDebris.gameObject);
            }
        }

        private void OnDestroyObject(object sender, EventActionArgs eventArgs)
        {
            m_onDestroy?.Invoke();
            if (m_createDebris)
            {
                InstantiateDebris(m_debris);
            }
        }

        // Start is called before the first frame update
        private void Awake()
        {
            m_object.Destroyed += OnDestroyObject;
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
            m_isDestroyed = true;
            m_onDestroy?.Invoke();
            if (m_createDebris)
            {
                InstantiateDebris(m_debris);
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
