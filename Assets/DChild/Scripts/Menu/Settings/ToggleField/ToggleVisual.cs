using Holysoft.Event;
using UnityEngine;

namespace DChild.Menu.UI
{
    //[ExecuteInEditMode]
    [RequireComponent(typeof(ToggleButton))]
    public abstract class ToggleVisual : MonoBehaviour
    {
        protected abstract void OnStateUpdate(object sender, ButtonToggledEventArgs eventArgs);

        private void OnEnable()
        {
            GetComponent<ToggleButton>().StateUpdate += OnStateUpdate;
        }

        private void OnDisable()
        {
            GetComponent<ToggleButton>().StateUpdate -= OnStateUpdate;
        }
    }

}