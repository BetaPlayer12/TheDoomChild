using DChild.Configurations.Editor;
using DChild.UI;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Location = DChild.Gameplay.Environment.Location;

namespace DChild.UI
{
    public class CollectathonDatabasePopulator_Window : OdinEditorWindow
    {

        [MenuItem("Tools/DChild Utility/Collectathon Database Populator")]
        private static void ShowWindow()
        {
            var window = GetWindow<CollectathonDatabasePopulator_Window>(false, "Collectathon Database Populator", true);
        }

        [SerializeField]
        private DialogueDatabase m_collectathonDatabase;

        [Button]
        public void PopulateDatabaseVariables(DialogueDatabase database)
        {
            Dictionary<Location, string> locationSuffixPair = new Dictionary<Location, string>();
            locationSuffixPair.Add(Location.City_Of_The_Dead, "COTD");
            locationSuffixPair.Add(Location.Graveyard, "GY");
            locationSuffixPair.Add(Location.Unholy_Forest, "UF");
            locationSuffixPair.Add(Location.Garden, "GD");
            locationSuffixPair.Add(Location.Laboratory, "LAB");
            locationSuffixPair.Add(Location.Library, "LIB");
            locationSuffixPair.Add(Location.Prison, "PR");
            locationSuffixPair.Add(Location.Throne_Room, "TR");
            locationSuffixPair.Add(Location.Realm_Of_Nightmare, "RON");
            locationSuffixPair.Add(Location.Temple_Of_The_One, "TOTO");

            List<string> variablesToAdd = new List<string>();

            foreach (var item in locationSuffixPair.Keys)
            {
                for (int i = 0; i < (int)CollectathonTypes._COUNT; i++)
                {
                    //Create Variable names
                    var collecatonType = (CollectathonTypes)i;
                    string total = $"Collecathon_{collecatonType}_Total_{locationSuffixPair[item]}";
                    string count = $"Collecathon_{collecatonType}_Count_{locationSuffixPair[item]}";

                    variablesToAdd.Add(total);
                    variablesToAdd.Add(count);
                }
            }

            Template template = TemplateTools.LoadFromEditorPrefs();

            foreach (string variableName in variablesToAdd)
            {
                Variable variable = template.CreateVariable(template.GetNextVariableID(database), variableName, "0", FieldType.Number);
                database.variables.Add(variable);
            }
            EditorUtility.SetDirty(database);
        }
    }
}
