using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public abstract class Bomb : AOEProjectile
    {
        [SerializeField, OnValueChanged("OnDetonateFXChange"), PreviewField(50f, ObjectFieldAlignment.Left)]
        private GameObject m_detonateFX;

        public override void Detonate()
        {
            base.Detonate();
            GameplaySystem.fXManager.InstantiateFX(m_detonateFX, transform.position);
        }

#if UNITY_EDITOR
        private bool OnDetonateFXChange()
        {
            if (m_detonateFX?.GetComponent<FX>() == false)
            {
                m_detonateFX = null;
                return false;
            }
            return true;
        }
#endif
    }
}