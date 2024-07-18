using UnityEngine;
using Sirenix.OdinInspector;
using DChild.Gameplay.Projectiles;

namespace DChild.Gameplay.Characters.Enemies
{
    public class RoyalDeathGuardDeathStenchWave : MonoBehaviour
    {
        [SerializeField]
        private float m_speed;
        [SerializeField]
        private float m_launchXOffset;
        [SerializeField]
        private SimpleAttackProjectile m_rightProjectile;
        [SerializeField]
        private SimpleAttackProjectile m_leftProjectile;

        private ParticleSystem m_rightProjectileFX;
        private ParticleSystem m_leftProjectileFX;

        [Button,HideInEditorMode]
        public void Execute()
        {
            LaunchProjectile(m_rightProjectile, m_rightProjectileFX, Vector2.right);
            LaunchProjectile(m_leftProjectile, m_leftProjectileFX, Vector2.left);
            m_leftProjectile.transform.localScale = Vector3.one; //Force Left Projectile to Be at the right orientation cuz Projectile.Launch changes its orientation
        }

        private void LaunchProjectile(SimpleAttackProjectile projectile, ParticleSystem fx, Vector2 direction)
        {
            projectile.gameObject.SetActive(true);
            projectile.transform.localPosition = direction * m_launchXOffset;
            projectile.Launch(direction, m_speed);
            fx.Play();
        }

        private void Awake()
        {
            m_rightProjectileFX = m_rightProjectile.GetComponent<ParticleSystem>();
            m_leftProjectileFX = m_leftProjectile.GetComponent<ParticleSystem>();
        }
    }
}