using System;

namespace Microsoft.Cognitive.DataStructures.Person
{
    [Serializable]
    public class CreatePerson
    {
        public string name;
        public string userData;

        public CreatePerson(string _name, string _userData)
        {
            name = _name;
            userData = _userData;
        }
    }
}
