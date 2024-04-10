namespace Fushigi.Logger
{
    public static class Logger
    {
        public static bool IsInitialized { get; private set; }

        private static FileStream? mOutputStream;
        private static StreamWriter? mConsoleWriter;

        public static void CreateLogger()
        {
            if (IsInitialized) return;

            mOutputStream = new FileStream("output.log", FileMode.Create);
            mConsoleWriter = new StreamWriter(mOutputStream)
            {
                AutoFlush = true
            };

            IsInitialized = true;

            LogMessage("Logger", "Initialized logger");
        }

        public static void CloseLogger()
        {
            if (!IsInitialized || mOutputStream == null || mConsoleWriter == null)
            {
                LogError("Logger", "Can't close logger before initializing!");
                return;
            }

            mOutputStream.Close();

            IsInitialized = false;
        }

        public static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!IsInitialized || e.ExceptionObject == null) return;
            if (e.ExceptionObject is not Exception exception) return;

            Console.ForegroundColor = ConsoleColor.Red;
            if (exception.StackTrace == null)
                Log(exception.Message);
            else
                Log(exception.StackTrace);
            Console.ForegroundColor = ConsoleColor.White;

            Environment.Exit(1);
        }

        private static void Log(object msg)
        {
            if (!IsInitialized || mConsoleWriter == null) return;

            mConsoleWriter.WriteLine(msg);
            Console.WriteLine(msg);
        }

        public static void LogMessage(object from, object msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Log($"[{from}]: {msg}");
        }

        public static void LogWarning(object from, object msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Log($"[WARN] [{from}]: {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void LogError(object from, object msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Log($"[ERROR] [{from}]: {msg}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
