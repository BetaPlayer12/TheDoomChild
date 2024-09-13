using DChild.Gameplay.ArmyBattle;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerArmyGroups", menuName = "DChild/Gameplay/Army/Player Army Groups")]
public class PlayerArmyGroups : ScriptableObject
{

    [SerializeField, InlineEditor(Expanded = true)]
    private ArmyGroupTemplateData[] m_armyGroups;

    public ArmyGroupTemplateData GetGroups(int index) => m_armyGroups[index];
    public int groupCount => m_armyGroups.Length;
}
