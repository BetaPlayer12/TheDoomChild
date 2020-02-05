using System.Collections;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class PlayerDamageCamShake : MonoBehaviour
    {
        [SerializeField, MinValue(0.1f)]
        private float m_duration;
        private Attacker m_attacker;

        private void OnTargetDamage(object sender, CombatConclusionEventArgs eventArgs)
        {
            StopAllCoroutines();
            if (eventArgs.target.isCharacter || eventArgs.target.instance.isAlive == false)
            {
                StartCoroutine(ShakeRoutine());
            }
        }

        private IEnumerator ShakeRoutine()
        {
            GameplaySystem.cinema.EnableCameraShake(true);
            yield return new WaitForSeconds(m_duration);
            GameplaySystem.cinema.EnableCameraShake(false);
        }

        private void Awake()
        {
            m_attacker = GetComponentInParent<Attacker>();
            m_attacker.TargetDamaged += OnTargetDamage;
        }
    }
}