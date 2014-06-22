using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AdsVenture.Presentation.ContentServer
{
    public static class WebApiConfig
    {
        #region Constraints
        public class QueryStringConstraint : IRouteConstraint
        {
            private string _constraintValue;
            private bool _mandatory;

            public QueryStringConstraint(string constraintValue, bool mandatory)
            {
                this._constraintValue = constraintValue;
                this._mandatory = mandatory;
            }

            public bool Match(System.Web.HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {

                //si !mandatory puede no estar o si esta tiene que ser el valor
                //si mandatory tiene que estar y ser el valor

                var incoming =
                    routeDirection == RouteDirection.IncomingRequest
                    && (
                        (
                            _mandatory
                            && httpContext.Request.QueryString[parameterName] == this._constraintValue
                        ) || (
                            !_mandatory
                            && (
                                string.IsNullOrWhiteSpace(httpContext.Request.QueryString[parameterName])
                                || httpContext.Request.QueryString[parameterName] == this._constraintValue
                            )
                        )
                    );

                var urlGeneration =
                    routeDirection == RouteDirection.UrlGeneration
                    && (
                        (
                            _mandatory
                            && values.ContainsKey(parameterName)
                            && values[parameterName].ToString() == this._constraintValue
                        ) || (
                            !_mandatory
                            && (
                                !values.ContainsKey(parameterName)
                                || values[parameterName].ToString() == this._constraintValue
                            )
                        )
                    );

                return incoming || urlGeneration;
            }
        }
        #endregion

        public static void Register(HttpConfiguration config)
        {
            config.EnableCors(WebApiApplication.ServiceLocator.GetService<EnableCorsAttribute>());

            Routes(config);
            Formatters(config);
            Filters(config);
        }

        private static void Routes(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional, },
                constraints: new { id = "\\d*" }
            );
        }

        private static void Formatters(HttpConfiguration config)
        {
            // JSON > Use by default
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes
                .FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            // JSON > Use camel case
            config.Formatters.JsonFormatter.SerializerSettings
                .ContractResolver = new CamelCasePropertyNamesContractResolver();

            // JSON > Convert dates to UTC
            config.Formatters.JsonFormatter.SerializerSettings
                .DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        }

        private static void Filters(HttpConfiguration config)
        {
            config.Filters.Add(new Filters.ExceptionHandlingAttribute(config.Formatters.JsonFormatter));
        }
    }
}
