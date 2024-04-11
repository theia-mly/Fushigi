using Fushigi.util;
using Silk.NET.Core;
using Silk.NET.Core.Contexts;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System.Runtime.InteropServices;

namespace Fushigi.windowing
{
    internal static class WindowManager
    {
        public static IGLContext? SharedContext { get; private set; } = null;

        private static GL? sGL = null;

        private record struct WindowResources(ImGuiController ImguiController, IInputContext Input, GL Gl, bool HasRenderDelegate);

        private static bool sIsRunning = false;

        private static readonly List<IWindow> sPendingInits = [];
        private static readonly List<(IWindow window, WindowResources res)> sWindows = [];

        public static unsafe void CreateWindow(out IWindow window, Vector2D<int>? initialWindowSize = null, Action? onConfigureIO = null)
        {
            var options = WindowOptions.Default;
            options.Title = $"Fushigi {Program.Version}";
            options.API = new GraphicsAPI(
                ContextAPI.OpenGL,
                ContextProfile.Core,
                ContextFlags.Debug | ContextFlags.ForwardCompatible,
                new APIVersion(3, 3)
                );

            if (initialWindowSize.TryGetValue(out var size))
                options.Size = size;

            options.IsVisible = false;

            window = Window.Create(options);

            var _window = window;

            window.Load += () =>
            {
                sGL ??= _window.CreateOpenGL();

                //initialization
                if (_window.Native!.Win32.HasValue)
                    WindowsDarkmodeUtil.SetDarkmodeAware(_window.Native.Win32.Value.Hwnd);

                var input = _window.CreateInput();
                var imguiController = new ImGuiController(sGL, _window, input, onConfigureIO);

                //update
                _window.Update += ds => imguiController.Update((float)ds);

                Logger.Logger.LogMessage("WindowManager", "Loading icon");
                using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(Path.Combine("res", "Icon.png"));
                var memoryGroup = image.GetPixelMemoryGroup();
                Memory<byte> array = new byte[memoryGroup.TotalLength * sizeof(Rgba32)];
                var block = MemoryMarshal.Cast<byte, Rgba32>(array.Span);
                foreach (var memory in memoryGroup)
                {
                    memory.Span.CopyTo(block);
                    block = block[memory.Length..];
                }

                var icon = new RawImage(image.Width, image.Height, array);
                _window.SetWindowIcon(ref icon);

                sWindows.Add((_window, new WindowResources(imguiController, input, sGL, false)));
            };

            sPendingInits.Add(window);
        }

        public static void RegisterRenderDelegate(IWindow window, Action<GL, double, ImGuiController> renderGLDelegate)
        {
            int idx = sWindows.FindIndex(x => x.window == window);

            if (idx == -1)
                throw new Exception($"window was not created using the {nameof(WindowManager)} class");

            var res = sWindows[idx].res;

            if(res.HasRenderDelegate)
                throw new Exception("window has already registered a render delegate");

            var isRequestShow = true;
            window.Render += (deltaSeconds) =>
            {
                res.ImguiController.MakeCurrent();

                renderGLDelegate.Invoke(res.Gl, deltaSeconds, res.ImguiController);

                if (isRequestShow)
                {
                    window.IsVisible = true;
                    isRequestShow = false;
                }
            };

            res.HasRenderDelegate = true;
            sWindows[idx] = (window, res);
        }

        public static void Run()
        {
            if (sIsRunning)
                return;

            sIsRunning = true;

            while (sWindows.Count > 0 || sPendingInits.Count > 0)
            {
                if (sPendingInits.Count > 0)
                {
                    foreach (var window in sPendingInits)
                    {
                        window.Initialize();
                        SharedContext ??= window.GLContext;
                    }

                    sPendingInits.Clear();
                }

                for (int i = 0; i < sWindows.Count; i++)
                {
                    var (window, res) = sWindows[i];

                    window.DoEvents();
                    if (!window.IsClosing)
                        window.DoUpdate();

                    if (!window.IsClosing)
                        window.DoRender();

                    if (window.IsClosing)
                    {
                        sWindows.RemoveAt(i);

                        if (window.GLContext == SharedContext && sWindows.Count > 0)
                            SharedContext = sWindows[0].window.GLContext;

                        res.Input.Dispose();
                        res.ImguiController.Dispose();

                        window.DoEvents();
                        window.Reset();

                        i--;
                    }
                }
            }

            sGL?.Dispose();
        }
    }
}