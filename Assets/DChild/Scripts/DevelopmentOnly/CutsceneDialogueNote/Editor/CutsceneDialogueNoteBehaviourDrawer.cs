using PixelCrushers.DialogueSystem;
using UnityEditor;
using UnityEngine;

namespace DChildDebug.Cutscene
{
    [CustomPropertyDrawer(typeof(CutsceneDialogueNoteBehaviour))]
    public class CutsceneDialogueNoteBehaviourDrawer : PropertyDrawer
    {
        private DialogueEntryPicker entryPicker = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int fieldCount = 3;
            return fieldCount * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty conversationProp = property.FindPropertyRelative("conversation");
            SerializedProperty entryIDProp = property.FindPropertyRelative("entryID");
            SerializedProperty entryTextProp = property.FindPropertyRelative("entryText");

            Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(singleFieldRect, conversationProp);

            singleFieldRect.y += EditorGUIUtility.singleLineHeight;
            if (entryPicker == null)
            {
                entryPicker = new DialogueEntryPicker(conversationProp.stringValue);
            }
            if (entryPicker.isValid)
            {
                entryIDProp.intValue = entryPicker.Draw(singleFieldRect, "Entry ID", entryIDProp.intValue);
                entryTextProp.stringValue = entryPicker.GetDialogue(entryIDProp.intValue);
            }
            else
            {
                EditorGUI.PropertyField(singleFieldRect, entryIDProp);
            }
        }
    }
}
