using DChild.Configurations.Editor;
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

        [Button]
        public void PopulateDatabaseVariables(DialogueDatabase database)
        {
            Dictionary<Location, string> locationSuffixPair = CollecathonUtility.AccessLocationDictionary();

            List<string> variablesToAdd = new List<string>();

            foreach (var item in locationSuffixPair.Keys)
            {
                for (int i = 0; i < (int)CollectathonTypes._COUNT; i++)
                {
                    //Create Variable names
                    var collecatonType = (CollectathonTypes)i;
                    string count = CollecathonUtility.GenerateCurrentCountVariableName(collecatonType,item);
                    string total = CollecathonUtility.GenerateCurrentTotalVariableName(collecatonType,item);

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

        [Button]
        public void RemoveAllVariables(DialogueDatabase database)
        {
            database.variables.Clear();
        }
    }
}
