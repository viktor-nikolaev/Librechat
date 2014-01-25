using NLog;

namespace LibreChat.Messenger.Services
{
  public static class LogService
  {
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public static void Log(string message)
    {
      _logger.Info(message);
    }
  }
}