using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Projectiles;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ProjectileThrow : MonoBehaviour
    {
        [SerializeField]
        [PreviewField(50, ObjectFieldAlignment.Left), ValidateInput("ValidateProjectile")]
        private GameObject m_projectile;
        [SerializeField]
        private Vector2 m_defaultAim;
        [SerializeField]
        private float m_defaultThrowForce;

        private Vector2 m_currentAim;
        private float m_currentThrowForce;

        public Vector2 currentAim => m_currentAim;
        public float currentThrowForce => m_currentThrowForce;

        public GameObject projectile => m_projectile;

        public void SetProjectile(GameObject projectile) => m_projectile = projectile;

        public void AdjustAim(Vector2 direction)
        {
            var xSign = Mathf.Sign(m_currentAim.x);
            m_currentAim = (m_currentAim + direction).normalized;
            //m_currentAim.x *= xSign;
        }

        public void AddThrowForce(float force) => m_currentThrowForce += force;
        public void ResetAim() => m_currentAim = m_defaultAim;
        public void ResetThrowForce() => m_currentThrowForce = m_defaultThrowForce;
        public void SetDefaultForce(float force) => m_defaultThrowForce = force;
        public void SetDefaultAim(Vector2 aim) => m_defaultAim = aim;

        public void Throw()
        {
            var projectileGO = this.InstantiateToScene(m_projectile, transform.position, transform.rotation);
            var projectile = projectileGO.GetComponent<Projectile>();
            projectile.SetVelocity(m_currentAim, m_currentThrowForce);
        }
    }
}