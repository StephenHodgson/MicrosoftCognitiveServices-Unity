using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct Blur
    {
        public BlurLevelType BlurLevelType
        {
            get
            {
                return (BlurLevelType)Enum.Parse(typeof(BlurLevelType), blurLevel);
            }
            set
            {
                blurLevel = value.ToStringCamelCase();
            }
        }
        public string blurLevel;
        public double value;
    }
}