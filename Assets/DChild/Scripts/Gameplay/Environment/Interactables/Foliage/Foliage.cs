using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Physics;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Systems.WorldComponents;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class Foliage : MonoBehaviour, IFoliage
    {
        [SerializeField]
        public float m_collisionForce;
        [SerializeField]
        public float m_displacementSpeed;
        [SerializeField]
        public float m_returnSpeed;
        [SerializeField]
        public float m_flipSpeed;

        private float m_shakeBendingDefault;
        private Vector4 m_WaveXmoveDefault;
        private Vector4 m_WaveZmoveDefault;

        private bool m_canCollideAgain;
        private bool m_reverse;
        private bool m_isReturning;

        private Renderer m_renderer;
        private MaterialPropertyBlock _propBlock;

        public float collisionForce => m_collisionForce;
        public float displacementSpeed => m_displacementSpeed;
        public float returnSpeed => m_returnSpeed;
        public float flipSpeed => m_flipSpeed;

        public bool CanCollideAgain { get { return m_canCollideAgain; } set { m_canCollideAgain = value; } }
        public bool Reverse { get { return m_reverse; } set { m_reverse = value; } }
        public bool IsReturning { get { return m_isReturning; } set { m_isReturning = value; } }

        public Vector2 Location { get { return transform.position; } }

        private const string SHAKEBENDING_VARNAME = "_ShakeBending";
        private const string WAVEMOVE_X_VARNAME = "_WaveXmove";
        private const string WAVEMOVE_Z_VARNAME = "_WaveZmove";

        public void ReverseVertices()
        {
            var xMove = _propBlock.GetVector(WAVEMOVE_X_VARNAME);
            var zMove = _propBlock.GetVector(WAVEMOVE_Z_VARNAME);
            if (xMove.w > 0)
            {
                var waveMove = m_flipSpeed * GameplaySystem.time.deltaTime;

                xMove.w -= waveMove;
                xMove.x -= waveMove;
                xMove.y += waveMove;
                xMove.z -= waveMove;

                _propBlock.SetVector(WAVEMOVE_X_VARNAME, -xMove);

                zMove.w -= waveMove;
                zMove.x -= waveMove;
                zMove.y += waveMove;
                zMove.z -= waveMove;

                _propBlock.SetVector(WAVEMOVE_X_VARNAME, -zMove);

            }
            else
            {
                _propBlock.SetVector(WAVEMOVE_X_VARNAME, -xMove);
                _propBlock.SetVector(WAVEMOVE_X_VARNAME, -zMove);

                m_isReturning = true;
                m_reverse = false;
            }
        }

        public void ReturnToDefault()
        {
            var shakeBending = _propBlock.GetFloat(SHAKEBENDING_VARNAME);

            if (m_isReturning)
            {
                if (_propBlock.GetFloat(SHAKEBENDING_VARNAME) < m_collisionForce)
                {

                    _propBlock.SetFloat(SHAKEBENDING_VARNAME, _propBlock.GetFloat(SHAKEBENDING_VARNAME) + (m_displacementSpeed * GameplaySystem.time.deltaTime));
                }
                else
                {
                    m_isReturning = false;
                }
            }
            else
            {
                if (shakeBending > m_shakeBendingDefault)
                {
                    _propBlock.SetFloat(SHAKEBENDING_VARNAME, shakeBending - (m_returnSpeed * GameplaySystem.time.deltaTime));
                }
                else
                {
                    if (_propBlock.GetVector(WAVEMOVE_X_VARNAME) != m_WaveXmoveDefault)
                    {
                        BendToRight();
                    }
                    else
                    {
                        BendToLeft();
                    }
                }
            }
        }

        public void SetToDefault()
        {
            _propBlock.SetFloat(SHAKEBENDING_VARNAME, m_shakeBendingDefault);
            _propBlock.SetVector(WAVEMOVE_X_VARNAME, m_WaveXmoveDefault);
            _propBlock.SetVector(WAVEMOVE_Z_VARNAME, m_WaveZmoveDefault);
        }

        public void Initialized()
        {
            m_renderer = GetComponent<Renderer>();
            _propBlock = new MaterialPropertyBlock();
            m_shakeBendingDefault = 1f;
            m_WaveXmoveDefault = new Vector4(0.024f, 0.04f, -0.12f, 0.096f);
            m_WaveZmoveDefault = new Vector4(0.006f, 0.02f, -0.02f, 0.1f);
        }

        public void ApplyChanges()
        {
            m_renderer.SetPropertyBlock(_propBlock);
        }

        public void GetPropertyBlock()
        {
            m_renderer.GetPropertyBlock(_propBlock);
        }

        private void BendToLeft()
        {
            if (_propBlock.GetFloat(SHAKEBENDING_VARNAME) > m_shakeBendingDefault)
            {
                _propBlock.SetFloat(SHAKEBENDING_VARNAME, _propBlock.GetFloat(SHAKEBENDING_VARNAME) + (m_displacementSpeed * GameplaySystem.time.deltaTime));
            }
            else
            {
                _propBlock.SetFloat(SHAKEBENDING_VARNAME, m_shakeBendingDefault);
                m_canCollideAgain = true;
            }
        }

        private void BendToRight()
        {
            if (_propBlock.GetFloat(SHAKEBENDING_VARNAME) > (m_shakeBendingDefault * 0.05f))
            {
                _propBlock.SetFloat(SHAKEBENDING_VARNAME, _propBlock.GetFloat(SHAKEBENDING_VARNAME) - (m_displacementSpeed * GameplaySystem.time.deltaTime));
            }
            else
            {
                _propBlock.SetVector(WAVEMOVE_X_VARNAME, m_WaveXmoveDefault);
                _propBlock.SetVector(WAVEMOVE_Z_VARNAME, m_WaveZmoveDefault);
            }
        }
    }
}
