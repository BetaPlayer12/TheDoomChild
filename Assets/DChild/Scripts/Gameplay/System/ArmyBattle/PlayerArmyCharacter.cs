using DChild.Gameplay.ArmyBattle;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerArmyCharacters", menuName = "DChild/Gameplay/Army/Player Army Characters")]
public class PlayerArmyCharacter : ScriptableObject
{

    [SerializeField, InlineEditor(Expanded = true)]
    private ArmyCharacter[] m_characters;
    

    public ArmyCharacter GetMember(int index) => m_characters[index];
}
