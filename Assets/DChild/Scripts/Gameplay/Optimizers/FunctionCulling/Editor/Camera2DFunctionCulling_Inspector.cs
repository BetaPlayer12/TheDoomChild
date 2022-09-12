using DChild.Gameplay.Optimization;
using DChild.Gameplay.Optimization.Modules;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace DChild.Editor.Inspector
{
    [CustomEditor(typeof(Camera2DFunctionCulling))]
    public class Camera2DFunctionCulling_Inspector : OdinEditor
    {
        [DrawGizmo(GizmoType.NotInSelectionHierarchy)]
        private void OnSceneGUI()
        {
            var modules = (IFunctionCullingModule2D[])Tree.GetPropertyAtUnityPath("m_modules").ValueEntry.WeakSmartValue;
            for (int i = 0; i < modules.Length; i++)
            {
                modules[i].DrawHandles(target);
            }
        }
    }
}