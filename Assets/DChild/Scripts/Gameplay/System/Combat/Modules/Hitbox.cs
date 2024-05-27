/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using DChild.Gameplay.VFX;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [AddComponentMenu("DChild/Gameplay/Combat/Hitbox")]
    public class Hitbox : MonoBehaviour
    {
        public const string TAG = "Hitbox";

        private IDamageable m_damageable;
        [SerializeField, DisableInPlayMode, HideInEditorMode]
        private Collider2D[] m_collider2Ds;
        [SerializeField]
        private FXSpawnConfigurationInfo m_damageFXInfo;
        [SerializeField]
        private Invulnerability m_invulnerabilityLevel;
        [SerializeField, HideIf("@m_invulnerabilityLevel == Invulnerability.MAX"), Range(0, 0.99f)]
        private float m_damageReduction;
        [SerializeField]
        private bool m_canBlockDamage;

        public IDamageable damageable => m_damageable;
        public FXSpawnConfigurationInfo damageFXInfo => m_damageFXInfo;
        public BodyDefense defense => new BodyDefense(m_invulnerabilityLevel, m_damageReduction);
        public Invulnerability invulnerabilityLevel => m_invulnerabilityLevel;

        public bool canBlockDamage => m_canBlockDamage;

        public void Enable()
        {
            for (int i = 0; i < m_collider2Ds.Length; i++)
            {
                m_collider2Ds[i].enabled = true;
            }
        }

        [Button]
        public void Disable()
        {
            for (int i = 0; i < m_collider2Ds.Length; i++)
            {
                m_collider2Ds[i].enabled = false;
            }
        }

        public virtual bool CanBeDamageBy(params Collider2D[] colliders) => true;

        public void SetInvulnerability(Invulnerability value) => m_invulnerabilityLevel = value;

        public void SetCanBlockDamageState(bool canBlockDamage) => m_canBlockDamage = canBlockDamage;

        private void Awake()
        {
            m_damageable = GetComponentInParent<IDamageable>();
        }

        public void OvverideDamageable(IDamageable changeDamageable)
        {
            m_damageable = changeDamageable;
        }

        private void OnValidate()
        {
            var compositeCollider = GetComponent<CompositeCollider2D>();
            if (compositeCollider)
            {
                var colliders = new List<Collider2D>(GetComponentsInChildren<Collider2D>());
                colliders.Remove(compositeCollider);
                m_collider2Ds = colliders.ToArray();
            }
            else
            {
                m_collider2Ds = GetComponentsInChildren<Collider2D>();
            }

            foreach (var collider2D in m_collider2Ds)
            {
                if(collider2D.tag != Hitbox.TAG)
                {
                    collider2D.tag = Hitbox.TAG;
                }
            }
        }
    }
}
