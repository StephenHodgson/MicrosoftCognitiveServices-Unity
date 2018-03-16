using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct Hair
    {
        public double bald;
        public bool invisible;
        public HairColor[] hairColor;
    }
}