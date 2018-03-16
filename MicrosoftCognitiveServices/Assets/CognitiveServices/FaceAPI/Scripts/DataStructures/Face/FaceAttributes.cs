using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct FaceAttributes
    {
        public double age;
        public string gender;
        public HeadPose headPose;
        public double smile;
        public FacialHair facialHair;
        public EmotionScores emotion;
        public GlassesType GlassesType
        {
            get
            {
                return (GlassesType)Enum.Parse(typeof(GlassesType), glasses);
            }
            set
            {
                glasses = value.ToStringCamelCase();
            }
        }
        public string glasses;
        public Blur blur;
        public Exposure exposure;
        public Noise noise;
        public Makeup makeup;
        public Accessory[] accessories;
        public Occlusion occlusion;
        public Hair hair;
    }
}