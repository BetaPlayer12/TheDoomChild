using Holysoft.Event;

namespace DChild.Menu.Inputs
{
    public class InputIconChangeEventArgs : IEventActionArgs
    {
        private GamepadIconData m_iconData;

        public GamepadIconData iconData => m_iconData;

        public void Set(GamepadIconData iconData,int indexInput) => m_iconData = iconData;
    }
}