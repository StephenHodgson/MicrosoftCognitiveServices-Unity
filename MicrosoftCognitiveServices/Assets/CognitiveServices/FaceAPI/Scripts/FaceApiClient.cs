using System.Collections.Generic;

namespace Microsoft.Cognitive.Face
{
    public static class FaceApiClient
    {
        private const string RequestHeaderKey = "Ocp-Apim-Subscription-Key";
        public static Region ResourceRegion { get; set; } = Region.WestUs;
        public static string FaceApiKey { get; set; } = "16ea7f10deab4828a098e5053b79a0e6";
        public static Dictionary<string, string> FaceApiKeyHeader
        {
            get
            {
                if (FaceApiKey != currentFaceApiKeyHeader[RequestHeaderKey])
                {
                    currentFaceApiKeyHeader = new Dictionary<string, string> { { RequestHeaderKey, FaceApiKey } };
                }

                return currentFaceApiKeyHeader;
            }
        }

        private static Dictionary<string, string> currentFaceApiKeyHeader = new Dictionary<string, string> { { RequestHeaderKey, FaceApiKey } };
    }
}
