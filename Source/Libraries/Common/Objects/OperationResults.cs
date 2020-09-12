namespace RTCV.Common.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    public class OperationResult
    {
        public string Message { get; private set; }
        public Exception Exception { get; private set; }
        public NLog.LogLevel Severity { get; private set; }

        public OperationResult(string message, NLog.LogLevel severity, Exception e = null)
        {
            this.Message = message;
            this.Severity = severity;
            this.Exception = e;
        }

        public OperationResult(string message, NLog.LogLevel severity, NLog.Logger logger, Exception e = null)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.Message = message;
            this.Severity = severity;
            this.Exception = e;
            logger.Log(severity, e, message);
        }
    }

    public class OperationResults
    {
        private List<OperationResult> messages;
        private readonly ReadOnlyCollection<OperationResult> Messages;

        public ReadOnlyCollection<OperationResult> Warnings => messages.Where(x => x.Severity == NLog.LogLevel.Warn).ToList().AsReadOnly();
        public ReadOnlyCollection<OperationResult> Errors => messages.Where(x => x.Severity == NLog.LogLevel.Error).ToList().AsReadOnly();

        public OperationResults()
        {
            messages = new List<OperationResult>();
            Messages = messages.AsReadOnly();
        }

        public void AddResult(OperationResult result) => messages.Add(result);

        public void AddResults(OperationResults results) {
            if (results == null)
            {
                throw new ArgumentNullException(nameof(results));
            }

            messages.AddRange(results.Messages);
        }

        public void AddWarning(string warning) => messages.Add(new OperationResult(warning, NLog.LogLevel.Warn));

        public void AddWarning(string warning, NLog.Logger logger)
        {
            messages.Add(new OperationResult(warning, NLog.LogLevel.Warn, logger));
            logger.Warn(warning);
        }

        public void AddError(string error, Exception e = null) => messages.Add(new OperationResult(error, NLog.LogLevel.Error, e));

        public void AddError(string error, NLog.Logger logger, Exception e = null)
        {
            messages.Add(new OperationResult(error, NLog.LogLevel.Error, logger, e));
            logger.Error(e, error);
        }

        public bool HasErrors() => Errors.Count > 0;

        public bool HasWarnings() => Warnings.Count > 0;

        public bool HasMessages() => Warnings.Count > 0 || Errors.Count > 0;

        public bool Failed => HasErrors();

        public string GetWarningsFormatted() => getMessagesFormatted(NLog.LogLevel.Warn);

        public string GetErrorsFormatted(bool includeException = false) => getMessagesFormatted(NLog.LogLevel.Error, includeException);

        private string getMessagesFormatted(NLog.LogLevel severity, bool includeException = false)
        {
            var sb = new StringBuilder();

            foreach (var message in messages.Where(x => x.Severity == severity))
            {
                sb.AppendLine($"{message.Severity}: {message.Message}");
                if (includeException)
                {
                    var ex = message.Exception;
                    while (ex != null)
                    {
                        sb.AppendLine($"Exception: {message.Exception}");
                        ex = ex.InnerException;
                    }
                }
            }
            return sb.ToString();
        }
    }

    public class OperationResults<T> : OperationResults
    {
        public T Result { get; set; }

        public OperationResults() : base()
        {
        }

        public OperationResults(T result)
        {
            Result = result;
        }
    }
}
