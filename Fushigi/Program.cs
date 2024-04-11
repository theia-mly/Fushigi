using Fushigi.util;
using Fushigi.param;
using Fushigi.ui;
using System.Runtime.InteropServices;
using Fushigi.windowing;
using Fushigi.Logger;
using Fushigi;

internal class Program
{
    public const string Version = "v1.1.5.2";

    private static void Main(string[] args)
    {
        Logger.CreateLogger();

        AppDomain.CurrentDomain.UnhandledException += Logger.HandleUnhandledException;

        Logger.LogMessage("Program", $"Starting Fushigi {Version}...");

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            Logger.LogMessage("Program", "Running on osx");
        else
            Logger.LogMessage("Program", "Not running on osx");

        Logger.LogMessage("Program", "Loading user settings...");
        UserSettings.Load();
        Logger.LogMessage("Program", "Loading parameter database...");
        ParamDB.Init();
        Logger.LogMessage("Program", "Loading area parameter loader...");
        ParamLoader.Load();

        Logger.LogMessage("Program", "Checking for imgui.ini");
        if (!Path.Exists("imgui.ini"))
        {
            Logger.LogMessage("Program", "Creating imgui.ini...");
            File.WriteAllText("imgui.ini", File.ReadAllText(Path.Combine("res", "imgui-default.ini")));
            Logger.LogMessage("Program", "Created!");
        };

        DRPC.Initialize();

        _ = new MainWindow();
        WindowManager.Run();

        Logger.CloseLogger();
    }
}