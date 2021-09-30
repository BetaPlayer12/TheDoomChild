using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTesting : Minion
{
    [SerializeField]
    private Image m_healthUi;

    [SerializeField]
    private Transform m_damagePosition;
    [SerializeField]
    private Damage m_damage;

    public float CurrentHealth => m_health.currentValue;
    protected override Damage startDamage => m_damage;
    protected override CombatCharacterAnimation animation => null;
    public override bool isAlive => m_health.currentValue != 0;

    private void Update()
    {
        m_healthUi.fillAmount = m_health.currentValue * .01f;
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        //InitializeAs(true);
    }
}
