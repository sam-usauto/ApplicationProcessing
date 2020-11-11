﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Common.DTOs.Loggers.Serilog;
using Serilog;
using Serilog.Events;

namespace LoggerMicroService.Helpers.Loggers.Serilog
{
    public static class CommonLogger
    {
        private static ILogger _perfLogger;
        private static ILogger _usageLogger;
        private static ILogger _errorLogger;
        private static ILogger _diagnosticLogger;

        public static void SetLoggers(SerilogConfig serilogConfig)
        {
            _perfLogger = new LoggerConfiguration()
                .WriteTo.File(path: serilogConfig.FileLocation.PerfLoggerLocation)
                //.WriteTo.File(path: Environment.GetEnvironmentVariable("LOGFILE_PERF"))
                .CreateLogger();

            _usageLogger = new LoggerConfiguration()
                .WriteTo.File(path: serilogConfig.FileLocation.UsageLoggerLocation)
                //.WriteTo.File(path: Environment.GetEnvironmentVariable("LOGFILE_USAGE"))
                .CreateLogger();

            _errorLogger = new LoggerConfiguration()
                .WriteTo.File(path: serilogConfig.FileLocation.ErrorLoggerLocation)
                //.WriteTo.File(path: Environment.GetEnvironmentVariable("LOGFILE_ERROR"))
                .CreateLogger();

            _diagnosticLogger = new LoggerConfiguration()
                .WriteTo.File(path: serilogConfig.FileLocation.DiagnosticLoggerLocation)
                //.WriteTo.File(path: Environment.GetEnvironmentVariable("LOGFILE_DIAG"))
                .CreateLogger();
        }

        static CommonLogger()
        {
            //_perfLogger = new LoggerConfiguration()
            //    .WriteTo.File(path: Environment.GetEnvironmentVariable("LOGFILE_PERF"))
            //    .CreateLogger();

            //_usageLogger = new LoggerConfiguration()
            //    .WriteTo.File(path: Environment.GetEnvironmentVariable("LOGFILE_USAGE"))
            //    .CreateLogger();

            //_errorLogger = new LoggerConfiguration()
            //    .WriteTo.File(path: Environment.GetEnvironmentVariable("LOGFILE_ERROR"))
            //    .CreateLogger();

            //_diagnosticLogger = new LoggerConfiguration()
            //    .WriteTo.File(path: Environment.GetEnvironmentVariable("LOGFILE_DIAG"))
            //    .CreateLogger();
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
                infoToLog.Location = string.IsNullOrEmpty(procName)
                    ? infoToLog.Location
                    : procName;
                infoToLog.Message = GetMessageFromException(infoToLog.Exception);
            }
            _errorLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        public static void WriteDiagnostic(LogDetail infoToLog)
        {
            var writeDiagnostics =
                Convert.ToBoolean(Environment.GetEnvironmentVariable("DIAGNOSTICS_ON"));
            if (!writeDiagnostics)
                return;

            _diagnosticLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }

        private static string GetMessageFromException(Exception ex)
        {
            if (ex.InnerException != null)
                return GetMessageFromException(ex.InnerException);

            return ex.Message;
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
    }
}
