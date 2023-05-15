using DChild;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonLordIceBomb : MonoBehaviour
{
    [SerializeField, TabGroup("Projectiles")]
    private ProjectileInfo m_projectileInfo;
    [SerializeField, TabGroup("Projectiles")]
    private int m_projectilesCount;
    [SerializeField, TabGroup("Projectiles")]
    private int m_launchRotations;
    private SimpleAttackProjectile m_attackProjectile;
    private Transform m_target;

    //private void Start()
    //{
    //    StartCoroutine(TravelRoutine());
    //}

    private void Awake()
    {
        m_attackProjectile = GetComponent<SimpleAttackProjectile>();
    }

    public void SetTarget(Transform target)
    {
        m_target = target;
        StartCoroutine(TravelRoutine());
    }

    private IEnumerator TravelRoutine()
    {
        yield return new WaitUntil(() => Vector2.Distance(transform.position, m_target.position) <= 0.5f);
        LaunchIcePattern(m_projectilesCount, m_launchRotations);
        yield return null;
    }

    private void LaunchIcePattern(int numberOfProjectiles, int rotations)
    {
        for (int x = 0; x < rotations; x++)
        {
            float angleStep = 360f / numberOfProjectiles;
            //float angle = 45f;
            float angle = 90f;
            for (int z = 0; z < numberOfProjectiles; z++)
            {
                Vector2 startPoint = new Vector2(transform.position.x, transform.position.y);
                float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * 5;
                float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * 5;

                Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
                Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * m_projectileInfo.speed;

                GameObject projectile = m_projectileInfo.projectile;
                var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(projectile);
                instance.transform.position = transform.position;
                var component = instance.GetComponent<Projectile>();
                component.ResetState();
                //component.GetComponent<Rigidbody2D>().velocity = projectileMoveDirection;
                //Vector2 v = component.GetComponent<Rigidbody2D>().velocity;
                //var projRotation = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                //component.transform.rotation = Quaternion.AngleAxis(projRotation, Vector3.forward);
                Vector2 v = projectileMoveDirection;
                var projRotation = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                component.transform.rotation = Quaternion.AngleAxis(projRotation, Vector3.forward);
                component.GetComponent<Rigidbody2D>().velocity = projectileMoveDirection;
                //m_delayLaunchRoutine = StartCoroutine(DelayedLaunchRoutine(component, projectileMoveDirection, m_info.attackDelay));

                angle += angleStep;
            }
        }
        m_attackProjectile.ForceCollision();
        //yield return null;
    }
}
