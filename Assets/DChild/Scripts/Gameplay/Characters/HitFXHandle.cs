﻿using DChild.Gameplay.Environment;
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