using DChild.Gameplay.Environment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters
{

    public class HitFXHandle : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_fx;
        [SerializeField, ToggleGroup("m_overrideColor", "Color Override")]
        private bool m_overrideColor;
        [SerializeField, ToggleGroup("m_overrideColor", "Color Override"), HideLabel]
        private Color m_colorToUse = Color.white;
        [SerializeField, ToggleGroup("m_overrideScale", "Scale Override")]
        private bool m_overrideScale;
        [SerializeField, ToggleGroup("m_overrideScale", "Scale Override"), HideLabel]
        private Vector3 m_scaleToUse = Vector3.one;

        private FXSpawnHandle<FX> m_spawnHandle;

        public GameObject fxObject { get { return m_fx; } set { m_fx = value; } }
        public bool overrideColor {get{ return m_overrideColor; } set { m_overrideColor = value; } }
        public Color colorToUse { get { return m_colorToUse; } set { m_colorToUse = value; } }
        public bool overideScale { get { return m_overrideScale; } set { overideScale = value; } }
        public Vector3 scaleOverride { get { return m_scaleToUse; } set { m_scaleToUse = value; } }

        public void SetFX(GameObject fx) => m_fx = fx;

        public void SpawnFX(Vector2 position, HorizontalDirection direction)
        {
            var fx = m_spawnHandle.InstantiateFX(m_fx, position, direction);
            if (m_overrideColor && fx.TryGetComponentInChildren(out RendererColorChangeHandle colorHandle))
            {
                colorHandle.ApplyColor(m_colorToUse);
            }
            if (m_overrideScale)
            {
                var fxTransform = fx.transform;
                fxTransform.SetParent(transform);
                var scale = fxTransform.localScale;
                var xScaleSign = Mathf.Sign(scale.x);
                scale = m_scaleToUse;
                scale.x *= xScaleSign;
                fxTransform.localScale = scale;
            }
        }
    }
}