using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct EmotionScores
    {
        public float anger;
        public float contempt;
        public float disgust;
        public float fear;
        public float happiness;
        public float neutral;
        public float sadness;
        public float surprise;
    }
}