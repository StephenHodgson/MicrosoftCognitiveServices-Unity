using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct FaceInfo
    {
        public string faceId;
        public FaceRectangle faceRectangle;
        public FaceLandmarks faceLandmarks;
    }

    [Serializable]
    public struct PersistedFaceInfo
    {
        public string persistedFaceId;
    }
}