using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullThrow : MonoBehaviour, ICancellableBehaviour, IResettableBehaviour, IComplexCharacterModule
{
    [SerializeField]
    private ProjectileInfo m_projectile;
    [SerializeField]
    private Transform m_spawnPoint;

    private Character m_character;
    private ProjectileLauncher m_launcher;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Execute();
        }
    }

    public void Execute()
    {
        //m_projectileHandle.Launch(m_projectile.projectile, m_spawnPoint.position, new Vector2((float)m_character.facing, 0), m_projectile.speed);
        m_launcher.LaunchProjectile(new Vector2((float)m_character.facing, 0));
    }

    public void Cancel()
    {

    }

    public void Reset()
    {

    }

    public void Initialize(ComplexCharacterInfo info)
    {
        m_character = info.character;
        m_launcher = new ProjectileLauncher(m_projectile, m_spawnPoint);

        m_launcher.SetProjectile(m_projectile);
        m_launcher.SetSpawnPoint(m_spawnPoint);
    }
}
