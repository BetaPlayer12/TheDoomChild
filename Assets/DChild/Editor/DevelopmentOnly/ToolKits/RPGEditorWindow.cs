using Sirenix.OdinInspector.Editor;
using UnityEditor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using System.Linq;
using DChild.Gameplay.Items;

namespace DChildEditor.Toolkit
{
    public class RPGEditorWindow : OdinMenuEditorWindow
    {
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

            var items = AssetDatabase.FindAssets("t:ItemData")
    .Select(guid => AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(guid)))
    .ToArray();

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                tree.Add($"Item/{item.category}/{item.name.Replace("Data","")}", item, item.icon);
            }



            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();
            //Create create Item Button
        }
    }
}