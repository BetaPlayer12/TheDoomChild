using UnityEngine;

public class ShieldBarrierController : MonoBehaviour
{
    [SerializeField] private int maxLife = 100;
    [SerializeField] private int damageAmount = 20;
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private string enemyTag = "EnemyProjectile";
    [SerializeField] private int currentLife;

    public Animator animator;
    public string damagedTriggerName = "Damaged";
    public string destroyedTriggerName = "Destroyed";

    private void Start()
    {
        currentLife = maxLife;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(enemyTag))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        currentLife -= damageAmount;
        animator.SetTrigger(damagedTriggerName);

        if (currentLife <= 0)
        {
            animator.SetTrigger(destroyedTriggerName);
            Destroy(gameObject, destroyDelay);
        }
    }
}
