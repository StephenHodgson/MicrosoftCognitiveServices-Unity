using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct Occlusion
    {
        public bool foreheadOccluded;
        public bool eyeOccluded;
        public bool mouthOccluded;
    }
}