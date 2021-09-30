#if UNITY_EDITOR
using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Injects The Stat to a Character based off the database
/// </summary>
public class EnemyStatInjector : MonoBehaviour
{
    [SerializeField]
    private Character m_toInject;
    [SerializeField, ValueDropdown("GetIDs")]
    private int m_enemy;
    [SerializeField, MinValue(1)]
    private int m_healthModifier;
    [SerializeField, MinValue(1)]
    private int m_damageModifier;
    [SerializeField]
    private DamageType m_damageType;

    protected IEnumerable GetIDs()
    {
        var connection = DChildDatabase.GetBestiaryConnection();
        connection.Initialize();
        var infoList = connection.GetAllInfo();
        connection.Close();

        var list = new ValueDropdownList<int>();
        for (int i = 0; i < infoList.Length; i++)
        {
            list.Add(infoList[i].name, infoList[i].ID);
        }
        return list;
    }

    [Button]
    private void Inject()
    {
        var connection = DChildDatabase.GetBestiaryConnection();
        connection.Initialize();
        var ratings = connection.GetRatings(m_enemy);
        m_toInject.GetComponentInChildren<Health>().SetMaxValue(ratings.HP * m_damageModifier);
        m_toInject.GetComponentInChildren<Attacker>().SetDamage(new Damage(m_damageType, ratings.DMG * m_damageModifier));
    }
}

#endif