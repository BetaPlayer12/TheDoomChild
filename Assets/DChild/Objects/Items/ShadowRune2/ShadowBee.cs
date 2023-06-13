using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Items;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBee : MonoBehaviour
{
    [SerializeField]
    private float m_duration;
    [SerializeField]
    private float m_interval;
    [SerializeField]
    private float m_rotationSpeed;
    [SerializeField, BoxGroup("Projectile")]
    private ProjectileInfo m_projectileInfo;
    [SerializeField, BoxGroup("Projectile")]
    private List<Transform> m_launcherPoints;
    [SerializeField, BoxGroup("Projectile")]
    private Transform m_rotationControl;
    [SerializeField, BoxGroup("FX")]
    private List<ParticleFX> m_fxs;
    //[SerializeField]
    //private PoolableObject m_object;
    private ProjectileLauncher m_launcher;
    private SimpleAttackProjectile m_projectile;
    private ShadowPetHandler m_eventHandler;

    private Coroutine m_rotationControlRoutine;

    private void Awake()
    {
        m_projectile = GetComponent<SimpleAttackProjectile>();
        m_launcher = new ProjectileLauncher(m_projectileInfo, m_launcherPoints[0]);
        m_eventHandler = GetComponent<ShadowPetHandler>();
    }

    private void Start()
    {
        StartCoroutine(ShadowBeeRoutine());
        m_rotationControlRoutine = StartCoroutine(RotationRoutine());
    }

    private IEnumerator ShadowBeeRoutine()
    {
        var timer = m_duration;
        var interval = m_interval;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            interval -= Time.deltaTime;
            if (interval < 0)
            {
                for (int i = 0; i < m_launcherPoints.Count; i++)
                {
                    m_launcher = new ProjectileLauncher(m_projectileInfo, m_launcherPoints[i]);
                    var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectileInfo.projectile);
                    instance.transform.position = m_launcherPoints[i].position;
                    if (GetComponentInParent<Character>() != null)
                        instance.GetComponent<Attacker>().SetParentAttacker(GetComponentInParent<Character>().GetComponent<Attacker>());

                    if (GetComponentInParent<Character>() != null)
                    {
                        m_launcher.AimAt(new Vector2(m_launcherPoints[i].position.x + (GetComponentInParent<Character>().facing == HorizontalDirection.Right ? 5 : -5), m_launcherPoints[i].position.y));
                    }
                    else
                    {
                        m_launcher.AimAt(new Vector2(m_launcherPoints[i].position.x + 5, m_launcherPoints[i].position.y));
                    }

                    m_launcher.LaunchProjectile(m_launcherPoints[i].right, instance.gameObject);
                }

                interval = m_interval;
            }
            yield return null;
        }
        for (int i = 0; i < m_fxs.Count; i++)
        {
            m_fxs[i].Stop();
        }
        if (m_rotationControlRoutine != null)
        {
            StopCoroutine(m_rotationControlRoutine);
            m_rotationControlRoutine = null;
        }
        m_rotationControl.localRotation = Quaternion.identity;
        m_eventHandler.PetDesummon();
        m_projectile.CallPoolRequest();
        yield return null;
    }

    private IEnumerator RotationRoutine()
    {
        while (true)
        {
            m_rotationControl.Rotate(0, 0, m_rotationSpeed, Space.World);
            yield return null;
        }
    }
}
