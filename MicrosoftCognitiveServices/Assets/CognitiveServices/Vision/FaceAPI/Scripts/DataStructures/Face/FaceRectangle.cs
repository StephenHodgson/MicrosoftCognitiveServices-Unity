using System;

namespace Microsoft.Cognitive.DataStructures.Face
{
    [Serializable]
    public class FaceRectangle
    {
        public int width;
        public int height;
        public int left;
        public int top;

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", left, top, width, height);
        }
    }
}
