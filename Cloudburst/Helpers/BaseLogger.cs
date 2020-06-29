using BepInEx.Logging;

internal class BaseLogger
{
    public static ManualLogSource logger;
    public static void Log(string message)
    {
        logger.LogInfo(message);
    }
    public static void LogD(string message)
    {
        logger.LogDebug(message);
    }
    public static void LogE(string message)
    {
        logger.LogError(message);
    }
    public static void LogF(string message)
    {
        logger.LogFatal(message);
    }
    public static void LogM(string message)
    {
        logger.LogMessage(message);
    }
    public static void LogW(string message)
    {
        logger.LogWarning(message);
    }
}