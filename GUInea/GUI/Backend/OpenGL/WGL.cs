﻿using GUInea.GUI.Backend;
using GUInea.GUI.Controls;
using GUInea.GUI.WindowManager;
using GUInea.GUI.WindowManager.Windows;
using System.Runtime.InteropServices;

namespace GUInea.GUI.Backend.OpenGL
{
    internal class WGL : IDisposable
    {
        public WGL(Window window)
        {
            this.window = window as WindowsWindow;

            if (this.window == null)
            {
                throw new PlatformNotSupportedException("Windows only");
            }

            var hdc = this.window.hdc;

            hglrc = wglCreateContext(hdc);
            wglMakeCurrent(hdc, hglrc);

            // This function should be called after creating the OpenGL context
            wglSwapIntervalEXT = Marshal.GetDelegateForFunctionPointer
                <wglSwapInterval>(wglGetProcAddress("wglSwapIntervalEXT"));

            wglSwapIntervalEXT(1);
        }

        public void Dispose()
        {
            wglMakeCurrent(window.hdc, IntPtr.Zero);
            wglDeleteContext(hglrc);
        }

        /// <summary>
        /// The handle to the OpenGL rendering context.
        /// </summary>
        private nint hglrc;

        private WindowsWindow window;

        [DllImport("opengl32.dll", SetLastError = true)]
        static extern nint wglCreateContext(nint hdc);

        [DllImport("opengl32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool wglMakeCurrent(nint hdc, nint hglrc);

        [DllImport("opengl32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool wglDeleteContext(nint hglrc);

        [DllImport("opengl32.dll", SetLastError = true)]
        static extern nint wglGetProcAddress(string lpszProc);

        delegate void wglSwapInterval(int interval);
        wglSwapInterval wglSwapIntervalEXT;
    }
}
