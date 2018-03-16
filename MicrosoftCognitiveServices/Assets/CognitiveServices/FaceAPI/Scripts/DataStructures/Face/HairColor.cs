using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct HairColor
    {
        public HairColorType HairColorType
        {
            get
            {
                return (HairColorType)Enum.Parse(typeof(HairColorType), color);
            }
            set
            {
                color = value.ToStringCamelCase();
            }
        }
        public string color;
        public double confidence;
    }
}