using AdsVenture.Presentation.ContentServer.Helpers.Extensions;
using AdsVenture.Presentation.ContentServer.Models;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace AdsVenture.Presentation.ContentServer.Filters
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        private MediaTypeFormatter _formatter;

        public ExceptionHandlingAttribute(MediaTypeFormatter formatter)
        {
            this._formatter = formatter;
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            var exc = context.Exception;

            ExceptionLogging.Log(exc);

            string exMessage = exc.Message ?? Commons.Resources.Shared.Error_Undefined;

            var excType = exc.GetType();
            string exTypeName = excType.Name;
            if (excType.IsGenericType && excType.GetGenericArguments().Length == 1)
            {
                Type g = excType.GetGenericTypeDefinition();
                var gTypeName = g.Name.Remove(g.Name.IndexOf('`'));
                var gArgTypeName = excType.GetGenericArguments()[0].Name;
                exTypeName = string.Format("{0}<{1}>", gTypeName, gArgTypeName);
            }

            // HttpException 404
            if (typeof(HttpException).IsAssignableFrom(exc.GetType())
                && (exc as HttpException).GetHttpCode() == 404)
            {
                ThrowError(HttpStatusCode.NotFound, exMessage,
                    new ErrorInfo()
                    {
                        Type = ErrorType.NOTFOUND,
                        Code = ErrorCode.ROUTE.ToString(),
                        Message = exMessage,
                    });
            }

            // EntityNotFoundException<ActionResource> => 404
            if (typeof(Core.Exceptions.EntityNotFoundException).IsAssignableFrom(exc.GetType()) && exc.GetType().IsGenericType)
            {
                var resourceType = excType.GetGenericArguments()[0];
                if(context.ActionContext.ActionDescriptor.IsResource(resourceType))
                {
                    exMessage = string.Format("Entity '{0}' was not found", resourceType.Name);
                    ThrowError(HttpStatusCode.NotFound, exMessage,
                        new ErrorInfo()
                        {
                            Type = ErrorType.NOTFOUND,
                            Code = resourceType.Name,
                            Message = exMessage,
                        });
                }
            }

            // UnauthorizedExceptions => 403
            if (typeof(Core.Exceptions.UnauthorizedException).IsAssignableFrom(exc.GetType()))
            {
                ThrowError(HttpStatusCode.Forbidden, exTypeName,
                    new DebuggeableErrorInfo()
                    {
                        Type = ErrorType.AUTH,
                        Code = ErrorCode.UNAUTH.ToString(),
                        Message = exMessage,
                        StackTrace = IsDebuggingEnabled ? exc.StackTrace : null
                    });
            }

            // Other BusinessExceptions => 409
            if (typeof(Core.Exceptions.BusinessException).IsAssignableFrom(exc.GetType()))
            {
                if (typeof(Core.Exceptions.BusinessUserException).IsAssignableFrom(exc.GetType())
                    || typeof(Core.Exceptions.ParameterException<>).IsAssignableFrom(exc.GetType()))
                {
                    ThrowError(HttpStatusCode.Conflict, exMessage,
                        new DebuggeableErrorInfo()
                        {
                            Type = ErrorType.BUSINESS,
                            Code = exTypeName,
                            Message = exMessage,
                            StackTrace = IsDebuggingEnabled ? exc.StackTrace : null
                        });
                }

                if (typeof(Core.Exceptions.EntityValidationException).IsAssignableFrom(exc.GetType()))
                {
                    var evex = (Core.Exceptions.EntityValidationException)exc;

                    ThrowError(HttpStatusCode.Conflict, exTypeName,
                        new ValidationErrorInfo()
                        {
                            Type = ErrorType.BUSINESS,
                            Code = ErrorCode.VALIDATION.ToString(),
                            Message = exMessage,
                            ValidationErrors = evex.Errors
                        });
                }

                ThrowError(HttpStatusCode.Conflict, exTypeName,
                    new DebuggeableErrorInfo()
                    {
                        Type = ErrorType.BUSINESS,
                        Code = exTypeName,
                        Message = exMessage,
                        StackTrace = IsDebuggingEnabled ? exc.StackTrace : null
                    });
            }

            // DbEntityValidationException => 500
            if (typeof(DbEntityValidationException).IsAssignableFrom(exc.GetType()))
            {
                var dbValExep = (DbEntityValidationException)exc;
                ThrowError(HttpStatusCode.InternalServerError, exTypeName,
                    new ValidationErrorInfo()
                    {
                        Type = ErrorType.CRITICAL,
                        Code = ErrorCode.DB_ENTITY_VALIDATION.ToString(),
                        Message = "Validation failed for one or more entities. See 'validationErrors' property for more details.",
                        ValidationErrors = dbValExep.EntityValidationErrors
                            .Cast<DbEntityValidationResult>()
                            .Select(x => new 
                            { 
                                Entity = x.Entry.Entity.GetType().Name, 
                                Errors = x.ValidationErrors 
                            }),
                    });
            }

            // Other Exceptions => 500
            ThrowError(HttpStatusCode.InternalServerError, exTypeName,
                new DebuggeableErrorInfo()
                {
                    Type = ErrorType.CRITICAL,
                    Code = exTypeName,
                    Message = exMessage,
                    StackTrace = IsDebuggingEnabled ? exc.StackTrace : null
                });
        }

        private bool IsDebuggingEnabled { get { return HttpContext.Current.IsDebuggingEnabled; } }

        private void ThrowError(HttpStatusCode statusCode, string reasonPhrase, ErrorInfo errorInfo)
        {
            throw new HttpResponseException(new HttpResponseMessage(statusCode)
            {
                Content = new ObjectContent<ErrorInfo>(errorInfo, _formatter),
                ReasonPhrase = reasonPhrase
            });
        }
    }
}