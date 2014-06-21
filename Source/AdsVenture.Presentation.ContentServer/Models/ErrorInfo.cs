using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdsVenture.Presentation.ContentServer.Models
{
    public class ErrorInfo
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorType Type { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class ValidationErrorInfo : ErrorInfo
    {
        [JsonProperty(Order = int.MaxValue)]
        public IEnumerable ValidationErrors { get; set; }
    }

    public class DebuggeableErrorInfo : ErrorInfo
    {
        [JsonProperty(Order = int.MaxValue)]
        public string StackTrace { get; set; }
    }

    public enum ErrorType
    {
        AUTH,
        BUSINESS,
        CRITICAL,
        NOTFOUND,
        LOGIN
    }

    public enum ErrorCode
    {
        NOAUTH,
        INVALID_TOKEN,
        UNAUTH,
        VALIDATION,
        ROUTE,
        INVALID_CREDENTIALS,
        DB_ENTITY_VALIDATION
    }
}