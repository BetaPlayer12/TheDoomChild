using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Menu.Codex
{
    public abstract class CodexIndexInfoUI<InfoType> : MonoBehaviour
    {
        public abstract void SetInfo(InfoType data);

        public abstract void SetAsNewInfo(bool isNew);

#if UNITY_EDITOR
        [ResponsiveButtonGroup, Button]
        private void MarkAsNew()
        {
            SetAsNewInfo(true);
        }
#endif
    }
}