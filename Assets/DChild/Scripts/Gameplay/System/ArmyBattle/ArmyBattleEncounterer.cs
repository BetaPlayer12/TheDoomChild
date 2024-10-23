using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment.Interractables;
using Holysoft.Event;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleEncounterer : MonoBehaviour , IButtonToInteract
    {
        [SerializeField,TabGroup("Initialize")]
        private ArmyBattleScenario Scenario;
        [SerializeField, TabGroup("Reference")]
        private SpriteRenderer SpriteRenderer;
        [SerializeField, TabGroup("Reference")]
        private bool Repeatable;
        [SerializeField, TabGroup("Initialize")]
        private Sprite Appearance;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        public bool showPrompt => throw new System.NotImplementedException();

        public string promptMessage => throw new System.NotImplementedException();

        public Vector3 promptPosition => throw new System.NotImplementedException();

        private void Awake()
        {
            SpriteRenderer.sprite = Appearance;
        }
        
        public void InitiateEncounter()
        {
            ArmyBattleSystem.BattleScenario = Scenario;
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
        public void Interact(Character character)
        {
            InitiateEncounter();
        }
    }
}
