using System.Collections.Generic;

namespace Microsoft.Cognitive.Vision.Face
{
    public static class FaceApiClient
    {
        private const string RequestHeaderKey = "Ocp-Apim-Subscription-Key";
        public static Region ResourceRegion { get; set; } = Region.WestUs;
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
