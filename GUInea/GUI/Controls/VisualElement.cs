namespace GUInea.GUI.Controls
{
    internal abstract class VisualElement
    {
        public abstract IEnumerable<ColorPoint> GetPoints();
    }

    internal class VisualRectangle : VisualElement
    {
        public override IEnumerable<ColorPoint> GetPoints()
        {
            yield return Rect.TopLeft;
            yield return Rect.TopRight;
            yield return Rect.BottomLeft;
            yield return Rect.BottomRight;
        }

        public ColorRect Rect;
    }

    internal struct ColorRect
    {
        public ColorPoint TopLeft, TopRight, BottomLeft, BottomRight;
    }

    internal struct Rect
    {
        public float X, Y, Width, Height;
    }

    internal struct ColorPoint
    {
        public Color Color;
        public Point Point;
    }

    internal struct Color
    {
        public float R, G, B, A;
    }

    internal struct Size
    {
        public float Width, Height;
    }

    internal struct Point
    {
        public float X, Y;
    }
}
