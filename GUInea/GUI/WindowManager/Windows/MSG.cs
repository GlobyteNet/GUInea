using System.Runtime.InteropServices;

namespace GUInea.GUI.WindowManager.Windows
{
    [StructLayout(LayoutKind.Sequential)]
    struct MSG
    {
        public nint hwnd;
        public uint message;
        public nint wParam;
        public nint lParam;
        public uint time;
        public Point pt;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Point { public float X, Y; }
}
