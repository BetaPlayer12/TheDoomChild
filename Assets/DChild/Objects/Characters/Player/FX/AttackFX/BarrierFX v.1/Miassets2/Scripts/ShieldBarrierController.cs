using System;
using DChild;
using DChild.Gameplay.Combat;
using UnityEngine;

public class ShieldBarrierController : MonoBehaviour
{
    [SerializeField] private int maxLife = 100;
    [SerializeField] private int damageAmount = 20;
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private string enemyTag = "EnemyProjectile";
    [SerializeField] private int currentLife;
    [SerializeField]
    private Damageable m_damageable;

    public Animator animator;
    public string damagedTriggerName = "Damaged";
    public string destroyedTriggerName = "Destroyed";

    private void Start()
    {
        currentLife = maxLife;
        m_damageable.DamageTaken += DamageTaken;
        m_damageable.DamageBlock += DamageTaken;
    }

    private void DamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
    {
        Debug.Log("Barrier taken Damage");
        TakeDamage();
    }

    private void TakeDamage()
    {
        Debug.Log("Barrier taken Damage FUNCTION");
        currentLife -= damageAmount;
        animator.SetTrigger(damagedTriggerName);

        if (currentLife <= 0)
        {
            //animator.SetTrigger(destroyedTriggerName);
            //Destroy(gameObject, destroyDelay);
        }
    }
}
