using DChild.Gameplay.Environment;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.QuestHints.DoorMapTracker
{
    public class DoorDatabaseInitiator : MonoBehaviour
    {
        [SerializeField]
        private DChild.Gameplay.Environment.Location _location;
        [SerializeField]
        private string _SceneNumber;
        [SerializeField]
        private DialogueDatabase _database;

        [SerializeField]
        private string _Tag;

        [SerializeField]
        private GameObject _AddSingleObject;
        [SerializeField, ShowIf("_AddSingleObject")]
        private string _Added_ObjectName = "Door4";

        [SerializeField]
        private int _NumberOfDoors;

        [Button]
        private void GenerateChestsVariablesFromScene()
        {
            Template template = new Template();
            for (int i = 1; i <= _NumberOfDoors; i++)
            {
                //var itemName = "Door";
                Variable var = template.CreateVariable(template.GetNextVariableID(_database), (_location.ToString() + "/" + "Scene" + _SceneNumber + "/" + _Tag + "_"+i), "false", FieldType.Boolean);
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
            Variable var = template.CreateVariable(template.GetNextVariableID(_database), (_location.ToString() + "/" + "Scene" + _SceneNumber + "/" + _Added_ObjectName), "false", FieldType.Boolean);
            if (_database.variables.Contains(var))
            {
                _database.variables.Remove(var);
            }
            _database.variables.Add(var);
        }
    }
}
