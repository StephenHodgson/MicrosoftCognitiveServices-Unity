using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct Accessory
    {
        public AccessoryType AccessoryType
        {
            get
            {
                return (AccessoryType)Enum.Parse(typeof(AccessoryType), type);
            }
            set
            {
                type = value.ToStringCamelCase();
            }
        }
        public string type;
        public double confidence;
    }
}