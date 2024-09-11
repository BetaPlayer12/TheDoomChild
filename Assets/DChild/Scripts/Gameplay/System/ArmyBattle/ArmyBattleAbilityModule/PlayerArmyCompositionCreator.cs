using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.ArmyBattle;

public class PlayerArmyCompositionCreator : MonoBehaviour
{
    [SerializeField]
    private PlayerArmyGroups playerArmyGroups;
    [SerializeField]
    private StoreRecruitedCharacters recruitedCharacters;
    [SerializeField]
    private ArmyCharacterData[] m_characters;
    [SerializeField]
    private ArmyCharacterData[] m_recruitedCharacters;
    [SerializeField]
    private string m_name;
    public ArmyCharacterData GetCharacter(int index) => m_characters[index];
    public int recruitedCharactersCount => m_recruitedCharacters.Length;
    public string name => m_name;
 
    public ArmyComposition CreatePlayerArmy()
    {
        InitializeRecruitedCharacters();
        List<ArmyAttackGroup> attackgroups = new List<ArmyAttackGroup>();
        List<ArmyAbilityGroup> abilitygroups = new List<ArmyAbilityGroup>();
        for (int i = 0; i < playerArmyGroups.groupCount; i++)
        {
            var armyGroup = playerArmyGroups.GetGroups(i);
            var memberAvailability = CheckMemberAvailability(armyGroup);

            if (HasAvailableMembers(memberAvailability) == false)
                continue;

            var attackgroup = new ArmyAttackGroup(armyGroup);

            if (armyGroup.canAttack)
            {
                attackgroup.SetMemberAvailability(memberAvailability);
                attackgroups.Add(attackgroup);
            }
            if (armyGroup.hasAbility)
            {
                var abilityGroup = new ArmyAbilityGroup(armyGroup);
                //we should set availability of ability group
                abilityGroup.SetAvailability(true);
                abilitygroups.Add(abilityGroup);
            }
                       
        }
        return new ArmyComposition(m_name, CalculateTotalTroopCount(), attackgroups.ToArray(), abilitygroups.ToArray());
    }

    private bool HasAvailableMembers(bool[] memberAvailability)
    {
        for(int i = 0; i < memberAvailability.Length; i++)
        {
            if(memberAvailability[i])
            {
                return true;
            }
        }
        return false;
    }
    private void InitializeRecruitedCharacters() 
    { 
        List<ArmyCharacterData> recruitedCharacter = new List<ArmyCharacterData>();
        for (int i = 0; i < m_characters.Length; i++)
        {
            var characterID = m_characters[i].ID;
            for (int j = 0; j < recruitedCharacters.recruitedCharactersIDCount; j++)
            {
                var recruitedID = recruitedCharacters.RetrieveRecruitedCharacterIDList(j);
                if (recruitedID == characterID)
                {
                    recruitedCharacter.Add(m_characters[i]);
                    break;
                }
            }
        }
        m_recruitedCharacters = recruitedCharacter.ToArray();

    }
    private bool[] CheckMemberAvailability(ArmyGroupTemplateData armyGroup)
    {
        var memberAvailability = new bool[armyGroup.memberCount];
        for (int k = 0; k < armyGroup.memberCount; k++)
        {
            memberAvailability[k] = HasBeenRecruited(armyGroup.GetMember(k));
        }
        return memberAvailability;
    }

    private bool HasBeenRecruited(ArmyCharacterData character)
    {
        
        for (int i = 0; i < recruitedCharactersCount; i++)
        {
            
            if (GetMember(i) == character)
            {
                return true;
            }
        }
        return false;
    }

    private int CalculateTotalTroopCount()
    {
        var troopCount = 0;
        for (int i = 0; i < recruitedCharactersCount; i++)
        {
            troopCount += GetMember(i).troopCount;
        }
        return troopCount;
       
    }
    private ArmyCharacterData GetMember(int index)
    {
        return m_recruitedCharacters[index];
    }

}