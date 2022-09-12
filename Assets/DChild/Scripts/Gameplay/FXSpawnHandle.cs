using DChild.Gameplay.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System;
using DChild.Gameplay.Environment;
using DChild.Gameplay.VFX;

namespace DChild.Gameplay
{
    public struct FXSpawnHandle<T> where T : FX
    {
        public T InstantiateFX(GameObject fx, Vector3 position, HorizontalDirection direction, Transform parent = null)
        {
            var instance = InstantiateFX(fx, position);
            instance.SetFacing(direction);
            if (parent != null)
            {
                instance.transform.SetParent(parent);
            }

            instance.Play();
            return instance;
        }

        public T InstantiateFX(GameObject fx, Vector3 position, Scene scene)
        {
            var instance = InstantiateFX(fx, position);
            SceneManager.MoveGameObjectToScene(instance.gameObject, scene);
            instance.Play();
            return instance;
        }

        public T InstantiateFX(GameObject fx, Vector3 position, Transform parent = null)
        {
            var instance = GameplaySystem.fXManager.InstantiateFX<T>(fx, position);

            if (fx != null)
            {
                instance.Play();
                if (parent != null)
                {
                    instance.transform.SetParent(parent);
                }
            }

            return instance;
        }

        public T InstantiateFX(FXSpawnConfigurationInfo info, Vector3 position, HorizontalDirection direction, Transform parent = null)
        {
            var instance = InstantiateFX(info.fx, position, direction, parent);
            ConfigureFX(instance, info);
            return instance;
        }

        public T InstantiateFX(FXSpawnConfigurationInfo info, Vector3 position, Transform parent = null)
        {
            var instance = InstantiateFX(info.fx, position, parent);
            ConfigureFX(instance, info);
            return instance;
        }

        private void ConfigureFX(T fx, FXSpawnConfigurationInfo info)
        {
            if (info.overrideColor && fx.TryGetComponentInChildren(out RendererColorChangeHandle colorHandle))
            {
                colorHandle.ApplyColor(info.colorToUse);
            }
            if (info.overrideScale)
            {
                var fxTransform = fx.transform;
                var scale = fxTransform.localScale;
                var xScaleSign = Mathf.Sign(scale.x);
                scale = info.scaleToUse;
                scale.x *= xScaleSign;
                fxTransform.localScale = scale;
            }
        }

        public void InstantiateFX(AssetReferenceFX refence, Action<GameObject, int> Callback)
        {
            GameplaySystem.fXManager.InstantiateFX(refence, Callback);
        }
    }
}