using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Add this to the Dialogue Manager. It adds a Lua function that you can use in your
/// dialogue entries' Script fields:
/// 
/// ChangeActorName(actorName, newDisplayName)
/// </summary>
public class ChangeActorNameLua : MonoBehaviour
{

    void OnEnable()
    {
        // Make the function available to Lua:
        Lua.RegisterFunction("ChangeActorName", this, SymbolExtensions.GetMethodInfo(() => ChangeActorName(string.Empty, string.Empty)));
        Lua.RegisterFunction("UseActorRelevantName", this, SymbolExtensions.GetMethodInfo(() => UseActorRelevantName(string.Empty, string.Empty)));
        Lua.RegisterFunction("HideActorName", this, SymbolExtensions.GetMethodInfo(() => HideActorName(string.Empty)));
        Lua.RegisterFunction("UseActorAlias", this, SymbolExtensions.GetMethodInfo(() => UseActorAlias(string.Empty)));
        Lua.RegisterFunction("UseActorKnownName", this, SymbolExtensions.GetMethodInfo(() => UseActorKnownName(string.Empty)));
    }

    void OnDisable()
    {
        // Remove the function from Lua:
        Lua.UnregisterFunction("ChangeActorName");
        Lua.UnregisterFunction("UseActorRelevantName");
        Lua.UnregisterFunction("UseActorAlias");
        Lua.UnregisterFunction("HideActorName");
        Lua.UnregisterFunction("UseActorKnownName");
    }


    public void ChangeActorName(string actorName, string newDisplayName)
    {
        if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: Changing " + actorName + "'s Display Name to " + newDisplayName);
        DialogueLua.SetActorField(actorName, "Display Name", newDisplayName);
        if (DialogueManager.IsConversationActive)
        {
            var actor = DialogueManager.MasterDatabase.GetActor(actorName);
            if (actor != null)
            {
                var info = DialogueManager.ConversationModel.GetCharacterInfo(actor.id);
                if (info != null) info.Name = newDisplayName;
            }
        }
    }
    public void HideActorName(string actorName)
    {
        ChangeActorName(actorName, "???");
    }

    public void UseActorAlias(string actorName)
    {
        ChangeActorName(actorName, DialogueLua.GetActorField(actorName, "Alias").asString);
    }

    public void UseActorKnownName(string actorName)
    {
        ChangeActorName(actorName, DialogueLua.GetActorField(actorName, "KnownName").asString);
    }

    public void UseActorRelevantName(string actorName, string conditionVariable)
    {
        var conditionVariableResult = DialogueLua.GetVariable(conditionVariable).asBool;
        var displayName = conditionVariableResult ? DialogueLua.GetActorField(actorName, "KnownName").asString : DialogueLua.GetActorField(actorName, "Alias").asString;
        ChangeActorName(actorName, displayName);
    }
}
