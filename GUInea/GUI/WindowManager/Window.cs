using GUInea.GUI.WindowManager.Windows;
using System.Runtime.InteropServices;

namespace GUInea.GUI.WindowManager
{
    public abstract class Window : IDisposable
    {
        public static Window Create()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsWindow();
            }
            else
            {
                throw new PlatformNotSupportedException("Windows only");
            }
        }

        public abstract void Show();

        /// <summary>
        /// Polls for window events.
        /// </summary>
        public abstract void PollEvents();

        public abstract void SwapBuffers();

        public abstract void Dispose();

        public abstract bool IsVisible { get; }
    }
}
