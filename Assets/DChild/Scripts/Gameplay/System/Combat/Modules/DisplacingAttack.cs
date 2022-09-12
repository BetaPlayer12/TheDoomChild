/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using UnityEngine;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Combat
{
    public class DisplacingAttack : MonoBehaviour
    {
        [InfoBox("IF There is a Character Compnent in its parent, it values should assume Right Facing")]
        [SerializeField]
        private bool m_awayFromPivotPoint = true;
        [SerializeField, ShowIf("m_awayFromPivotPoint")]
        private float m_force;
        [SerializeField, HideIf("m_awayFromPivotPoint")]
        private Vector2 m_specificForce;

        private Character m_character;
        private ColliderDamage m_colliderDamage;
        private IAttacker m_attacker;

        private IDamageable m_cacheDamageable;

        private void Awake()
        {
            m_character = GetComponentInChildren<Character>();
            m_colliderDamage = GetComponent<ColliderDamage>();
            m_colliderDamage.DamageableDetected += OnTargetDetected;
            m_attacker = GetComponentInParent<IAttacker>();
            m_attacker.TargetDamaged += OnTargetDamage;
        }

        private void OnTargetDetected(TargetInfo arg1, Collider2D arg2)
        {
            m_cacheDamageable = arg1.instance;
        }

        private void OnTargetDamage(object sender, CombatConclusionEventArgs eventArgs)
        {
            if (m_cacheDamageable == eventArgs.target.instance)
            {
                if (((MonoBehaviour)m_cacheDamageable).TryGetComponentInChildren(out DisplacementHandler displacementHandler))
                {
                    Vector2 displacementForce = Vector2.zero;

                    if (m_awayFromPivotPoint)
                    {
                        var directionToTarget = (m_cacheDamageable.position - (Vector2)transform.position).normalized;
                        displacementForce = new Vector2(Mathf.Sign(directionToTarget.x) * m_force, 0);
                    }
                    else
                    {
                        if (m_character == null)
                        {
                            displacementForce = m_specificForce;
                        }
                        else
                        {
                            displacementForce = m_specificForce * (m_character.facing == HorizontalDirection.Right ? 1 : -1);
                        }
                    }
                    displacementHandler.Execute(displacementForce);
                }
            }
        }
    }
}
