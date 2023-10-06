#region ========================================================================= USING =====================================================================================
using NLog;
using System;
using System.IO;
using NLog.Config;
using NLog.Targets;
#endregion

namespace Lyrida.Infrastructure.Common.Logging;

/// <summary>
/// Implementation of ILoggerManager interface that logs using NLog
/// </summary>
/// <remarks>
/// Creation Date: 26th of January, 2021
/// </remarks>
public class NLogLogger : ILoggerManager
{
    #region ================================================================== FIELD MEMBERS ================================================================================
    private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
    #endregion

    #region ====================================================================== CTOR =====================================================================================
    /// <summary>
    /// Default C-tor
    /// </summary>
    public NLogLogger()
    {
        SetLoggingEnvironment();
    }
    #endregion

    #region ===================================================================== METHODS ===================================================================================
    /// <summary>
    /// Initializes NLog
    /// </summary>
    private static void SetLoggingEnvironment()
    {
        // create NLog configuration  
        LoggingConfiguration config = new();
        if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs")))
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"));
        FileTarget fileTarget = new("target2")
        {
            FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", $"{DateTime.Now:yyMMdd}.log"),
            Layout = "${time} ${level} ${message}  ${exception}"
        };
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));
        config.AddTarget(fileTarget);
        LogManager.Configuration = config;
        // enable when debugging NLog itself:
        // LogManager.ThrowExceptions = true;
    }

    /// <summary>
    /// Writes the diagnostic message at Debug level
    /// </summary>
    /// <param name="message">The message to log</param>
    public void LogDebug(string message)
    {
        logger.Debug(message + Environment.NewLine);
    }

    /// <summary>
    /// Writes the diagnostic exception at Debug level
    /// </summary>
    /// <param name="exception">The exception to log</param>
    public void LogDebug(Exception exception)
    {
        logger.Debug(exception);
    }

    /// <summary>
    /// Writes the diagnostic message at Error level
    /// </summary>
    /// <param name="message">The exception to log</param>
    public void LogError(string message)
    {
        logger.Error(message + Environment.NewLine);
    }

    /// <summary>
    /// Writes the diagnostic exception at Error level
    /// </summary>
    /// <param name="exception">The exception to log</param>
    public void LogError(Exception exception)
    {
        logger.Error(exception);
    }

    /// <summary>
    /// Writes the diagnostic message at Info level
    /// </summary>
    /// <param name="message">The message to log</param>
    public void LogInfo(string message)
    {
        logger.Info(message + Environment.NewLine);
    }

    /// <summary>
    /// Writes the diagnostic exception at Info level
    /// </summary>
    /// <param name="exception">The exception to log</param>
    public void LogInfo(Exception exception)
    {
        logger.Info(exception);
    }

    /// <summary>
    /// Writes the diagnostic message at Warn level
    /// </summary>
    /// <param name="message">The message to log</param>
    public void LogWarn(string message)
    {
        logger.Warn(message + Environment.NewLine);
    }

    /// <summary>
    /// Writes the diagnostic exception at Warn level
    /// </summary>
    /// <param name="exception">The exception to log</param>
    public void LogWarn(Exception exception)
    {
        logger.Warn(exception);
    }
    #endregion
}