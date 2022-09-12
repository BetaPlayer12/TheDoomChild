using DChild.Menu.Bestiary;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BestiaryDataParser : Editor
{
    [SerializeField]
    private TextAsset m_csvFile;
    [SerializeField]
    private List<BestiaryData> m_bestiaryDataList = new List<BestiaryData>();

    [Button]
    public void ReadFromcsv()
    {
        string[] readData = m_csvFile.text.Split(new string[] { ",", "\n" },StringSplitOptions.None);
        var bestiaryDbList = new List<BestiaryData>(AssetDatabase.FindAssets("t:BestiaryData").Select(guid => AssetDatabase.LoadAssetAtPath<BestiaryData>(AssetDatabase.GUIDToAssetPath(guid))));
        int tableSize = readData.Length / 7 - 1;

        for(int x = 0; x < bestiaryDbList.Count; x++)
        {
            var currentData = bestiaryDbList[x];
            for (int i = 0; i < tableSize; i++)
            {
                if(currentData.projectName == readData[7 * (i + 1)])
                {
                    bestiaryDbList[x].SetDisplayName(readData[7 * (i + 1) + 1]);
                    if(readData[7 * (i + 1) + 1] != " ")
                    {
                        bestiaryDbList[x].UseDisplayName(true);
                    }
                    bestiaryDbList[x].SetTitle(readData[7 * (i + 1) + 2]);
                    bestiaryDbList[x].SetDesciption(readData[7 * (i + 1) + 4]);
                    bestiaryDbList[x].SetStoreNotes(readData[7 * (i + 1) + 5]);
                    bestiaryDbList[x].SetHuntersNotes(readData[7 * (i + 1) + 6]);
                    EditorUtility.SetDirty(bestiaryDbList[x]);
                    AssetDatabase.SaveAssets();

                }
                Debug.Log(readData[7 * (i + 1)]);
                Debug.Log(readData[7 * (i + 1) + 1]);
                Debug.Log(readData[7 * (i + 1) + 2]);
                Debug.Log(readData[7 * (i + 1) + 3]);
                Debug.Log(readData[7 * (i + 1) + 4]);
                Debug.Log(readData[7 * (i + 1) + 5]);
                Debug.Log(readData[7 * (i + 1) + 6]);
            }
        }


    }

    [Button]
    public void PopulateCSV()
    {
        var bestiaryDbList = new List<BestiaryData>(AssetDatabase.FindAssets("t:BestiaryData").Select(guid => AssetDatabase.LoadAssetAtPath<BestiaryData>(AssetDatabase.GUIDToAssetPath(guid))));
        string[] readData = m_csvFile.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        int tableSize = readData.Length / 7 - 1;
        StreamWriter writer = new StreamWriter(AssetDatabase.GetAssetPath(m_csvFile), true);

        for (int x = 0; x < bestiaryDbList.Count; x++)
        {
            var currentData = bestiaryDbList[x];
            //for(int y = 0; y < tableSize; y++)
            //{
            //    if(currentData.projectName == readData[7 * (y + 1)])
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            if(currentData.projectName == "")
            {
                writer.WriteLine($"{currentData.creatureName},{currentData.creatureName},{currentData.title},{currentData.storeNotes},{currentData.hunterNotes}");
            }
            else
            {
                writer.WriteLine($"{currentData.projectName},{currentData.creatureName},{currentData.storeNotes},{currentData.hunterNotes}");
            }
           
            
        }
        writer.Close();
    }
}
