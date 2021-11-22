using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public class ImmortalPoleGuy : MonoBehaviour
    {
        [SerializeField,ReadOnly]
        private string m_idleAnimation = "idle";
        [SerializeField, ReadOnly]
        private string m_flinchAnimation = "hit animation";

        private SpineAnimation m_animation;

        public void SetAnimations(string idleAnimation, string flinchAnimation)
        {
            m_idleAnimation = idleAnimation;
            m_flinchAnimation = flinchAnimation;
        }

        private void Start()
        {
            var damageable = GetComponent<Damageable>();
            damageable.DamageTaken += OnDamageTaken;
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            m_animation.SetAnimation(0, m_flinchAnimation, false);
            m_animation.AddAnimation(0, m_idleAnimation, true, 0);
        }

        public void Reset()
        {
            m_animation.SetAnimation(0, m_idleAnimation, true);
        }
    }
}