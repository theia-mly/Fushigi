using Fushigi.util;
using Fushigi.param;
using Fushigi.ui;
using System.Runtime.InteropServices;
using Fushigi.windowing;
using Fushigi.Logger;
using Fushigi;
using System;
using System.Diagnostics;
using FuzzySharp;

internal class Program
{
    public const string Version = "v1.5.2";

    public static MainWindow MainWindow { get; private set; }

    private static void Main(string[] args)
    {
        try
        {
            RunFushigi();
        } catch
        {
            static string TerminalURL(string url) => $"\u001B]8;;{url}\a{url}\u001B]8;;\a";

            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Could not start Fushigi!");

            // Let the user download a virus
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please make sure you have .NET 8 installed. You can download it here:");
            Console.WriteLine(TerminalURL("https://dotnet.microsoft.com/en-us/download/dotnet/8.0"));
            Console.ResetColor();
        }
    }

    private static void RunFushigi()
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

        MainWindow = new MainWindow();
        WindowManager.Run();

        Logger.CloseLogger();
    }

    /*
    private static bool TestForNET8()
    {
        // Ask the .NET version
        System.Diagnostics.Process cmd = new System.Diagnostics.Process();
        cmd.StartInfo.FileName = "cmd.exe";
        cmd.StartInfo.RedirectStandardInput = true;
        cmd.StartInfo.RedirectStandardOutput = true;
        cmd.StartInfo.CreateNoWindow = true;
        cmd.StartInfo.UseShellExecute = false;
        cmd.Start();

        cmd.StandardInput.WriteLine("dotnet --version");
        cmd.StandardInput.Flush();
        cmd.StandardInput.Close();
        cmd.WaitForExit();
        string version = cmd.StandardOutput.ReadToEnd();
        string verionNumber = version.Split("--version\r\n")[1].Split(".")[0];

        if (verionNumber != "5")
        {
            return false;
        }
        else
        {
            return true;
        }
    }*/
}