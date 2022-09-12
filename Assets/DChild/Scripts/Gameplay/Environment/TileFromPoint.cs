using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild
{
    [ExecuteAlways]
    public class TileFromPoint : MonoBehaviour
    {
        private enum Dimenision
        {
            Height,
            Width
        }

        [SerializeField]
        private bool m_runInPlayMode = true;
        [SerializeField, DisableInPlayMode]
        private Dimenision m_dimensionToTile;
        [SerializeField]
        private float m_ratio = 0.1f;
        [SerializeField, HorizontalGroup("Use")]
        private bool m_useX;
        [SerializeField, HorizontalGroup("Use")]
        private bool m_useY;
        [SerializeField]
        private SpriteRenderer[] m_renderers;

        private void Awake()
        {
            enabled = m_runInPlayMode;
        }

        private void LateUpdate()
        {
            var position = transform.position;
            Vector2 reference = new Vector2(m_useX ? position.x : 0, m_useY ? position.y : 0);

            for (int i = 0; i < m_renderers.Length; i++)
            {
                var renderPos = m_renderers[i].transform.position;
                var target = new Vector2(m_useX ? renderPos.x : 0, m_useY ? renderPos.y : 0);
                var distance = (reference - target).magnitude;

                var size = m_renderers[i].size;
                if (m_dimensionToTile == Dimenision.Height)
                {
                    size.y = m_ratio * distance;
                }
                else
                {
                    size.x = m_ratio * distance;
                }
                m_renderers[i].size = size;
            }
        }
    }

}