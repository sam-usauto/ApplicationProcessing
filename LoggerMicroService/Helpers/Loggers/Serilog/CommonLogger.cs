using System;
using System.Collections.Generic;
using Serilog;

namespace LoggerMicroService.Helpers.Loggers.Serilog
{
    public class CommonLogger
    {
        private static ILogger _perfLogger;
        private static ILogger _usageLogger;
        private static ILogger _errorLogger;
        private static ILogger _diagnosticLogger;

        public static LoggerFileLocation _loggerFileLocation;   // Location for logger files
        public static bool _enableDiagnostics;                  // write diagnostics to log
        public static LogDefaultWhere _logDefaultWhere;          // default values for the log WHERE section

        static CommonLogger()
        {
        }

        /// <summary>
        /// SetLogger must be called one time to set the loggers and the log file locations
        /// </summary>
        /// <param name="_loggerFileLocation"></param>
        public static void SetLoggers(LoggerFileLocation LoggerFileLocation, bool EnableDiagnostics, LogDefaultWhere LogDefaultWhere)
        {
            try
            {
                _enableDiagnostics = EnableDiagnostics;

                _logDefaultWhere = LogDefaultWhere;

                _perfLogger = new LoggerConfiguration()
                    // .WriteTo.File(path: LoggerFileLocation.PerfLoggerLocation)
                    .WriteTo.RollingFile(LoggerFileLocation.PerfLoggerLocation, retainedFileCountLimit: null)
                    .CreateLogger();

                _usageLogger = new LoggerConfiguration()
                    // .WriteTo.File(path: LoggerFileLocation.UsageLoggerLocation)
                    .WriteTo.RollingFile(LoggerFileLocation.UsageLoggerLocation, retainedFileCountLimit: null)
                    .CreateLogger();

                _errorLogger = new LoggerConfiguration()
                    // .WriteTo.File(path: LoggerFileLocation.ErrorLoggerLocation)
                    .WriteTo.RollingFile(LoggerFileLocation.ErrorLoggerLocation, retainedFileCountLimit: null)
                    .CreateLogger();

                _diagnosticLogger = new LoggerConfiguration()
                    // .WriteTo.File(path: LoggerFileLocation.DiagnosticLoggerLocation)
                    .WriteTo.RollingFile(LoggerFileLocation.DiagnosticLoggerLocation, retainedFileCountLimit: null)
                    .CreateLogger();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void WriteDebugInfoUsage(string msg, Exception ex, Dictionary<string, object> additionalInfo)
        {
            var infoToLog = new LogDetail();
            infoToLog.Message = msg;
            if (additionalInfo != null)
            {
                infoToLog.AdditionalInfo = additionalInfo;
            }
            if (ex != null)
            {
                infoToLog.Exception = ex;
            }
            _usageLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        public static void WriteErrorInfoUsage(string msg, Exception ex, Dictionary<string, object> additionalInfo)
        {
            var infoToLog = new LogDetail();
            infoToLog.Message = msg;
            if (additionalInfo != null)
            {
                infoToLog.AdditionalInfo = additionalInfo;
            }
            if (ex != null)
            {
                infoToLog.Exception = ex;
            }
            _errorLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
            WriteErrorLogSpacerUsage();
        }

        // write empty log.. used as visualized spacer between logs
        public static void WriteSeeErrorLogUsage()
        {
            var infoToLog = new LogDetail();
            infoToLog.Message = "See Error log for details";
            _usageLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        // write empty log.. used as visualized spacer between logs
        public static void WriteSpacerUsage()
        {
            var infoToLog = new LogDetail();
            infoToLog.Message = "                                                                                             ";
            _usageLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        // write empty log.. used as visualized spacer between logs
        public static void WriteAbundantSpacerUsage(bool Saved, string msg = "( No enough information to save )")
        {
            var infoToLog = new LogDetail();
            if (Saved)
            {
                infoToLog.Message = "---  C O M P L E T E D  ---";
            }
            else
            {
                infoToLog.Message = $"---  F A I L E D   {msg}  -----------";
            }
            _usageLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);

            WriteSpacerUsage();

        }

        public static void WritePerf(LogDetail infoToLog)
        {
            _perfLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }
        public static void WriteUsage(LogDetail infoToLog)
        {
            _usageLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }
        public static void WriteError(LogDetail infoToLog)
        {
            if (infoToLog.Exception != null)
            {
                var procName = FindProcName(infoToLog.Exception);
                infoToLog.Location = string.IsNullOrEmpty(procName) ? infoToLog.Location : procName;
                infoToLog.Message = GetMessageFromException(infoToLog.Exception);
            }
            _errorLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
            WriteErrorLogSpacerUsage();
        }

        // write empty log.. used as visualized spacer between logs
        public static void WriteErrorLogSpacerUsage()
        {
            var infoToLog = new LogDetail();
            infoToLog.Message = "                                                                                             ";
            _errorLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        public static void WriteDiagnostic(LogDetail infoToLog)
        {
            if (!_enableDiagnostics)
                return;

            _diagnosticLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        private static string FindProcName(Exception ex)
        {
            var sqlEx = ex as SqlException;
            if (sqlEx != null)
            {
                var procName = sqlEx.Procedure;
                if (!string.IsNullOrEmpty(procName))
                    return procName;
            }

            if (!string.IsNullOrEmpty((string)ex.Data["Procedure"]))
            {
                return (string)ex.Data["Procedure"];
            }

            if (ex.InnerException != null)
                return FindProcName(ex.InnerException);

            return null;
        }

        private static string GetMessageFromException(Exception ex)
        {
            if (ex.InnerException != null)
                return GetMessageFromException(ex.InnerException);

            return ex.Message;
        }
    }
}
