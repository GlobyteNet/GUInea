using GUInea.GUI.Backend.OpenGL;
using GUInea.GUI.WindowManager;
using System.Diagnostics;
using System.Text;

namespace GUInea.GUI
{
    class OpenGLProgram
    {
        [STAThread]
        public static void Run()
        {
            var window = Window.Create();

            GL.Viewport(100, 100, 800, 600);

            window.Show();

            uint vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, 1, vertexShaderSource, null);
            GL.CompileShader(vertexShader);

            bool success;
            GL.GetShaderiv(vertexShader, ParameterName.CompileStatus, out success);
            if (!success)
            {
                char[] infoLog = new char[512];
                GL.GetShaderInfoLog(vertexShader, 512, out _, infoLog);
                Console.WriteLine("ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" + infoLog);
            }

            uint fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, 1, fragmentShaderSource, null);
            GL.CompileShader(fragmentShader);

            GL.GetShaderiv(fragmentShader, ParameterName.CompileStatus, out success);
            if (!success)
            {
                char[] infoLog = new char[512];
                GL.GetShaderInfoLog(fragmentShader, 512, out _, infoLog);
                Console.WriteLine("ERROR::SHADER::FRAGMENT::COMPILATION_FAILED\n" + infoLog);
            }

            uint shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);

            GL.GetProgramiv(shaderProgram, ParameterName.LinkStatus, out success);
            if (!success)
            {
                byte[] infoLog = new byte[512];
                GL.GetProgramInfoLog(shaderProgram, 512, out _, infoLog);
                string infoLogStr = Encoding.ASCII.GetString(infoLog);
                Console.WriteLine("ERROR::SHADER::PROGRAM::LINKING_FAILED\n" + infoLogStr);
            }

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            GL.UseProgram(shaderProgram);

            int vertexColorLocation = GL.GetUniformLocation(shaderProgram, "u_Color");
            GL.Uniform4f(vertexColorLocation, 1.0f, 1.0f, 0.0f, 1.0f);

            var vertices = new float[]
            {
                0.0f, 0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
                -0.5f, -0.5f, 0.0f,
                0.0f, -1.0f, 0.0f
            };

            var indices = new uint[]
            {
                0, 1, 2, 3
            };

            uint vao = GL.GenVertexArray();
            uint vbo = GL.GenBuffer();
            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices, BufferUsage.StaticDraw);

            GL.VertexAttribPointer(0, 3, DataType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            uint ibo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices, BufferUsage.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            var color = new float[] { 1.0f, 0.0f, 0.0f, 1.0f };

            var sw = new Stopwatch();

            while (window.IsVisible)
            {
                sw.Restart();

                window.PollEvents();

                // slowly change the red color
                if (color[0] >= 1.0f)
                    color[0] = 0.0f;
                else
                    color[0] += 0.01f;

                GL.Uniform4f(vertexColorLocation, color[0], color[1], color[2], color[3]);

                GL.Clear(ClearBufferMask.ColorBufferBit);

                GL.BindVertexArray(vao);
                GL.DrawElements(DrawMode.TriangleStrip, 4, DataType.UnsignedInt, 0);

                window.SwapBuffers();

                //GL.Flush();

                sw.Stop();
                Console.WriteLine("Elapsed: " + sw.ElapsedTicks);
            }

            // Cleanup
            window.Dispose();
        }

        static string[] vertexShaderSource = new string[]
        {
            "#version 330 core\n layout (location = 0) in vec3 aPos;\n void main()\n {\n   gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n }\0"
        };
        //const string vertexShaderSource = @"""
        //#version 330 core

        //layout (location = 0) in vec3 aPos;

        //void main()
        //{
        //    gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
        //}
        //""" + "\0";

        static string[] fragmentShaderSource = new string[]
        {
            "#version 330 core\n out vec4 FragColor;\n uniform vec4 u_Color;\n void main()\n {\n    FragColor = u_Color;\n }\n\0"
        };
    }

    enum DWFlags : uint
    {
        PFD_DRAW_TO_WINDOW = 0x00000004,
        PFD_SUPPORT_OPENGL = 0x00000020,
        PFD_DOUBLEBUFFER = 0x00000001,
        PFD_TYPE_RGBA = 0
    }
}
