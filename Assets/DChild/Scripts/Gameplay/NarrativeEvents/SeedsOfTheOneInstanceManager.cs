using DChild.Gameplay.Combat;
using DChild.Gameplay.Quests;
using Holysoft.Collections;
using Holysoft.Event;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedsOfTheOneInstanceManager : MonoBehaviour
{
    [SerializeField]
    private ForceQuestUpdateHandle m_forceQuestUpdateHandle;
    [SerializeField,VariablePopup(true)]
    private string m_connectedVariable;
    [SerializeField] 
    private DialogueSystemTrigger m_seedsQuestStartDialogueSystemTrigger;
    [SerializeField]
    private DialogueSystemTrigger m_seedsQuestEndDialogueSystemTrigger;
    [SerializeField]
    private Flag instanceTracker;
    [SerializeField]
    private List<Damageable> m_SeedsOfTheOne;

    private void Awake()
    {
        for( int i = 0; i < m_SeedsOfTheOne.Count; i++)
        {
            m_SeedsOfTheOne[i].Destroyed += OnSeedDies;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //instanceTracker.HasFlag(Flag.Index0); //Check if True
        //instanceTracker |= Flag.Index0; //Add
        //instanceTracker &= ~Flag.Index2; //Remove

        //var value = 1 << i;

        instanceTracker = (Flag)(DialogueLua.GetVariable(m_connectedVariable).asInt);

        for (int i = 0; i < m_SeedsOfTheOne.Count; i++)
        {
            var currentFlag = (Flag)(1 << i);

            if (instanceTracker.HasFlag(currentFlag))
            {
                m_SeedsOfTheOne[i].gameObject.SetActive(false);
            }
        }

        DialogueLua.SetVariable(m_connectedVariable, (int)instanceTracker);

    }

    private void OnSeedDies(object sender, EventActionArgs eventArgs)
    {
        AddSeedFlag();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void AddSeedFlag()
    {
        for (int i = 0; i < m_SeedsOfTheOne.Count; i++)
        {
            var currentFlag = (Flag)(1 << i);

            if (!m_SeedsOfTheOne[i].isAlive)
            {
                instanceTracker |= currentFlag;
            }

            if (instanceTracker.HasFlag(currentFlag))
            {
                m_SeedsOfTheOne[i].gameObject.SetActive(false);
            }
        }

        DialogueLua.SetVariable(m_connectedVariable, (int)instanceTracker);
        DialogueLua.SetVariable("Seed_Dead_Count", DialogueLua.GetVariable("Seed_Dead_Count").AsInt + 1);

        //Set Seeds Quest active if Seeds count is less than 1
        if (DialogueLua.GetVariable("Seed_Dead_Count").AsInt >= 1)
        {
            m_seedsQuestStartDialogueSystemTrigger.OnUse();
        }

        //Set Seeds Quest as success when dead seeds is equal to total seeds and set Desecrate Statue Quest as active
        if (DialogueLua.GetVariable("Seed_Dead_Count").AsInt >= DialogueLua.GetVariable("Total_Seed_Instances").AsInt)
        {
            m_seedsQuestEndDialogueSystemTrigger.OnUse();
        }

        m_forceQuestUpdateHandle.SendQuestUpdate();
    }
}
