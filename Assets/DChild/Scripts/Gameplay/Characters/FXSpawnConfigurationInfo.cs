using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.VFX
{
    [System.Serializable, BoxGroup]
    public class FXSpawnConfigurationInfo
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

        public FXSpawnConfigurationInfo(GameObject fx, Color colorToUse, Vector3 scaleToUse)
        {
            m_fx = fx;
            m_overrideColor = true;
            m_colorToUse = colorToUse;
            m_overrideScale = true;
            m_scaleToUse = scaleToUse;
        }

        public FXSpawnConfigurationInfo(GameObject fx, Vector3 scaleToUse)
        {
            m_fx = fx;
            m_overrideColor = false;
            m_colorToUse = Color.white;
            m_overrideScale = true;
            m_scaleToUse = scaleToUse;
        }

        public FXSpawnConfigurationInfo(GameObject fx, Color colorToUse)
        {
            m_fx = fx;
            m_overrideColor = true;
            m_colorToUse = colorToUse;
            m_overrideScale = false;
            m_scaleToUse = Vector3.zero;
        }

        public GameObject fx { get { return m_fx; } set { m_fx = value; } }
        public bool overrideColor { get { return m_overrideColor; } set { m_overrideColor = value; } }
        public Color colorToUse { get { return m_colorToUse; } set { m_colorToUse = value; } }
        public bool overrideScale { get { return m_overrideScale; } set { m_overrideScale = value; } }
        public Vector3 scaleToUse { get { return m_scaleToUse; } set { m_scaleToUse = value; } }
    }
}