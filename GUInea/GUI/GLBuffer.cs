using GUInea.GUI.Backend.OpenGL;

namespace GUInea.GUI
{
    internal class GLBuffer<T> : IDisposable
        where T : unmanaged
    {
        public readonly uint Id;

        public GLBuffer()
        {
            Id = GL.GenBuffer();
        }

        public void BufferData(T[] data, BufferTarget target, BufferUsage usageHint)
        {
            GL.BindBuffer(target, Id);

            GL.BufferData(target, data, usageHint);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(Id);
        }
    }
}
