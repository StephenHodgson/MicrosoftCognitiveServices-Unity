using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct IdentifyResult
    {
        public string faceId;

        public Candidate[] candidates;
    }
}