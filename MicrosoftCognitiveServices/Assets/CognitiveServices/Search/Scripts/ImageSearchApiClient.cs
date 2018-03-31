using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Cognitive.Search.Images
{
    public static class ImageSearchApiClient
    {
        private const string RequestHeaderKey = "Ocp-Apim-Subscription-Key";
        public static string ApiKey { get; set; } = "===>API Key Goes Here<===";
        public static Dictionary<string, string> ApiKeyHeader
        {
            get
            {
                if (ApiKey != currentFaceApiKeyHeader[RequestHeaderKey])
                {
                    currentFaceApiKeyHeader = new Dictionary<string, string> { { RequestHeaderKey, ApiKey } };
                }

                return currentFaceApiKeyHeader;
            }
        }

        private static Dictionary<string, string> currentFaceApiKeyHeader = new Dictionary<string, string> { { RequestHeaderKey, ApiKey } };
    }
}
