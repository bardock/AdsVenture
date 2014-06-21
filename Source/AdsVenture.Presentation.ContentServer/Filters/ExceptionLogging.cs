using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bardock.Utils.Extensions;

namespace AdsVenture.Presentation.ContentServer.Filters
{
    public class ExceptionLogging
    {

        public static void Log(Exception exc)
        {
            Bardock.Utils.Logger.ILog log = null;
            try
            {
                log = Bardock.Utils.Logger.Manager.GetLog<ExceptionLogging>();

                // Ignore some exception types
                if (exc == null || exc is System.Threading.ThreadAbortException || exc is HttpException && ((HttpException)exc).GetHttpCode().In(401, 403, 404))
                {
                    return;
                }

                string message = GetMessage(exc);

                // Log exception
                if (exc is HttpException || typeof(Core.Exceptions.BusinessException).IsAssignableFrom(exc.GetType()))
                {
                    log.Warn(message, exc);
                }
                else
                {
                    log.Error(message, exc);

                    // Clear the error from the server
                    //Server.ClearError();
                }

            }
            catch (Exception ex)
            {
                if (log != null)
                {
                    log.Error("Logging unhandled exception failed", ex);
                }
            }
        }

        private static string GetMessage(Exception exc)
        {
            string message = string.Empty;
            try
            {
                message += string.Format("\n   User ID: {0}", "-" /* TODO: Helpers.Authentication.SessionUser.Current.ID */);
            }
            catch
            {
            }
            try
            {
                message += string.Format("\n   URL: {0}", HttpContext.Current.Request.Url.ToString());
            }
            catch
            {
            }
            try
            {
                if (typeof(Core.Exceptions.EntityValidationException).IsAssignableFrom(exc.GetType()))
                {
                    message += "\n   VALIDATION ERRORS:";
                    ((Core.Exceptions.EntityValidationException)exc)
                        .Errors.ToList()
                        .ForEach(x => message += string.Format("\n      {0}", x.ErrorMessage));
                }
            }
            catch
            {
            }
            return message;
        }
    }
}