using PixelCrushers.DialogueSystem;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace DChildDebug.Cutscene
{
    [CustomPropertyDrawer(typeof(DialogueBehaviour))]
    public class DialogueBehaviourDrawer : OdinValueDrawer<DialogueBehaviour>
    {
        private DialogueEntryPicker entryPicker = null;
        private DialogueEntryPicker noteEntryPicker = null;
        private string entryPickerLastConvesationString;
        private string noteEntryPickerLastConvesationString;

        protected override void DrawPropertyLayout(GUIContent label)
        {

            var children = ValueEntry.Property.Children;
            var dialougeDatabase = (DialogueDatabase)children.Get("m_referenceDatabase").ValueEntry.WeakSmartValue;
            for (int i = 0; i < children.Count; i++)
            {
                var child = children.Get(i);
                if (child.Name == "entryID")
                {
                    var jumpToEntry = children.Get("jumpToSpecificEntry");
                    if ((bool)jumpToEntry.ValueEntry.WeakSmartValue)
                    {
                        if (entryPicker == null)
                        {
                            var conversation = children.Get("conversation");
                            var conversationString = (string)conversation.ValueEntry.WeakSmartValue;
                            UpdateDialogueEntryPicker(ref entryPicker, ref entryPickerLastConvesationString, dialougeDatabase, conversationString);
                        }
                        else
                        {
                            var conversation = children.Get("conversation");
                            var conversationString = (string)conversation.ValueEntry.WeakSmartValue;
                            if (entryPickerLastConvesationString != conversationString)
                            {
                                UpdateDialogueEntryPicker(ref entryPicker, ref entryPickerLastConvesationString, dialougeDatabase, conversationString);
                            }
                        }
                        if (entryPicker.isValid)
                        {
                            child.Draw();
                            var valueEntry = child.ValueEntry;
                            valueEntry.WeakSmartValue = entryPicker.Draw(child.LastDrawnValueRect, "Entry ID", (int)valueEntry.WeakSmartValue);
                        }
                        else
                        {
                            child.Draw();
                        }
                    }
                }
                else if(child.Name == "noteEntryID")
                {
                    if (noteEntryPicker == null)
                    {
                        var conversation = children.Get("noteConversation");
                        var conversationString = (string)conversation.ValueEntry.WeakSmartValue;
                        UpdateDialogueEntryPicker(ref noteEntryPicker, ref noteEntryPickerLastConvesationString, dialougeDatabase, conversationString);
                    }
                    else
                    {
                        var conversation = children.Get("noteConversation");
                        var conversationString = (string)conversation.ValueEntry.WeakSmartValue;
                        UpdateDialogueEntryPicker(ref noteEntryPicker, ref noteEntryPickerLastConvesationString, dialougeDatabase, conversationString);
                    }
                    if (noteEntryPicker.isValid)
                    {
                        child.Draw();
                        var valueEntry = child.ValueEntry;
                        valueEntry.WeakSmartValue = noteEntryPicker.Draw(child.LastDrawnValueRect, "Note Entry ID", (int)valueEntry.WeakSmartValue);
                        var entryText = children.Get("noteEntryText");
                        entryText.ValueEntry.WeakSmartValue = noteEntryPicker.GetDialogue((int)valueEntry.WeakSmartValue);
                    }
                    else
                    {
                        child.Draw();
                    }
                }
                else
                {
                    child.Draw();
                }

            }
        }

        private void UpdateDialogueEntryPicker(ref DialogueEntryPicker entryPicker, ref string lastConversationString, DialogueDatabase dialougeDatabase, string conversationString)
        {
            lastConversationString = conversationString;
            entryPicker = new DialogueEntryPicker(conversationString);
            entryPicker.SetDialogueEntries(dialougeDatabase, conversationString);
        }
    }
}
