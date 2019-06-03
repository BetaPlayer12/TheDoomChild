namespace Holysoft.UI
{
    public class UIElement3D : UIElement
    {
        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }
    }
}