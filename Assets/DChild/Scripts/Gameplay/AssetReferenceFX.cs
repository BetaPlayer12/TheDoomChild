using UnityEngine;
using UnityEngine.AddressableAssets;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay
{
    [System.Serializable]
    public class AssetReferenceFX : AssetReferenceT<GameObject>
    {
        public AssetReferenceFX(string guid) : base(guid)
        {

        }

        public override bool ValidateAsset(Object obj)
        {
            var go = obj as GameObject;
            return go != null && (go.GetComponent<FX>() != null || go.GetComponent<ParticleSystem>());
        }

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            //this load can be expensive...
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return go != null && (go.GetComponent<FX>() != null|| go.GetComponent<ParticleSystem>());
#else
            return false;
#endif
        }
    }
}