using UnityEngine;

namespace Holysoft.UI
{
    public class SimpleUICanvas : UICanvas
    {
        private void OnValidate()
        {
            AssignComponents();
        }
    }
}