using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    public class NavMapPOISerializationConfigurator : MonoBehaviour
    {
        [System.Serializable]
        public struct ScenePOIInfo
        {
            [SerializeField, MinValue(0)]
            public int m_sceneIndex;
            [SerializeField, MinValue(0)]
            public int m_itemCount;
        }

        [SerializeField]
        public enum POIItems
        {
            savepoint,
            obstacle,
            realmgate,
        }

        [SerializeField]
        private DialogueDatabase m_database;
        [SerializeField, HorizontalGroup("NamePrefix")]
        private Environment.Location m_location;
        [SerializeField, TableList(AlwaysExpanded = true)]
        private ScenePOIInfo[] m_info;
        [SerializeField]
        private POIItems m_poiItems;


        public ScenePOIInfo[] sceneInfo => m_info;
        public Environment.Location location => m_location;
        public DialogueDatabase database => m_database;

        [Button]
        public void SerializeToDatabase()
        {
            for (int y = 0; y < m_database.variables.Count; y++)
            {
                if (m_database.variables[y].fields[0].value.Contains("POI"))
                {
                    if (m_database.variables[y].fields[0].value.Contains($"{m_poiItems}"))
                    {
                        m_database.variables.Remove(m_database.variables[y]);
                        y--;
                    }
                    else
                    {

                    }


                }
                else
                {

                }
            }


            var template = Template.FromDefault();
            for (int k = 0; k < m_info.Length; k++)
            {
                var info = m_info[k];
                for (int i = 0; i < info.m_itemCount; i++)
                {
                    var varID = template.GetNextVariableID(m_database);
                    var variable = template.CreateVariable(varID, CreateVariableName(info.m_sceneIndex, i), "false", FieldType.Boolean);
                    m_database.variables.Add(variable);
                }
            }

        }


        private string CreateVariableName(int sceneIndex, int index) => $"{m_location}_{sceneIndex}_POI_{m_poiItems}_{index}";

    }
}

