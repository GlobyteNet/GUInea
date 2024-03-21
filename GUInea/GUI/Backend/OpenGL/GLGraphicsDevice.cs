using GUInea.GUI.Controls;
using GUInea.GUI.WindowManager;

namespace GUInea.GUI.Backend.OpenGL
{
    internal class GLGraphicsDevice : GraphicsDevice
    {
        public GLGraphicsDevice(Window window)
        {
            wgl = new WGL(window);
        }

        public override void Dispose()
        {
            wgl.Dispose();
        }

        internal override void RenderPoints(IEnumerable<ColorPoint> points)
        {
            if (vao == 0)
            {
                vao = GL.GenVertexArray();
                GL.BindVertexArray(vao);
            }

            if (vbo == 0)
            {
                vbo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            }

            if (ibo == 0)
            {
                ibo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            }

            if (points.Count() > 0)
            {
                var data = points.SelectMany(p => new[] { p.Point.X, p.Point.Y, p.Color.R, p.Color.G, p.Color.B, p.Color.A }).ToArray();
                GL.BufferData(BufferTarget.ArrayBuffer, data, BufferUsage.StaticDraw);
            }

        }

        private WGL wgl;

        private uint vao;
        private uint vbo;
        private uint ibo;
    }
}
