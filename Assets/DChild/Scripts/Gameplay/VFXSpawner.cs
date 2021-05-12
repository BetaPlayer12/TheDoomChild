using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DChild.Gameplay
{

    public class VFXSpawner : MonoBehaviour
    {
        [SerializeField]
        private AssetReferenceFX m_fx;
        private bool m_usePooling;
        private FXSpawnHandle<FX> m_fxHandle;

        public void Set(AssetReferenceFX fx)
        {
            m_fx = fx;
            m_usePooling = ((GameObject)m_fx.Asset).GetComponent<FX>();
        }

        public void Spawn()
        {
            if (m_usePooling)
            {
                throw new NotImplementedException();
            }
            else
            {
                AddressableSpawner.Spawn(m_fx, transform.position, 0, OnSpawn);
            }
        }

        private void OnSpawn(GameObject instance, int arg2)
        {
            instance.transform.parent = transform;
            instance.transform.localScale = Vector3.one;
            instance.transform.parent = null;
        }

        private void Awake()
        {
            m_usePooling = ((GameObject)m_fx.Asset).GetComponent<FX>();
        }
    }
}