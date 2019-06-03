/******************************
 * 
 * Manages FX that are instantiated rather than played;
 * 
 ******************************/
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.VFX
{
    public interface IFXManager
    {
        T InstantiateFX<T>(GameObject fx, Vector3 position) where T : FX;
        void InstantiateFX(GameObject fx, Vector3 position);
    }

    public class FXManager : MonoBehaviour, IGameplaySystemModule, IGameplayInitializable, IFXManager
    {
        private static FXPool m_pool;

        public void Initialize()
        {
            m_pool = GameSystem.poolManager.GetOrCreatePool<FXPool>();
        }

        public T InstantiateFX<T>(GameObject fx, Vector3 position) where T : FX => (T)SmartInstantiateFX(position, fx);
        public void InstantiateFX(GameObject fx, Vector3 position) => SmartInstantiateFX(position, fx);

        #region Instatiation
        private static FX SmartInstantiateFX(Vector3 position, GameObject fxGO)
        {
            if (fxGO == null)
                return null;

            var fx = fxGO.GetComponent<FX>();
            var pooledFX = m_pool.RetrieveFromPool(fx.fxName);
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
            pooledFX.SetParent(null);
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