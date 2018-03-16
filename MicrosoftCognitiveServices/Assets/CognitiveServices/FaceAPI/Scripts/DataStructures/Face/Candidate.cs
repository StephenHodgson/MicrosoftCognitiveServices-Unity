using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct Candidate
    {
        public string personId;

        public double confidence;
    }
}