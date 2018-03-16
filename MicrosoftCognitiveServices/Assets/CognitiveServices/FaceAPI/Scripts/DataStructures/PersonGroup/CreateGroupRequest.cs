using System;

namespace Microsoft.Cognitive.DataStructures.PersonGroup
{
    [Serializable]
    public class CreateGroupRequest
    {
        public string name;
        public string userData;

        public CreateGroupRequest(string _name, string _userData)
        {
            name = _name;
            userData = _userData;
        }
    }
}