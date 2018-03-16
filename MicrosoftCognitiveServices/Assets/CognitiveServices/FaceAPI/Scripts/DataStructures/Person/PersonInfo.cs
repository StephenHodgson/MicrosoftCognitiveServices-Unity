using System;

namespace Microsoft.Cognitive.DataStructures.Person
{
    [Serializable]
    public struct PersonInfo
    {
        public string personId;
        public string[] persistedFaceIds;
        public string name;
        public string userData;
    }
}
