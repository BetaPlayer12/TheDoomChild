using UnityEngine;

namespace Holysoft.UI
{
    public static class UIUtility
    {
        private struct Anchor
        {
            public Anchor(Vector2 min, Vector2 max) : this()
            {
                this.min = min;
                this.max = max;
            }

            public Anchor(float minX, float minY, float maxX, float maxY)
            {
                this.min = new Vector2(minX, minY);
                this.max = new Vector2(maxX, maxY);
            }

            public Vector2 min;
            public Vector2 max;
        }

        public static void CloseCanvas(UICanvas canvas)
        {
            canvas.Hide();
#if UNITY_EDITOR
            var components = canvas.GetComponents<UICanvasComponent>();
            for (int i = 0; i < (components?.Length ?? 0); i++)
            {
                components[i].ForceDisable();
            }
#endif
        }

        public static void OpenCanvas(UICanvas canvas)
        {
            canvas.Show();
#if UNITY_EDITOR
            var components = canvas.GetComponents<UICanvasComponent>();
            for (int i = 0; i < (components?.Length ?? 0); i++)
            {
                components[i].ForceEnable();
            }
#endif
        }

        public static void SetAnchor(RectTransform target, HorizontalAnchorType horizontalAnchorType, VerticalAnchorType verticalAnchorType)
        {
            var anchor = new Anchor();
            if (horizontalAnchorType == HorizontalAnchorType.Stretch)
            {
                anchor.min.x = 0;
                anchor.max.x = 1;
            }
            else
            {
                float x = ((int)horizontalAnchorType) * 0.5f;
                anchor.min.x = x;
                anchor.max.x = x;
            }

            if (verticalAnchorType == VerticalAnchorType.Stretch)
            {
                anchor.min.y = 0;
                anchor.max.y = 1;
            }
            else
            {
                float y = ((int)verticalAnchorType) * 0.5f;
                anchor.min.y = y;
                anchor.max.y = y;
            }

            target.anchorMin = anchor.min;
            target.anchorMax = anchor.max;
        }

        public static void SetPivot(RectTransform target, HorizontalAnchorType horizontalAnchorType, VerticalAnchorType verticalAnchorType)
        {
            var pivot = new Vector2();
            if (horizontalAnchorType == HorizontalAnchorType.Stretch)
            {
                pivot.x = 0.5f;
            }
            else
            {
                pivot.x = ((int)horizontalAnchorType) * 0.5f;
            }

            if (verticalAnchorType == VerticalAnchorType.Stretch)
            {
                pivot.y = 0.5f;
            }
            else
            {
                pivot.y = ((int)verticalAnchorType) * 0.5f;
            }

            target.pivot = pivot;
        }
    }
}