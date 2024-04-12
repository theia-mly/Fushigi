using DiscordRPC;
using DiscordRPC.Logging;

namespace Fushigi
{
    public static class DRPC
    {
        const string mClientID = "1227725734429196298";

        static DiscordRpcClient? mClient;
        static readonly DateTime mStartTime = DateTime.Now.ToUniversalTime();

        public static void Initialize()
        {
            mClient = new DiscordRpcClient(mClientID)
            {
                Logger = new RPCLogger()
            };

            AppDomain.CurrentDomain.ProcessExit += Dispose;

            mClient.OnReady += (sender, e) =>
            {
                Logger.Logger.LogMessage("DiscordRPC", "Ready");
            };

            mClient.Initialize();
        }

        private static void Dispose(object? sender, EventArgs e)
        {
            mClient?.Dispose();
        }

        static void SetPresence(string details, string state, string imageKey)
        {
            if (mClient == null)
            {
                Logger.Logger.LogWarning("DiscordRPC", "Tried to set presence with a null client");
                return;
            }

            mClient.SetPresence(new RichPresence()
            {
                Details = details,
                State = state,
                Assets = new Assets()
                {
                    LargeImageKey = imageKey,
                    LargeImageText = "Fushigi"
                },
                Timestamps = new Timestamps()
                {
                    Start = mStartTime
                }
            });
        }

        public static void SetEditingCourse(string courseID, string courseName, int worldNumber)
        {
            SetPresence($"Editing {courseID}", courseName, "icon" + worldNumber);
        }

        class RPCLogger : ILogger
        {
            LogLevel mLogLevel;

            public LogLevel Level { get => mLogLevel; set => mLogLevel = value; }

            public RPCLogger()
            {
                mLogLevel = LogLevel.Warning;
            }

            public void Error(string message, params object[] args)
            {
                Logger.Logger.LogError("DiscordRPC", message);
            }

            public void Info(string message, params object[] args)
            {
                //Logger.Logger.LogMessage("DiscordRPC", message);
            }

            public void Trace(string message, params object[] args)
            {
                //Logger.Logger.LogMessage("DiscordRPC", message);
            }

            public void Warning(string message, params object[] args)
            {
                Logger.Logger.LogWarning("DiscordRPC", message);
            }
        }
    }
}
