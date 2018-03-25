using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct IdentifyRequest
    {
        public string[] faceIds;
        public string personGroupId;
        public int maxNumOfCandidatesReturned;
        public float confidenceThreshold;

        public IdentifyRequest(string[] _faceIds, string _personGroupId, int _maxNumOfCandidatesReturned, float _confidenceThreshold)
        {
            faceIds = _faceIds;
            personGroupId = _personGroupId;
            maxNumOfCandidatesReturned = _maxNumOfCandidatesReturned;
            confidenceThreshold = _confidenceThreshold;
        }
    }
}
