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

public class CollectathonEntity : MonoBehaviour
{
    private enum EntityType
    {
        NormalChest,
        SeedsOfTheOne,
        ShardChest,
        SoulSkillChest,
    }

    [SerializeField]
    private DialogueSystemTrigger m_trigger;
    [SerializeField]
    private List<LootChest> m_chestList;
    [SerializeField]
    private List<LootChest> m_soulSkillList;
    [SerializeField]
    private List<LootChest> m_shardList;
    [SerializeField]
    private List<Damageable> m_seedList;
    [SerializeField]
    private Location m_location;
    [SerializeField]
    private CollectathonTypes m_type;

    public UnityEvent m_luaOverride;

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
