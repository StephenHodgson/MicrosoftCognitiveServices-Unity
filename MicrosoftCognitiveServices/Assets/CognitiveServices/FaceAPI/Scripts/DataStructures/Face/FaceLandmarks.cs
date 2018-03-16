using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct FaceLandmarks
    {
        public FeatureCoordinate pupilLeft;
        public FeatureCoordinate pupilRight;
        public FeatureCoordinate noseTip;
        public FeatureCoordinate mouthLeft;
        public FeatureCoordinate mouthRight;
        public FeatureCoordinate eyebrowLeftOuter;
        public FeatureCoordinate eyebrowLeftInner;
        public FeatureCoordinate eyeLeftOuter;
        public FeatureCoordinate eyeLeftTop;
        public FeatureCoordinate eyeLeftBottom;
        public FeatureCoordinate eyeLeftInner;
        public FeatureCoordinate eyebrowRightInner;
        public FeatureCoordinate eyebrowRightOuter;
        public FeatureCoordinate eyeRightInner;
        public FeatureCoordinate eyeRightTop;
        public FeatureCoordinate eyeRightBottom;
        public FeatureCoordinate eyeRightOuter;
        public FeatureCoordinate noseRootLeft;
        public FeatureCoordinate noseRootRight;
        public FeatureCoordinate noseLeftAlarTop;
        public FeatureCoordinate noseRightAlarTop;
        public FeatureCoordinate noseLeftAlarOutTip;
        public FeatureCoordinate noseRightAlarOutTip;
        public FeatureCoordinate upperLipTop;
        public FeatureCoordinate upperLipBottom;
        public FeatureCoordinate underLipTop;
        public FeatureCoordinate underLipBottom;
    }
}