using Microsoft.Extensions.Logging;
using Newshore.Journeys.Application._Messages;
using Newshore.Journeys.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Newshore.Journeys.Application.Response
{
    public class JourneysResponse<TResponse>
    {
        public bool Success { get; set; }

        public TResponse Result { get; set; }

        public string Message { get; set; }

        private readonly Func<TResponse> @Delegate;

        private readonly ILogger @Logger;

        public JourneysResponse(Func<TResponse> @delegate, string message, ILogger @logger)
        {
            @Delegate = @delegate;
            Message = message;
            @Logger = @logger;
        }

        public static JourneysResponse<TResponse> Create(Func<TResponse> @delegate, string message, ILogger @logger)
        {
            return new JourneysResponse<TResponse>(@delegate, message, @logger);
        }

        public JourneysResponse<TResponse> Execute()
        {
            try
            {
                @Logger.LogInformation(LogMessage.StartTransaction);
                Result = @Delegate();
                Success = true;
                @Logger.LogInformation(LogMessage.TransactionEndedSuccessfully);
                return this;
            }
            catch(DomainException domainEx)
            {
                Success = false;
                Message = domainEx.Message;
                @Logger.LogInformation(LogMessage.TransactionEndedWithErrors);
                @Logger.LogError(domainEx.Message, domainEx);
                return this;
            }
            catch(Exception ex)
            {
                Success = false;
                Message = ErrorMessage.InternalServerError;
                @Logger.LogInformation(LogMessage.TransactionEndedWithErrors);
                @Logger.LogError(ex.Message, ex);
                return this;
            }
        }
    }
}
