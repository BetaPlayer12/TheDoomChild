using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment.Interractables;
using Holysoft.Event;
using DChild.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleEncounterer : MonoBehaviour , IButtonToInteract , ISerializableComponent
    {
        [SerializeField,TabGroup("Initialize")]
        private ArmyBattleScenario m_Scenario;
        [SerializeField, TabGroup("Reference")]
        private SpriteRenderer m_SpriteRenderer;
        [SerializeField, TabGroup("Reference")]
        private bool m_Repeatable;
        [SerializeField, TabGroup("Initialize")]
        private Sprite m_Appearance;
        [HideInInspector]
        private bool m_IsDefeated;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        public bool showPrompt => throw new System.NotImplementedException();

        public string promptMessage => throw new System.NotImplementedException();

        public Vector3 promptPosition => throw new System.NotImplementedException();

        private void Awake()
        {
            m_SpriteRenderer.sprite = m_Appearance;
        }
        
        [Button]
        public void InitiateEncounter()
        {

            ArmyBattleSystem.BattleScenario = m_Scenario;
            Debug.Log("ARMY BATTLE SCENARIO INITIATED :"+ArmyBattleSystem.BattleScenario.name);
        }
        /*
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
            Debug.Log(playerObject);
            if (collision.tag != "Sensor")
            {
                InitiateEncounter();
                if(!Repeatable)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        */
        [Button]
        public void DefeatArmy()
        {
            Debug.Log(name + " Is Defeated");
            Destroy(this.gameObject);
        }
        public void Interact(Character character)
        {
            InitiateEncounter();
        }

        public ISaveData Save()
        {
            throw new System.NotImplementedException();
        }

        public void Load(ISaveData data)
        {
            throw new System.NotImplementedException();
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }
    }
}
