using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace DChild
{
    public class DialogueLuaScriptHandle : MonoBehaviour
    {
        [SerializeField,LuaScriptWizard(true)]
        private string m_variable;

        public void RunScript()
        {
            Lua.Run(m_variable);
        }
    }
}