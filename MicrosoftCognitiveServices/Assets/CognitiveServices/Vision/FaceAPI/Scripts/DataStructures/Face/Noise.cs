using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct Noise
    {
        public NoiseLevelType NoiseLevelType
        {
            get
            {
                return (NoiseLevelType)Enum.Parse(typeof(NoiseLevelType), noiseLevel);
            }
            set
            {
                noiseLevel = value.ToStringCamelCase();
            }
        }
        public string noiseLevel;
        public double value;
    }
}