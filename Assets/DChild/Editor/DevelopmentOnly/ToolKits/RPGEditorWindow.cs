using Sirenix.OdinInspector.Editor;
using UnityEditor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using System.Linq;
using DChild.Gameplay.Items;
using UnityEngine;
using DChildEditor.ThirdParty;
using DChild.Gameplay.Characters.NPC;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using System;
using DChild.Menu.Bestiary;
using DChild.Gameplay.Combat.StatusAilment;

namespace DChildEditor.Toolkit
{
    public class RPGEditorWindow : OdinMenuEditorWindow
    {
        private const string SCRIPT_ICONS = "Assets/DChild/Objects/DevelopementOnly/Editor/Resources/";

        [MenuItem("Tools/Kit/RPG Editor")]
        private static void Open()
        {
            var window = GetWindow<RPGEditorWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;

            //Items
            AddGenericItemList<ItemData>("t:ItemData", (item) =>
            {
                tree.Add($"Item/{item.category}/{item.name.Replace("Data", "")}", item, item.icon);
            });

            //NPCS
            AddGenericItemList<NPCProfile>("t:NPCProfile", (npc) =>
            {
                tree.Add($"Characters/NPC/{npc.name.Replace("Data", "")}", npc, npc.baseIcon);
            });


            List<string> statusEffectNames = new List<string>();
            AddGenericItemList<StatusEffectData>("t:StatusEffect", (statusEffect) =>
            {
                var statusEffectName = statusEffect.name;
                string result = " ";

                if (statusEffectNames.Contains(statusEffectName) == true)
                {
                    if (statusEffectName == statusEffect.name)
                    {
                        result = $"{statusEffect.name + (1)}";
                        statusEffectNames.Add(result);
                    }
                }

                if (statusEffectNames.Contains(statusEffectName)== false)
                {
                    statusEffectNames.Add(statusEffectName);
                }

           
                
                tree.Add($"StatusEffect/{statusEffect.type}/{result}", statusEffect);
            });

            //Enemies
            #region Enemies
            List<string> bossNames = new List<string>();
            Sprite phaseIcon = AssetDatabase.LoadAssetAtPath<Sprite>(SCRIPT_ICONS + "BossPhase.png");
            AddGenericItemList<BossPhaseData>("t:BossPhaseData", (data) =>
            {
                var bossName = data.name.Split(new[] { "Phase" }, System.StringSplitOptions.None)[0];
                tree.Add($"Characters/Boss/{bossName}/{data.name.Replace("Data", "").Replace("Info", "")}", data, phaseIcon);
                if (bossNames.Contains(bossName) == false)
                {
                    bossNames.Add(bossName);
                }
            });

            List<string> minionName = new List<string>();
            Sprite AIIcon = AssetDatabase.LoadAssetAtPath<Sprite>(SCRIPT_ICONS + "AIICon.png");
            AddGenericItemList<AIData>("t:AIData", (data) =>
            {
                string result = "";
                if (FindStartingString(data.name, bossNames, out result) == false)
                {
                    if (FindStartingString(data.name, minionName, out result) == false)
                    {
                        result = data.name.Split(new[] { "AI" }, System.StringSplitOptions.None)[0];
                        minionName.Add(result);
                    }
                    tree.Add($"Characters/Minion/{result}/{data.name.Replace("Data", "").Replace("Info", "")}", data, AIIcon);
                }
                else
                {
                    tree.Add($"Characters/Boss/{result}/{data.name.Replace("Data", "").Replace("Info", "")}", data, AIIcon);
                }
            });

            AddGenericItemList<BestiaryData>("t:BestiaryData", (data) =>
            {
                string result = "";
                if (FindStartingString(data.name, bossNames, out result) == false)
                {
                    if (FindStartingString(data.name, minionName, out result) == false)
                    {
                        tree.Add($"Characters/No AI/{result}/{data.name.Replace("Data", "").Replace("Info", "")}", data, data.infoImage);
                    }
                    else
                    {
                        tree.Add($"Characters/Minion/{result}/{data.name.Replace("Data", "").Replace("Info", "")}", data, data.infoImage);
                    }
                }
                else
                {
                    tree.Add($"Characters/Boss/{result}/{data.name.Replace("Data", "").Replace("Info", "")}", data, data.infoImage);
                }
            });
            #endregion


            //loot
            var loots = new TrialLootKit();
            loots.PopulateList();
            tree.Add($"Loot Data Editor/Minion Loot", loots);

            var breakableLoots = new TrialLootKit();
            breakableLoots.PopulateBreakableList();
            tree.Add("Loot Data Editor/Breakable Objects Loot", breakableLoots);

            var bossLoot = new TrialLootKit();
           bossLoot.PopulateBossLootList();
            tree.Add("Loot Data Editor/Boss Loot", bossLoot);

            //Character Stats
            var characterStats = new TrialStatsKit();
            characterStats.PopulateList();
            tree.Add($"Enemy Stats Data Editor/Minion Stats", characterStats);

            //Character Stats
            var bossStats = new TrialBossKit();
            bossStats.PopulateList();
            tree.Add($"Enemy Stats Data Editor/Boss Stats", bossStats);

           

            tree.EnumerateTree().SortMenuItemsByName(true);

            return tree;

            void AddGenericItemList<T>(string filter, Action<T> action) where T : UnityEngine.Object
            {
                var objects = AssetDatabase.FindAssets(filter)
  .Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)))
  .ToArray();

                for (int i = 0; i < objects.Length; i++)
                {
                    action(objects[i]);
                }
            }

            bool FindStartingString(string value, List<string> list, out string result)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var current = list[i];
                    if (value.StartsWith(current))
                    {
                        result = current;
                        return true;
                    }
                }
                result = null;
                return false;
            }
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Item")))
                {
                    ObjectCreator.ShowScriptableObjectDialog<ItemData>("Assets/DChlld/Objects/Misc/ItemData", obj =>
                    {
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}