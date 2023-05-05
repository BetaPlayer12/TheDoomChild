using DChild.Gameplay.Combat;
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
    [SerializeField,VariablePopup(true)]
    private string m_connectedVariable;
    [SerializeField]
    private Flag instanceTracker;
    [SerializeField] 
    private DialogueSystemTrigger m_dialogueSystemTrigger;
    [SerializeField]
    private DialogueSystemTrigger m_questEndDialogueSystemTrigger;

    [SerializeField]
    private List<Damageable> m_SeedsOfTheOne;

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

            m_SeedsOfTheOne[i].DamageTaken += OnSeedDies;
        }

        DialogueLua.SetVariable(m_connectedVariable, (int)instanceTracker);
    }

    private void OnSeedDies(object sender, Damageable.DamageEventArgs eventArgs)
    {
        for (int i = 0; i < m_SeedsOfTheOne.Count; i++)
        {
            if (!m_SeedsOfTheOne[i].isAlive)
            {
                AddSeedFlag();
            }
        }
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

        if(DialogueLua.GetVariable("Seed_Dead_Count").AsInt < 1)
        {
            m_dialogueSystemTrigger.OnUse();
        }

        if(DialogueLua.GetVariable("Seed_Dead_Count").AsInt >= DialogueLua.GetVariable("Total_Seed_Instances").AsInt)
        {
            m_questEndDialogueSystemTrigger.OnUse();
        }

        DialogueLua.SetVariable(m_connectedVariable, (int)instanceTracker);
        DialogueLua.SetVariable("Seed_Dead_Count", DialogueLua.GetVariable("Seed_Dead_Count").AsInt + 1);
    }
}
