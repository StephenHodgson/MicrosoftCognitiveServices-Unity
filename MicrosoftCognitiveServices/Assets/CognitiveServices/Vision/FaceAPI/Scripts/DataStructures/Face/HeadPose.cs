using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public struct HeadPose
    {
        public double roll;
        public double yaw;
        public double pitch;
    }
}