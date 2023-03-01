using DChild.Menu.Bestiary;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor; 
#endif
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChildEditor
{
    public class BestiaryEditor : OdinEditorWindow
    {
        [MenuItem("Component/DChild/Bestiary Editor")]
        private static void OpenWindow()
        {
            GetWindow<BestiaryEditor>().Show();
        }

        [TableList(ShowPaging = true, NumberOfItemsPerPage = 10)]
        public List<BestiaryEditorTable> TableList;

        [Button]
        private void PopulateTable()
        {
            string[] results;
            TableList.Clear();

            results = AssetDatabase.FindAssets("t:BestiaryData");

            foreach (string guid in results)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BestiaryEditorTable rowData = new BestiaryEditorTable();
                BestiaryData data = AssetDatabase.LoadAssetAtPath<BestiaryData>(path);
                rowData.Data = data;
                rowData.InfoImage = data.infoImage;
                rowData.DisplayName = data.creatureName;
                rowData.Description = data.description;
                TableList.Add(rowData);
            }

            TableList.Sort((x, y) => x.DisplayName.CompareTo(y.DisplayName));
        }
    }

    [Serializable]
    public class BestiaryEditorTable
    {
        [TableColumnWidth(57, Resizable = true)]
        public BestiaryData Data;

        [TableColumnWidth(40, Resizable = true)]
        public Sprite InfoImage;

        [TableColumnWidth(40, Resizable = true)]
        public string DisplayName;

        [TextArea(1, 10)]
        public string Description;

        [TableColumnWidth(20)]
        [Button(name:"Update")]
        public void Action()
        {
            Data.SetInfoImage(InfoImage);
            Data.SetDisplayName(DisplayName);
            Data.SetDesciption(Description);
            Debug.Log("Bestiary Data Updated.");
        }
    }
}