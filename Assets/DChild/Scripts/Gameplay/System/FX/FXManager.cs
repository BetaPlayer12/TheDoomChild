/******************************
 * 
 * Manages FX that are instantiated rather than played;
 * 
 ******************************/
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.VFX
{
    public interface IFXManager
    {
        T InstantiateFX<T>(GameObject fx, Vector3 position) where T : FX;
        void InstantiateFX(GameObject fx, Vector3 position);

        void InstantiateFX(AssetReferenceFX reference, Action<GameObject, int> Callback);
    }

    public class FXManager : MonoBehaviour, IGameplaySystemModule, IGameplayInitializable, IFXManager
    {
        private static FXPool m_pool;

        public void Initialize()
        {
            m_pool = GameSystem.poolManager?.GetPool<FXPool>() ?? null;
        }

        public T InstantiateFX<T>(GameObject fx, Vector3 position) where T : FX => (T)SmartInstantiateFX(position, fx);
        public void InstantiateFX(GameObject fx, Vector3 position) => SmartInstantiateFX(position, fx);

        public void InstantiateFX(AssetReferenceFX reference, Action<GameObject, int> Callback)
        {
            m_pool.GetOrCreateItem(reference, 0, Callback);
        }

        #region Instatiation
        private static FX SmartInstantiateFX(Vector3 position, GameObject fxGO)
        {
            if (fxGO == null)
                return null;

            var pooledFX = m_pool.GetOrCreateItem(fxGO, position, Quaternion.identity);
            if (pooledFX == null)
            {
                return InstantiateFX(position, fxGO);
            }
            else
            {
                return UsePooledFX(position, pooledFX);
            }
        }



        private static FX UsePooledFX(Vector3 position, FX pooledFX)
        {
            pooledFX.transform.parent = null;
            pooledFX.transform.position = position;
            pooledFX.gameObject.SetActive(true);
            pooledFX.Play();
            return pooledFX;
        }

        private static FX InstantiateFX(Vector3 position, GameObject fxGO)
        {
            var instantiatedFX = Instantiate(fxGO, position, Quaternion.identity);
            var particleFX = instantiatedFX.GetComponent<FX>();
            particleFX.Play();
            return particleFX;
        }



        #endregion
    }
}