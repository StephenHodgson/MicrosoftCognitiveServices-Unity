using System;
using Microsoft.Cognitive.DataStructures.Person;

namespace Microsoft.Cognitive.DataStructures.PersonGroup
{
    [Serializable]
    public struct PersonGroupInfo
    {
        public string personGroupId;
        public string name;
        public string userData;
        public PersonInfo[] People { get; set; }
        public TrainingStatus TrainingStatus { get; set; }
    }
}
