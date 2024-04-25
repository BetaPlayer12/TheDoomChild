using DChild.Gameplay;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Environment;
using DChild.Gameplay.NavigationMap;
using DChild.UI;
using Holysoft.Event;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Location = DChild.Gameplay.Environment.Location;

public class CollectathonEntityTracker : MonoBehaviour
{
    private enum EntityType
    {
        NormalChest,
        SeedsOfTheOne,
        ShardChest,
        SoulSkillChest,
    }

    [SerializeField]
    private Location m_location;
    [SerializeField]
    private DialogueSystemTrigger m_trigger;
    [SerializeField, ValueDropdown("GetChestList", IsUniqueList = true, SortDropdownItems = true)]
    private List<LootChest> m_chestList;
    [SerializeField, ValueDropdown("GetChestList", IsUniqueList = true, SortDropdownItems = true)]
    private List<LootChest> m_soulSkillList;
    [SerializeField, ValueDropdown("GetChestList", IsUniqueList = true, SortDropdownItems = true)]
    private List<LootChest> m_shardList;
    [SerializeField, ValueDropdown("GetSeedsOfTheOne", IsUniqueList = true, SortDropdownItems = true)]
    private List<Damageable> m_seedList;


    private CollectathonTypes m_type;
    public UnityEvent m_luaOverride;

    private IEnumerable GetChestList()
    {
        Dictionary<string, int> nameToInstanceCountPair = new Dictionary<string, int>();

        ValueDropdownList<LootChest> list = new ValueDropdownList<LootChest>();
        var chests = FindObjectsOfType<LootChest>();
        for (int i = 0; i < chests.Length; i++)
        {
            var chest = chests[i];
            var chestName = chest.name;

            if (nameToInstanceCountPair.TryGetValue(chestName, out int index))
            {
                index++;
                chestName = $"{chest.name} [{index}]";
                nameToInstanceCountPair[chest.name] = index;
            }
            else
            {
                nameToInstanceCountPair.Add(chest.name, 1);
            }

            var itemName = GenerateCategory(chest.name) + chestName;
            list.Add(itemName, chest);
        }

        return list;

        string GenerateCategory(string chestName)
        {
            string category = "";
            if (chestName.Contains("Soul"))
            {
                category = "Soul Skill";
            }
            else if (chestName.Contains("Health") || chestName.Contains("Shadow") || chestName.Contains("Weapon"))
            {
                category = "Shard";
            }
            else
            {
                category = "Loot";
            }

            return category + " Chests/";
        }
    }

    private IEnumerable GetSeedsOfTheOne()
    {
        Dictionary<string, int> nameToInstanceCountPair = new Dictionary<string, int>();
        ValueDropdownList<Damageable> list = new ValueDropdownList<Damageable>();
        var ais = FindObjectsOfType<SeedOfTheOneAI>();
        for (int i = 0; i < ais.Length; i++)
        {
            var ai = ais[i];

            var aiName = ai.name;
            if (nameToInstanceCountPair.TryGetValue(aiName, out int index))
            {
                index++;
                aiName = $"{ai.name} [{index}]";
                nameToInstanceCountPair[ai.name] = index;
            }
            else
            {
                nameToInstanceCountPair.Add(ai.name, 1);
            }

            var itemName = "Everything/" + aiName;
            list.Add(itemName, ai.GetComponent<Damageable>());
        }

        return list;
    }

    private void LuaCodeOverride(CollectathonTypes types)
    {

        var variableName = CollecathonUtility.GenerateCurrentCountVariableName(types, m_location);
        var variableSyntax = $"Variable[\"{variableName}\"]";
        var increment = 1;
        m_trigger.luaCode = $"{variableSyntax} = {variableSyntax} + {increment}";
        m_luaOverride?.Invoke();
    }

    private void OnSeedInteraction(object sender, EventActionArgs eventArgs)
    {
        m_type = CollectathonTypes.SeedsOfTheOne;
        LuaCodeOverride(m_type);
    }

    private void OnLootChestInteraction(object sender, EventActionArgs eventArgs)
    {
        m_type = CollectathonTypes.NormalChest;
        LuaCodeOverride(m_type);
    }

    private void OnShardChestInteraction(object sender, EventActionArgs eventArgs)
    {
        m_type = CollectathonTypes.ShardChest;
        LuaCodeOverride(m_type);
    }
    private void OnSoulSkillChestInteraction(object sender, EventActionArgs eventArgs)
    {
        m_type = CollectathonTypes.SoulSkillChest;
        LuaCodeOverride(m_type);
    }

#if UNITY_EDITOR
    [Button]
    private void Interact()
    {
        LuaCodeOverride(m_type);
    }

#endif

    void Awake()
    {

        if (m_chestList != null)
        {

            for (int x = 0; x < m_chestList.Count; x++)
            {
                m_chestList[x].InteractionOptionChange += OnLootChestInteraction;
            }
        }

        if (m_seedList != null)
        {
            for (int x = 0; x < m_seedList.Count; x++)
            {
                m_seedList[x].Destroyed += OnSeedInteraction;
            }
        }

        if (m_soulSkillList != null)
        {
            for (int x = 0; x < m_soulSkillList.Count; x++)
            {
                m_soulSkillList[x].InteractionOptionChange += OnSoulSkillChestInteraction;
            }
        }

        if (m_shardList != null)
        {
            for (int x = 0; x < m_shardList.Count; x++)
            {
                m_shardList[x].InteractionOptionChange += OnShardChestInteraction;
            }
        }


    }



    void LateUpdate()
    {

    }
}
