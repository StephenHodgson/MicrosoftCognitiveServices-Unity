using System;

namespace Microsoft.Cognitive.Search.Images.DataStructures {
    [Serializable]

    public struct ImageInfo
    {
        public string name;
        public string webSearchUrl;
        public string webSearchUrlPingSuffix;
        public string thumbnailUrl;
        public string datePublished;
        public string contentUrl;
        public string hostPageUrl;
        public string hostPageUrlPingSuffix;
        public string contentSize;
        public string encodingFormat;
        public string hostPageDisplayUrl;
        public int width;
        public int height;
        public ThumbnailInfo Thumbnail;
        public string imageInsightsToken;
        public string imageId;
        public string accentColor;
    }
}