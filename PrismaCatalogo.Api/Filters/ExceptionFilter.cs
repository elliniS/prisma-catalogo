using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PrismaCatalogo.Api.Exceptions;
using PrismaCatalogo.Api.Extensions;
using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);


            if (context.Exception is APIException)
            {
                APIException apiException = (APIException)context.Exception;

                context.Result = new BadRequestObjectResult(new ErrorDetails(422, context.Exception.Message, context.ModelState.GetErrosMenssages()))
                {
                    StatusCode = apiException.CodeStatus
                };
            }
            else
            {
                context.Result = new BadRequestObjectResult(new ErrorDetails(500, "Erro não tratado", null))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
