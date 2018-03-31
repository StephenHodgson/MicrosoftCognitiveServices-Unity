using System;

namespace Microsoft.Cognitive.Search.Images.DataStructures
{
    [Serializable]
    public struct ImageSearchInfo
    {
        public string _type;
        public InstrumentationInfo instrumentation;
        public string readLink;
        public string webSearchUrl;
        public string webSearchUrlPingSuffix;
        public int totalEstimatedMatches;
        public ImageInfo[] value;
    }
}
