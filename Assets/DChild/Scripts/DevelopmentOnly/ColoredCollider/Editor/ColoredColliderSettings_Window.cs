using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace DChild.Configurations.Editor
{
    public class ColoredColliderSettings_Window : OdinEditorWindow
    {
        [MenuItem("Tools/DChild Utility/Colored Collider Settings")]
        private static void ShowWindow()
        {
            var window = GetWindow<ColoredColliderSettings_Window>(false, "Colored Collider Settings", true);
        }

        [SerializeField, InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
        public ColoredColliderSettings m_settings;


        protected override void Initialize()
        {
            m_settings = ColoredColliderSettings.instance;
        }
    }
}