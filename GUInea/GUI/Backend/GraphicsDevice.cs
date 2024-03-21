using GUInea.GUI.Controls;

namespace GUInea.GUI.Backend
{
    public abstract class GraphicsDevice : IDisposable
    {
        public abstract void Dispose();

        internal abstract void RenderPoints(IEnumerable<ColorPoint> points);
    }
}
