using DChild.Gameplay.Environment;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.QuestHints.ChestMapTracker
{
    public class ChestDatabaseInitiator : MonoBehaviour
    {
        [SerializeField]
        private DChild.Gameplay.Environment.Location _location;
        [SerializeField]
        private string _SceneNumber;
        [SerializeField]
        private DialogueDatabase _database;

        [SerializeField]
        private GameObject _AddSingleObject;

        [Button]
        private void GenerateChestsVariablesFromScene()
        {
            Template template = new Template();
            var chests = FindObjectsOfType<LootChest>();
            for (int i = 0; i < chests.Length; i++)
            {
                var chest = chests[i];
                var chestName = chest.gameObject.name;

                var itemName = (GenerateCategory(chestName) + chestName);
                Variable var = template.CreateVariable(template.GetNextVariableID(_database), (_location.ToString() + "/" + "Scene" + _SceneNumber + "/" + itemName), "false", FieldType.Boolean);
                if (_database.variables.Contains(var))
                {
                    _database.variables.Remove(var);
                }
                _database.variables.Add(var);
            }
            // https://www.pixelcrushers.com/forum/viewtopic.php?t=5556 this is the source for making this
        }

        [Button, ShowIf("_AddSingleObject")]
        private void AddSingleObject()
        {
            Template template = new Template();
            var chest = _AddSingleObject.name;
            var itemName = (GenerateCategory(chest) + chest);
            Variable var = template.CreateVariable(template.GetNextVariableID(_database), (_location.ToString() + "/" + "Scene" + _SceneNumber + "/" + itemName), "false", FieldType.Boolean);
            if (_database.variables.Contains(var))
            {
                _database.variables.Remove(var);
            }
            _database.variables.Add(var);
        }

        private string GenerateCategory(string chestName)
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
}
