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
                            entryPicker = new DialogueEntryPicker(conversationString);
                            entryPicker.SetDialogueEntries(dialougeDatabase, conversationString);
                        }
                        else
                        {
                            var conversation = children.Get("conversation");
                            var conversationString = (string)conversation.ValueEntry.WeakSmartValue;
                            entryPicker.SetDialogueEntries(dialougeDatabase, conversationString);
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
                        noteEntryPicker = new DialogueEntryPicker(conversationString);
                        noteEntryPicker.SetDialogueEntries(dialougeDatabase, conversationString);
                    }
                    else
                    {
                        var conversation = children.Get("noteConversation");
                        var conversationString = (string)conversation.ValueEntry.WeakSmartValue;
                        noteEntryPicker.SetDialogueEntries(dialougeDatabase, conversationString);
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
    }
}
