using System;

namespace Microsoft.Cognitive.DataStructures.PersonGroup
{
    [Serializable]
    public struct TrainingStatus
    {
        public StatusType StatusType
        {
            get
            {
                if (string.IsNullOrEmpty(status)) return StatusType.failed;
                return (StatusType)Enum.Parse(typeof(StatusType), status);
            }
            set
            {
                status = value.ToStringCamelCase();
            }
        }

        public string status;
        public DateTime createdDateTime;
        public DateTime lastActionDateTime;
        public string message;
    }
}