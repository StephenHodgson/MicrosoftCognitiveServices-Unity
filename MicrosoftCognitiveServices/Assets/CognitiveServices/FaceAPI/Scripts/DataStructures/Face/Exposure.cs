using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct Exposure
    {
        public ExposureLevelType ExposureLevelType
        {
            get
            {
                return (ExposureLevelType)Enum.Parse(typeof(ExposureLevelType), exposureLevel);
            }
            set
            {
                exposureLevel = value.ToStringCamelCase();
            }
        }
        public string exposureLevel;
        public double value;
    }
}